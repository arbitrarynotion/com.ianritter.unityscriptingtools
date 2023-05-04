using System;
using System.Collections.Generic;
using System.Linq;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.ToolingConstants;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.MetaData.MetaDataGathering;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger
{
    [CreateAssetMenu(menuName = LoggerAssetMenuName)]
    public class CustomLogger : ScriptableObject
    {
        [SerializeField] private bool showLogs = true;
        [SerializeField] private bool useClassPrefix = true;
        [SerializeField] private bool boldMethods = true;
        [SerializeField] private bool boldBlockMethods = true;
        [SerializeField] private bool nicifiedNames = true;
        [SerializeField] private bool includeStackTrace = false;
        [SerializeField] private bool fullPathName = false;
        [SerializeField] private string targetClass = "";
        [SerializeField] private string targetMethod  = "";
        
        // [SerializeField] [Range( 5, 20 )] private int maxPrefixCharacters = 10;
        

        [SerializeField] private CustomLoggerSymbol logPrefix;
        [SerializeField] private CustomLoggerSymbol blockDivider;
        [SerializeField] private CustomLoggerSymbol indentMarker;
        [SerializeField] private CustomLoggerSymbol methodDividers;
        [SerializeField] private CustomLoggerSymbol logBlockStart;
        [SerializeField] private CustomLoggerSymbol logBlockEnd;
        [SerializeField] private CustomLoggerSymbol logEventPrefix;
        
        [SerializeField] private Color blockMethodColor;
        [SerializeField] private Color methodColor;
        
        // [SerializeField] private Texture buttonTexture;

        private string _blockMethodHexColor;
        private string _methodHexColor;

        private string _lastCallingClass;

        private readonly Object _sender;
        
        private Stack<MethodEntry> _methodStack = new Stack<MethodEntry>();
        
        private int _blockTabLevel;

        private int _lineNumber;
        

        public CustomLogger( Object sender )
        {
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
            _lineNumber = 0;
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
        public void LogStart( bool blockStart = false, string introMessage = "", CustomLogType logType = CustomLogType.Standard )
        {
            if ( !LogAllowed() ) return;
            
            // [System.Runtime.CompilerServices.CallerMemberName] string memberName = "";

            // Must get the indent string before pushing the method entry so its indent is not included.
            string indent = GetIndentString();
            
            // Add new method entry to the stack.
            PushMethodEntry( blockStart, logType );
            
            // Print log using indent that does not include an indent for this new method entry.
            string message = GetLogEndCapMessage( GetCurrentMethodEntry(), logBlockStart, GetCurrentCallingClassName() );
            PrintLog( $"{indent}{message}", logType );
            
            IncrementTabLevel();

            if ( introMessage.Equals( "" ) ) return;
            
            // Used GetIndentString here because we want the indent from this log start to be included.
            string introMarker = ApplyTextColor( "<b>↳</b> ", blockStart ? _blockMethodHexColor : _methodHexColor );
            PrintLog( $"{GetIndentString()}{introMarker} {introMessage}", logType );
        }

        /// <summary>
        /// Print the ending line of a log sequence preceded by a Log Block End symbol. If and end message is provided,
        /// it will be printed as a standard log before the end log line is printed. If this ends a block, it will
        /// automatically be encapsulated in Log Block Divider symbols.
        /// </summary>
        public void LogEnd( string endMessage = "" )
        {
            if ( !LogAllowed() ) return;
            
            CustomLogType logType = GetCurrentMethodEntryLogType();
            string indent = GetIndentString();
            
            // Pop method entry from the stack and cache it's calling class.
            MethodEntry methodEntry = PopMethodEntry();
            _lastCallingClass = methodEntry.CallingClassName;

            if ( !endMessage.Equals( "" ) )
            {
                string introMarker = ApplyTextColor( "<b>↱</b> ", methodEntry.IsBlockStart() ? _blockMethodHexColor : _methodHexColor );
                PrintLog( $"{indent}{introMarker} {endMessage}", logType );
            }
            
            DecrementTabLevel();
            
            indent = GetIndentString();
            string message = GetLogEndCapMessage( methodEntry, logBlockEnd, _lastCallingClass );
            
            PrintLog( $"{indent}{message}", logType );

            if ( _methodStack.Count == 0 ) _blockTabLevel = 0;
        }

        public void LogEvent( string message = "" )
        {
            if ( !LogAllowed() ) return;

            string methodName = GetMethodName( 2 );
            methodName = nicifiedNames ? NicifyVariableName( methodName ) : methodName;
            message = BuildPrefixedLogMessage( logEventPrefix, $"{methodName}: {message}");
            PrintLog( $"{GetIndentString()}{message}" );
        }

        public void Log( string message )
        {
            if ( !LogAllowed() ) return;

            PrintLog( $"{GetIndentString()}{message}", GetCurrentMethodEntryLogType() );
        }
        
        public void Log( string message, CustomLogType logType )
        {
            if ( !LogAllowed() ) return;

            PrintLog( $"{GetIndentString()}{message}", logType );
        }

        public void LogIndentStart( string message, bool startHere = false, int incrementAmount = 1 )
        {
            if ( !LogAllowed() ) return;
            
            CustomLogType methodLogType = GetCurrentMethodEntryLogType();

            if ( startHere )
            {
                IncrementMethodIndent( incrementAmount );
                PrintLog( $"{GetIndentString()}{message}", methodLogType );
                return;
            }
            
            PrintLog( $"{GetIndentString()}{message}", methodLogType );
            IncrementMethodIndent( incrementAmount );
        }
        
        /// <summary>
        /// Print log where all preceding logs will be indented by 1. If startHere is true, this log will
        /// also be indented.
        /// </summary>
        public void LogIndentStart( string message, CustomLogType logType, bool startHere = false, int incrementAmount = 1 )
        {
            if ( !LogAllowed() ) return;

            if ( startHere )
            {
                IncrementMethodIndent( incrementAmount );
                PrintLog( $"{GetIndentString()}{message}", logType );
                return;
            }
            
            PrintLog( $"{GetIndentString()}{message}", logType );
            IncrementMethodIndent( incrementAmount );
        }
        
        
        public void LogIndentEnd( string message, int decrementAmount = 1, bool startHere = false )
        {
            if ( !LogAllowed() ) return;

            CustomLogType methodLogType = GetCurrentMethodEntryLogType();

            if ( startHere )
            {
                DecrementMethodIndent( decrementAmount );
                PrintLog( $"{GetIndentString()}{message}", methodLogType );
                return;
            }
            
            PrintLog( $"{GetIndentString()}{message}", methodLogType );
            DecrementMethodIndent( decrementAmount );
        }

        /// <summary>
        /// Print log where all preceding logs will has their indent decreased by a default of 1 unless the amount is
        /// specified via decrementAmount.
        /// </summary>
        public void LogIndentEnd( string message, CustomLogType logType, bool startHere = false, int decrementAmount = 1 )
        {
            if ( !LogAllowed() ) return;

            if ( startHere )
            {
                DecrementMethodIndent( decrementAmount );
                PrintLog( $"{GetIndentString()}{message}", logType );
                return;
            }
            
            PrintLog( $"{GetIndentString()}{message}", logType );
            DecrementMethodIndent( decrementAmount );
        }
        
        public void LogOneTimeIndent( string message, int incrementAmount = 1 )
        {
            if ( !LogAllowed() ) return;
            
            CustomLogType methodLogType = GetCurrentMethodEntryLogType();

            IncrementMethodIndent( incrementAmount );
            PrintLog( $"{GetIndentString()}{message}", methodLogType );
            DecrementMethodIndent( incrementAmount );
        }

        /// <summary>
        /// Print log that is indented by 1, but where the indent doesn't carry over to further logs.
        /// </summary>
        public void LogOneTimeIndent( string message, CustomLogType logType, int incrementAmount = 1 )
        {
            if ( !LogAllowed() ) return;
            
            IncrementMethodIndent( incrementAmount );
            PrintLog( $"{GetIndentString()}{message}", logType );
            DecrementMethodIndent( incrementAmount );
        }

        private string GetLogEndCapMessage( MethodEntry methodEntry, CustomLoggerSymbol loggerSymbol, string callingClass )
        {
            string formattedBlockStart = ApplyTextColor( $"{loggerSymbol.GetSymbol()} ", loggerSymbol.GetHexColor() );
            string methodName = methodEntry.MethodName;
            
            string callingClassPrefix = "";
            if ( useClassPrefix )
                callingClassPrefix = $" {GetColoredString( NicifyVariableName( callingClass ), logPrefix.GetHexColor() )} :: ";

            if ( !methodEntry.IsBlockStart() && boldMethods )
                methodName = $"<b>{methodName}</b>";

            string message = $"{formattedBlockStart} {callingClassPrefix}{GetColoredString( methodName, _methodHexColor )}";

            // Below is for only when the message is part of a block start.
            if ( !methodEntry.IsBlockStart() ) return message;
            
            string formattedBlockDivider = ApplyTextColor( $"{blockDivider.GetSymbol()} ", blockDivider.GetHexColor() );
            methodName = GetColoredString( methodName, _blockMethodHexColor );
            
            if ( boldBlockMethods )
                methodName = $"<b>{methodName}</b>";
            
            return $"{formattedBlockStart} <b>{formattedBlockDivider}</b> {callingClassPrefix}{methodName} <b>{formattedBlockDivider}</b>";
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

        private void PushMethodEntry( bool blockStart, CustomLogType logType )
        {
            _methodStack.Push( new MethodEntry( blockStart, nicifiedNames, logType,
                includeStackTrace, fullPathName, targetClass, targetMethod ) );
            // PrintCurrentMethodEntryList();
        }

        private MethodEntry GetCurrentMethodEntry()
        {
            if ( _methodStack.Count == 0 )
                throw new ArgumentOutOfRangeException($"Trying to peek an empty stack!");
            return _methodStack.Peek();
        }

        private CustomLogType GetCurrentMethodEntryLogType()
        {
            return _methodStack.Count == 0 ? CustomLogType.Standard : GetCurrentMethodEntry().GetLogType();
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

        public void IncrementMethodIndent( int amount = 1 )
        {
            for (int i = 0; i < amount; i++)
            {
                IncrementMethodTabLevel();
            }
        }
        
        public void DecrementMethodIndent( int amount = 1 )
        {
            for (int i = 0; i < amount; i++)
            {
                DecrementMethodTabLevel();
            }
        }
        
        private void DecrementMethodTabLevel() => GetCurrentMethodEntry().DecrementTabLevel();
        public void IncrementMethodTabLevel() => GetCurrentMethodEntry().IncrementTabLevel();
        
        private void IncrementTabLevel() => ++_blockTabLevel;
        
        private void DecrementTabLevel() => _blockTabLevel = Mathf.Max( ( _blockTabLevel - 1 ), 0 );
        
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

#endregion


#region Text Formatting
        
        public string ApplyNameFormatting( string variableName ) => nicifiedNames ? NicifyVariableName( variableName ) : variableName;

        private string ApplyPrefix( string message ) => 
            $"<color={logPrefix.GetHexColor()}>{logPrefix.GetSymbol()}</color> : {message}";

        private string ApplyTextColor( string message, string hexColor ) => 
            $"<color={hexColor}>{message}</color>";
        
        private string ApplyTextColor( CustomLoggerSymbol loggerSymbol ) => 
            $"<color={loggerSymbol.GetHexColor()}>{loggerSymbol.GetSymbol()}</color>";
        
        private string ApplyBlockSeparator( string message ) => 
            $"{ApplyTextColor( blockDivider )} {message} {ApplyTextColor( blockDivider )}";
            
        // The initial intent of this method was to make all calling class strings the same length. However, as the
        // debug output is in a variable font size, this turned out to be ineffective. It may be useful for other
        // situations with long names, however, so I'm keeping it here.
        // private string ConformNameToStringSize( string rawName )
        // {
        //     // Adding one extra character for the ellipsis.
        //     char[] outputName = new char[maxPrefixCharacters + 1];
        //     for (int i = 0; i < outputName.Length - 1; i++)
        //     {
        //         outputName[i] = ( i <= rawName.Length - 1 ) ? rawName[i] : '_';
        //     }
        //
        //     outputName[maxPrefixCharacters] = ( rawName.Length > maxPrefixCharacters ) ? '…' : '_';
        //     string formattedOutputName = $"{new string( outputName ),-15}";
        //     return formattedOutputName;
        // }

#endregion


#region Log Output

        private void PrintLog( string message, CustomLogType type = CustomLogType.Standard )
        {
            // string numberedMessage = $"{_lineNumber++.ToString()}){message}";
            // string numberedMessage = $": {_lineNumber++.ToString("00")} : {ApplyPrefix( message )}";
            message = ApplyPrefix( message );
            switch (type)
            {
                case CustomLogType.Standard:
                    Debug.Log( message, _sender );
                    break;
                case CustomLogType.Warning:
                    Debug.LogWarning( message, _sender );
                    break;
                case CustomLogType.Error:
                    Debug.LogError( message, _sender );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( type ), type, null );
            }
        }

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
                // "――►",
                "►", // Note that the left and right arrows may not display the same size here but they match in Unity's debug output.
                new Color( 0.29803923f, 0.8f, 0.29803923f, 1f ) 
            );
            logBlockEnd = new CustomLoggerSymbol( 
                true,
                // "←", 
                // "◀――", 
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
