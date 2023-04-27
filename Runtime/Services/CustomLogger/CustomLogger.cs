using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using static ToolingConstants;
using static Services.TextFormatting.TextFormat;
using Debug = UnityEngine.Debug;

namespace Services.CustomLogger
{
    // public interface ICustomLogger
    // {
    //     void LogStart( MethodBase methodBase, bool blockStart = false );
    //
    //     void LogEnd( MethodBase methodBase, bool blockEnd = false );
    //     
    //     void Log( string message, bool increaseTabLevel = false, bool oneTimeIncrement = false );
    //
    //     void LogEvent( MethodBase methodBase, bool increaseTabLevel = false );
    //
    //     void LogWarning( string message, bool increaseTabLevel = false );
    //
    //     void LogError( string message, bool increaseTabLevel = false );
    //     
    //     void DecrementMethodTabLevel();
    //     
    //     void IncrementMethodTabLevel();
    // }
    
    
    
    [CreateAssetMenu(menuName = LoggerAssetMenuName)]
    public class CustomLogger : ScriptableObject
    {
        private class MethodEntry
        {
            // The rawName is not currently necessary but may be used to include the current method rawName.
            public readonly string MethodName;
            public readonly string CallingClassName;
            private readonly bool _blockStart;
            private int _tabLevel;

            // 0 = GetMethodName, 1 = MethodEntry.ctor, 2 = PushMethodEntry, 3 = LogStart, 4 = CallingClassMethod
            private const int StackTraceIndex = 4;

            // Had to do nicify names first so the constructor modification didn't get processed by that algorithm.
            public MethodEntry( bool blockStart, bool nicifyName )
            {
                _blockStart = blockStart;
                
                MethodName = GetMethodName( nicifyName );
                CallingClassName = GetCallingClassName();
                // Debug.Log( $"MethodName Comparison:" );
                // Debug.Log( $"    CallingClass: {GetColoredStringGreen( CallingClassName )}" );
                // Debug.Log( $"    StackTrace MethodName: {GetColoredStringYellow( GetMethodName() )}" );
                if ( !MethodName.Equals( "ctor" ) ) return;
                
                // PrintStackTrace();
                MethodName = $"Constructor ({NicifyVariableName( CallingClassName )})";
            }

            public bool IsBlockStart() => _blockStart;

            private string GetMethodName( bool nicifyName )
            {
                var stackTrace = new StackTrace( true );
                string fullMethodName = stackTrace.GetFrames()[StackTraceIndex].GetMethod().ToString();
                string methodNameWithoutReturn = fullMethodName.Split( ' ' ).Last();
                string cleanMethodName = methodNameWithoutReturn.Split( '(' )[0];
                return nicifyName ? NicifyVariableName( cleanMethodName ) : cleanMethodName;
                // return stackTrace.GetFrames()[StackTraceIndex].GetMethod().ToString();
            }

            public void IncrementTabLevel() => ++_tabLevel;
            public void DecrementTabLevel() => _tabLevel = Mathf.Max( 0, ( _tabLevel - 1 ) );

            public int GetTabLevel() => _tabLevel;
            
            private string GetCallingClassName()
            {
                var stackTrace = new StackTrace( true );
                // Note that the calling class is on step back in the stacktrace CallingClass -> LogStart(), so index is 1.
                // However, the trace has the entire path rawName so it needs to be Split by '\', then the ".cs" has to be
                // removed from the end of the class rawName using another Split.
                // return stackTrace.GetFrames()[2].GetFileName().Split( '\\' ).Last().Split( '.' )[0];
                return ExtractFilenameString( stackTrace.GetFrames()[StackTraceIndex].GetFileName() );
            }

            private string ExtractFilenameString( string fullPathOfFilename ) => 
                fullPathOfFilename.Split( '\\' ).Last().Split( '.' )[0];

            private void PrintStackTrace()
            {
                var stackTrace = new StackTrace( true );
                foreach ( StackFrame frame in stackTrace.GetFrames() )
                {
                    Debug.Log( $"FileName: {GetColoredStringOrange( ExtractFilenameString( frame.GetFileName() ) )}");
                    Debug.Log( $"    Method: {GetColoredStringYellow( frame.GetMethod().ToString() )}, ");
                    Debug.Log( $"    Line: {frame.GetFileLineNumber().ToString()}, ");
                    Debug.Log( $"    Column: {frame.GetFileColumnNumber().ToString()}" );
                }
            }
        }
        
        [SerializeField] private bool showLogs = true;
        [SerializeField] private bool useClassPrefix = true;
        [SerializeField] private bool boldMethods = true;
        [SerializeField] private bool boldBlockMethods = true;
        [SerializeField] [Range( 5, 20 )] private int maxPrefixCharacters = 10;
        
        [SerializeField] private bool nicifiedNames = true;

        [SerializeField] private CustomLoggerSymbol logPrefix;
        [SerializeField] private CustomLoggerSymbol blockDivider;
        [SerializeField] private CustomLoggerSymbol indentMarker;
        [SerializeField] private CustomLoggerSymbol methodDividers;
        [SerializeField] private CustomLoggerSymbol logBlockStart;
        [SerializeField] private CustomLoggerSymbol logBlockEnd;
        [SerializeField] private CustomLoggerSymbol logEventPrefix;
        
        [SerializeField] private Color blockMethodColor;
        [SerializeField] private Color methodColor;

        private string _blockMethodHexColor;
        private string _methodHexColor;

        private string _lastCallingClass;

        private readonly Object _sender;
        
        // Method calls are stacked to keep track of when the block tab level needs to be updated.
        private Stack<MethodEntry> _methodStack = new Stack<MethodEntry>();
        
        // Tracks the overall indent of the debug readout sequence.
        private int _blockTabLevel;
        

        public CustomLogger( Object sender )
        {
            Debug.Log( "Custom Logger constructor called..." );
            _sender = sender;
        }
        
        private void OnEnable()
        {
            UpdatePrefixColor();
            UpdatePrefixColor();
            if ( _methodStack == null )
                _methodStack = new Stack<MethodEntry>();
            _methodStack.Clear();
            _blockTabLevel = 0;
        }

        private void OnValidate() => UpdatePrefixColor();
        
        private void UpdatePrefixColor()
        {
            logPrefix.UpdateHexColor();
            blockDivider.UpdateHexColor();
            logBlockStart.UpdateHexColor();
            indentMarker.UpdateHexColor();
            methodDividers.UpdateHexColor();
            logBlockEnd.UpdateHexColor();
            logEventPrefix.UpdateHexColor();

            _blockMethodHexColor = $"#{ColorUtility.ToHtmlStringRGBA( blockMethodColor )}";
            _methodHexColor = $"#{ColorUtility.ToHtmlStringRGBA( methodColor )}";

        }


#region Log Methods
        
        /// <summary>
        /// Print the starting line of a log sequence preceded by a Log Block Start symbol where subsequent logs will be
        /// indented based on their depth in the call stack. If blockStart is true, log will be encapsulated in
        /// Log Block Divider symbols - note that the matching LogEnd will automatically include such encapsulation as well.
        /// </summary>
        public void LogStart( bool blockStart = false )
        {
            if ( !LogAllowed() ) return;

            // Must get the indent string before pushing the method entry so its indent is not included.
            string indent = GetIndentString();
            
            // Add new method entry to the stack.
            PushMethodEntry( blockStart );
            
            // Print log using indent that does not include an indent for this new method entry.
            string message = GetBlockLogMessage( GetCurrentMethodEntry(), logBlockStart, GetCurrentCallingClassName() );
            PrintStandardLog( $"{indent}{message}" );
            
            IncrementTabLevel();
        }

        /// <summary>
        /// Print the ending line of a log sequence preceded by a Log Block End symbol. If this ends a block, it will
        /// automatically be encapsulated in Log Block Divider symbols.
        /// </summary>
        public void LogEnd()
        {
            if ( !LogAllowed() ) return;
            
            // Pop method entry from the stack and cache it's calling class.
            MethodEntry methodEntry = PopMethodEntry();
            _lastCallingClass = methodEntry.CallingClassName;
            
            DecrementTabLevel();
            
            string indent = GetIndentString();
            string message = GetBlockLogMessage( methodEntry, logBlockEnd, _lastCallingClass );
            
            PrintStandardLog( $"{indent}{message}" );

            if ( _methodStack.Count == 0 ) _blockTabLevel = 0;
        }

        private string GetBlockLogMessage( MethodEntry methodEntry, CustomLoggerSymbol loggerSymbol, string callingClass )
        {
            string formattedBlockStart = ApplyTextColor( $"{loggerSymbol.GetSymbol()} ", loggerSymbol.GetHexColor() );
            string methodName = methodEntry.MethodName;
            
            string callingClassPrefix = "";
            if ( useClassPrefix )
            {
                // string callingClassName = isEndLog ? _lastCallingClass : GetCurrentCallingClassName();
                callingClassPrefix = $" {GetColoredString( NicifyVariableName( callingClass ), logPrefix.GetHexColor() )} :: ";
            }
            
            if ( !methodEntry.IsBlockStart() && boldMethods )
                methodName = $"<b>{methodName}</b>";

            string message = $"{formattedBlockStart} {callingClassPrefix}{GetColoredString( methodName, _methodHexColor )}";

            if ( !methodEntry.IsBlockStart() ) return message;
            
            string formattedBlockDivider = ApplyTextColor( $"{blockDivider.GetSymbol()} ", blockDivider.GetHexColor() );
            methodName = GetColoredString( methodName, _blockMethodHexColor );
            
            if ( boldBlockMethods )
                methodName = $"<b>{methodName}</b>";
            
            return $"{formattedBlockDivider} {formattedBlockStart} {methodName} {formattedBlockDivider}";
        }

        public void LogEvent( string message = "" )
        {
            if ( !LogAllowed() ) return;
            // if ( increaseTabLevel ) IncrementMethodTabLevel();
            message = BuildPrefixedLogMessage( logEventPrefix, $"{GetMethodName()}: {message}");
            PrintStandardLog( $"{GetIndentString()}{message}" );
        }
        
        private string GetMethodName( bool nicifyName = true )
        {
            var stackTrace = new StackTrace( true );
            string fullMethodName = stackTrace.GetFrames()[2].GetMethod().ToString();
            string methodNameWithoutReturn = fullMethodName.Split( ' ' ).Last();
            string cleanMethodName = methodNameWithoutReturn.Split( '(' )[0];
            return nicifyName ? NicifyVariableName( cleanMethodName ) : cleanMethodName;
            // return stackTrace.GetFrames()[StackTraceIndex].GetMethod().ToString();
        }

        public void Log( string message )
        {
            if ( !LogAllowed() ) return;
            
            PrintStandardLog( $"{GetIndentString()}{message}" );
        }
        
        /// <summary>
        /// Print log where all preceding logs will be indented by 1. If startHere is true, this log will
        /// also be indented.
        /// </summary>
        public void LogIndentStart( string message, bool startHere = false )
        {
            if ( !LogAllowed() ) return;

            message = $"{GetIndentString()}{message}";
            if ( startHere )
            {
                IncrementMethodTabLevel();
                PrintStandardLog( message );
                return;
            }
            
            PrintStandardLog( message );
            IncrementMethodTabLevel();
        }

        /// <summary>
        /// Print log where all preceding logs will has their indent decreased by a default of 1 unless the amount is
        /// specified via decrementAmount.
        /// </summary>
        public void LogIndentEnd( string message, int decrementAmount = 1 )
        {
            if ( !LogAllowed() ) return;

            PrintStandardLog( $"{GetIndentString()}{message}" );
            for (int i = 0; i < decrementAmount; i++)
            {
                DecrementMethodTabLevel();
            }
        }
        
        // Indent this line only.
        //     Increment indent before this log: 
        //     Decrement indent after this log.
        // incrementIndent = true, startHere = true, decrementIndentAfter = true, decrementAmount = 1
        
        /// <summary>
        /// Print log that is indented by 1, but where the indent doesn't carry over to further logs.
        /// </summary>
        public void LogOneTimeIndent( string message )
        {
            if ( !LogAllowed() ) return;

            IncrementMethodTabLevel();
            PrintStandardLog( $"{GetIndentString()}{message}" );
            DecrementMethodTabLevel();
        }

        /// <summary>
        /// Print log that will use the yellow Warning symbol.
        /// </summary>
        public void LogWarning( string message )
        {
            if ( !LogAllowed() ) return;
            PrintWarningLog( $"{GetIndentString()}{message}" );
        }

        /// <summary>
        /// Print log that will use the red Error symbol.
        /// </summary>
        public void LogError( string message )
        {
            if ( !LogAllowed() ) return;
            PrintErrorLog( $"{GetIndentString()}{message}" );
        }

        private string BuildPrefixedLogMessage( CustomLoggerSymbol loggerSymbol, string methodName ) => 
            $"{ApplyTextColor( loggerSymbol.GetSymbol(), loggerSymbol.GetHexColor() )} {methodName}";

        private bool LogAllowed()
        {
            if ( !showLogs ) return false;
            if ( _methodStack == null ) _methodStack = new Stack<MethodEntry>();
            return true;
        }

#endregion

        
        


#region Method Entry Handling

        private void PushMethodEntry( bool blockStart )
        {
            _methodStack.Push( new MethodEntry( blockStart, nicifiedNames ) );
            // PrintCurrentMethodEntryList();
        }

        private MethodEntry GetCurrentMethodEntry()
        {
            if ( _methodStack.Count == 0 )
                throw new ArgumentOutOfRangeException($"Trying to peek an empty stack!");
            return _methodStack.Peek();
        }

        private string GetCurrentMethodName()
        {
            return _methodStack.Count == 0 ? "N/A" : GetCurrentMethodEntry().MethodName;
        }
        
        private string GetCurrentCallingClassName()
        {
            return _methodStack.Count == 0 ? _lastCallingClass : GetCurrentMethodEntry().CallingClassName;
        }

        private MethodEntry PopMethodEntry()
        {
            if ( _methodStack.Count <= 0 )
                throw new ArgumentOutOfRangeException($"Trying to pop an empty stack!");
            return _methodStack.Pop();
        }
        
        private void PrintCurrentMethodEntryList()
        {
            Debug.Log( $"MethodEntries Stack ({_methodStack.Count.ToString()} total):" );
            int count = 0;
            foreach ( MethodEntry methodEntry in _methodStack )
            {
                Debug.Log( $"    {count.ToString()}: {methodEntry.CallingClassName}.{methodEntry.MethodName}(), tabLevel: {methodEntry.GetTabLevel().ToString()}" );
                count++;
            }
        }

#endregion
        



#region Tabbing

        public void DecrementMethodIndent( int amount = 1 )
        {
            for (int i = 0; i < amount; i++)
            {
                DecrementMethodTabLevel();
            }
        }
        
        private void DecrementMethodTabLevel() => GetCurrentMethodEntry().DecrementTabLevel();
        private void IncrementMethodTabLevel() => GetCurrentMethodEntry().IncrementTabLevel();
        
        private void IncrementTabLevel() => ++_blockTabLevel;
        
        private void DecrementTabLevel() => _blockTabLevel = Mathf.Max( ( _blockTabLevel - 1 ), 0 );

        // private string GetBlockIndent() => GetIndent( indentMarker, _blockTabLevel );
        // // private string GetBlockIndent() => GetIndent( indentMarker, _methodStack.Count );
        //
        // private string GetMethodIndent()
        // {
        //     if ( _methodStack == null ) Debug.LogWarning( "_methodStack is null!" );
        //     return GetIndent( methodDividers, ( _methodStack.Count > 0 ? GetCurrentMethodEntry().GetTabLevel() : 0 ) );
        // }

        // private string GetIndent( CustomLoggerSymbol loggerSymbol, int tabLevel )
        // {
        //     string divider = ApplyTextColor( $"{loggerSymbol.GetSymbol()} ", loggerSymbol.GetHexColor() );
        //     // string totalIndent = divider;
        //     string totalIndent = "";
        //     for (int i = 0; i < tabLevel; i++)
        //     {
        //         // string symbol = ( i < ( tabLevel - 1 ) ) ? divider : "";
        //         totalIndent += $"{LoggerTabText}{divider}";
        //     }
        //     return totalIndent;
        // }

        private string GetIndentString()
        {
            string indentString = "";
            // Iterate through methodEntry stack.
            //     Add a block divider for this method, then add a method divider for each tab level in this method.

            string formattedIndentMarker = ApplyTextColor( indentMarker );
            string formattedMethodDividers = ApplyTextColor( methodDividers );
            for (int i = _methodStack.Count - 1; i >= 0; i--)
            {
                MethodEntry methodEntry = _methodStack.ElementAt( i );
                indentString += $"{LoggerTabText}{formattedIndentMarker} ";
                for (int j = 0; j < methodEntry.GetTabLevel(); j++)
                {
                    indentString += $"{LoggerTabText}{formattedMethodDividers} ";
                }
            }
            

            return indentString;
        }
        
        // private string ApplyIndent( string message ) => $"{GetBlockIndent()}{GetMethodIndent()}{message}";
        private string ApplyIndent( string message ) => $"{GetIndentString()}{message}";

#endregion


#region Text Formatting
        
        public string ApplyNameFormatting( string variableName ) => nicifiedNames ? NicifyVariableName( variableName ) : variableName;

        private string ApplyPrefix( string message )
        {
            string formattedLogPrefix = $"<color={logPrefix.GetHexColor()}>{logPrefix.GetSymbol()}</color> :";
            // if ( useClassPrefix )
            // {
            //     string callingClassName = isEndLog ? _lastCallingClass : GetCurrentCallingClassName();
            //     formattedLogPrefix += $" {GetColoredString( callingClassName, logPrefix.GetHexColor() )}:";
            // }
            return $"{formattedLogPrefix} {message}";
        }

        private string ConformNameToStringSize( string rawName )
        {
            // Adding one extra character for the ellipsis.
            char[] outputName = new char[maxPrefixCharacters + 1];
            for (int i = 0; i < outputName.Length - 1; i++)
            {
                outputName[i] = ( i <= rawName.Length - 1 ) ? rawName[i] : '_';
            }

            outputName[maxPrefixCharacters] = ( rawName.Length > maxPrefixCharacters ) ? '…' : '_';
            string formattedOutputName = $"{new string( outputName ),-15}";
            return formattedOutputName;
        }

        private string ApplyTextColor( string message, string hexColor ) => 
            $"<color={hexColor}>{message}</color>";
        
        private string ApplyTextColor( CustomLoggerSymbol loggerSymbol ) => 
            $"<color={loggerSymbol.GetHexColor()}>{loggerSymbol.GetSymbol()}</color>";
        
        private string ApplyBlockSeparator( string message ) => 
            $"{ApplyTextColor( blockDivider )} {message} {ApplyTextColor( blockDivider )}";
            // $"{GetColoredStringBlack( $"----------" )} {message} {GetColoredStringBlack( "----------" )}";

#endregion


#region Log Output

        private void PrintStandardLog( string message ) =>
            Debug.Log( ApplyPrefix( $"{message}" ), _sender );

        private void PrintWarningLog( string message ) =>
            Debug.LogWarning( ApplyPrefix( $"{message}" ), _sender );

        private void PrintErrorLog( string message ) =>
            Debug.LogError( ApplyPrefix( $"{message}" ), _sender );

#endregion
        
        /// <summary>
        /// Sets all CustomLogger settings to default values.
        /// </summary>
        public void SetValuesToDefault()
        {
            showLogs = true;
            nicifiedNames = true;

            logPrefix = new CustomLoggerSymbol( 
                true, 
                "Name_Here", 
                new Color( 0.21226415f, g: 0.8490566f, b: 0.75683147f, 1f ) 
            );
            blockDivider = new CustomLoggerSymbol( 
                true, 
                "―――――", 
                new Color( 0f, 0f, 0f, 1f ) 
            );
            indentMarker = new CustomLoggerSymbol( 
                true, 
                "|", 
                new Color( 0f, 0f, 0f, 1f ) 
            );
            methodDividers = new CustomLoggerSymbol( 
                true, 
                ".", 
                new Color( 0f, 0f, 0f, 1f ) 
            );
            logBlockStart = new CustomLoggerSymbol( 
                true, 
                // "→",
                "►", // Note that the left and right arrows may not display the same size here but they match in Unity's debug output.
                new Color( 0.29803923f, 0.8f, 0.29803923f, 1f ) 
            );
            logBlockEnd = new CustomLoggerSymbol( 
                true, 
                // "←", 
                "◀", 
                new Color( 0.29803923f, 0.6f, 0.9019608f, 1f ) 
            );
            logEventPrefix = new CustomLoggerSymbol( 
                true, 
                "***", 
                new Color( 0.9019608f, 0.5019608f, 0.2f, 1f ) 
            );

            blockMethodColor = new Color( 1f, 1f, 0f, 1f );
            methodColor = new Color( 0.9019608f, 0.5019608f, 0.2f, 1f );
        }
    }
}
