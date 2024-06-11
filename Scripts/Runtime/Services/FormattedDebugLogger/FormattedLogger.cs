using System;
using System.Collections.Generic;
using System.Linq;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.MetaData.MetaDataGathering;
using Object = UnityEngine.Object;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger
{
    [CreateAssetMenu( menuName = LoggerAssetMenuName )]
    public class FormattedLogger : ScriptableObject
    {
        private const string IntroMarkerSymbol = "↳";
        private const string OutMarkerSymbol = "↱";
        
        [SerializeField] private bool showLogs = true;
        [SerializeField] private bool padBlocks = true;
        [SerializeField] private bool useClassPrefix = true;
        [SerializeField] private bool boldBlockMethods = true;
        [SerializeField] private bool boldMethods = true;
        [SerializeField] private bool nicifiedNames = true;

        [SerializeField] private Color blockMethodColor = new Color( 1f, 1f, 0f, 1f );
        [SerializeField] private Color methodColor = new Color( 0.9019608f, 0.5019608f, 0.2f, 1f );
        [SerializeField] private Color unityEventsColor = new Color( 0f, 0f, 0f, 1f );
        [SerializeField] private Color focusColor = new Color( 0f, 1f, 1f, 1f );

        [SerializeField] private FormattedLoggerSymbol logPrefix = new FormattedLoggerSymbol
        (
            true,
            "Name_Here",
            new Color( 0.21226415f, 0.8490566f, 0.75683147f, 1f )
        );
        [SerializeField] private FormattedLoggerSymbol blockDivider = new FormattedLoggerSymbol
        (
            true,
            "―――――",
            new Color( 0.142f, 0.142f, 0.142f, 1f )
        );
        [SerializeField] private FormattedLoggerSymbol indentMarker = new FormattedLoggerSymbol
        (
            true,
            "|",
            new Color( 0.142f, 0.142f, 0.142f, 1f )
        );
        [SerializeField] private FormattedLoggerSymbol methodDividers = new FormattedLoggerSymbol
        (
            true,
            ".",
            new Color( 0.142f, 0.142f, 0.142f, 1f )
        );
        [SerializeField] private FormattedLoggerSymbol logBlockStart = new FormattedLoggerSymbol
        (
            true,

            // "→",
            // "――►",
            "►", // Note that the left and right arrows may not display the same size here but they match in Unity's debug output.
            new Color( 0.29803923f, 0.8f, 0.29803923f, 1f )
        );
        [SerializeField] private FormattedLoggerSymbol logBlockEnd = new FormattedLoggerSymbol
        (
            true,

            // "←", 
            // "◀――", 
            "◀",
            new Color( 0.29803923f, 0.6f, 0.9019608f, 1f )
        );
        [SerializeField] private FormattedLoggerSymbol logEventPrefix = new FormattedLoggerSymbol
            (
                true,
                "***",
                new Color( 0.9019608f, 0.5019608f, 0.2f, 1f )
            );
        [SerializeField] private FormattedLoggerSymbol focusAccent = new FormattedLoggerSymbol
        (
            true,
            "••••",
            new Color( 1f, 1f, 0f, 1f ) 
        );

        // [SerializeField] [Range( 5, 20 )] private int maxPrefixCharacters = 10;

        [SerializeField] private string targetClass = "";
        [SerializeField] private string targetMethod = "";

        [SerializeField] private bool fullPathName;
        [SerializeField] private bool includeStackTrace;

        private readonly Object _sender;
        private int _blockTabLevel;
        private string _lastCallingClass;
        private Stack<MethodEntry> _methodStack = new Stack<MethodEntry>();

        private string _blockMethodHexColor;
        private string _methodHexColor;

        public FormattedLogger( Object sender )
        {
            _sender = sender;
            SetValuesToDefault();
        }


#region Colors

        private void UpdatePrefixColor()
        {
            logPrefix.UpdateHexColor();
            blockDivider.UpdateHexColor();
            logBlockStart.UpdateHexColor();
            indentMarker.UpdateHexColor();
            methodDividers.UpdateHexColor();
            logBlockEnd.UpdateHexColor();
            logEventPrefix.UpdateHexColor();
            focusAccent.UpdateHexColor();

            _blockMethodHexColor = $"#{ColorUtility.ToHtmlStringRGBA( blockMethodColor )}";
            _methodHexColor = $"#{ColorUtility.ToHtmlStringRGBA( methodColor )}";
        }

#endregion


#region Log Output

        private void PrintLog( string message, FormattedLogType type = FormattedLogType.Standard )
        {
            // if ( !IsTargetMethod() ) return;

            // string numberedMessage = $"{_lineNumber++.ToString()}){message}";
            // string numberedMessage = $": {_lineNumber++.ToString("00")} : {ApplyPrefix( message )}";
            message = ApplyPrefix( message );
            switch (type)
            {
                case FormattedLogType.Standard:
                    Debug.Log( message, _sender );
                    break;
                case FormattedLogType.Warning:
                    Debug.LogWarning( message, _sender );
                    break;
                case FormattedLogType.Error:
                    Debug.LogError( message, _sender );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( type ), type, null );
            }
        }

#endregion
        

#region LifeCycle

        private void OnEnable()
        {
            UpdatePrefixColor();
            if( _methodStack == null )
                _methodStack = new Stack<MethodEntry>();
            _methodStack.Clear();
            _blockTabLevel = 0;
        }

        private void OnValidate() => UpdatePrefixColor();

#endregion


#region Log Methods

        public void BlankLine()
        {
            if( !LogAllowed() ) return;
            PrintLog( "", GetCurrentMethodEntryLogType() );
        }

        public void Log( string message )
        {
            if( !LogAllowed() ) return;

            PrintLog( $"{GetIndentString()}{message}", GetCurrentMethodEntryLogType() );
        }

        public void Log( string message, FormattedLogType logType )
        {
            if( !LogAllowed() ) return;

            PrintLog( $"{GetIndentString()}{message}", logType );
        }

        public void LogEvent( string message = "" )
        {
            if( !LogAllowed() ) return;

            string methodName = GetMethodName( 2 );
            methodName = nicifiedNames ? NicifyVariableName( methodName ) : methodName;
            message = BuildPrefixedLogMessage( logEventPrefix, $"{methodName}: {message}" );
            PrintLog( $"{GetIndentString()}{message}" );
        }

#region LogStartAndEnd

        public void LogStart( string introMessage ) => LogStart( false, false, introMessage, FormattedLogType.Standard );
        public void LogStart( bool blockStart = false, string introMessage = "" ) => LogStart( blockStart, false, introMessage, FormattedLogType.Standard );
        public void LogStart( bool blockStart, bool isFocused, string introMessage = "" ) => LogStart( blockStart, isFocused, introMessage, FormattedLogType.Standard );
        public void LogStart( bool blockStart, string introMessage, FormattedLogType logType ) => LogStart( blockStart, false, introMessage, logType );

        /// <summary>
        ///     Print the starting line of a log sequence preceded by a Log Block Start symbol where subsequent logs will be<br/>
        ///     indented based on their depth in the call stack. If blockStart is true, log will be encapsulated in<br/>
        ///     Log Block Divider symbols - note that the matching LogEnd will automatically include such encapsulation as well.
        /// </summary>
        private void LogStart( bool blockStart, bool isFocused, string introMessage, FormattedLogType logType )
        {
            if( !LogAllowed() ) return;

            // If pad blocks or is focused is selected, print a blank line.
            if ( ( blockStart && padBlocks ) || isFocused ) BlankLine();
            
            // Must get the indent string before pushing the method entry so its indent is not included.
            string indent = GetIndentString();

            // Add new method entry to the stack.
            PushMethodEntry( blockStart, isFocused, logType );

            // Print log using indent that does not include an indent for this new method entry.
            string message = GetLogEndCapMessage( GetCurrentMethodEntry(), logBlockStart, GetCurrentCallingClassName() );
            PrintLog( $"{indent}{message}", logType );

            IncrementBlockTabLevel();

            if( introMessage.Equals( "" ) ) return;

            // Used GetIndentString here because we want the indent from this log start to be included.
            // string color = isFocused ? focusAccent : ( blockStart ? _blockMethodHexColor : _methodHexColor );
            string introMarker = ApplyTextColor( $"<b>{IntroMarkerSymbol}</b> ", GetHexColorForMethodEntry( isFocused, blockStart ) );
            
            PrintLog( $"{GetIndentString()}{introMarker} {introMessage}", logType );
        }
        

        /// <summary>
        ///     Print the ending line of a log sequence preceded by a Log Block End symbol. If and end message is provided,
        ///     it will be printed as a standard log before the end log line is printed. If this ends a block, it will
        ///     automatically be encapsulated in Log Block Divider symbols.
        /// </summary>
        public void LogEnd( string endMessage = "" )
        {
            if( !LogAllowed() ) return;

            FormattedLogType logType = GetCurrentMethodEntryLogType();
            
            // Get the indent string after resetting the current method entry's tab level.
            string indent = "";
            if( !endMessage.Equals( "" ) )
            {
                ResetMethodTabLevelForLogEnd();
                indent = GetIndentString();
            }

            // Pop method entry from the stack and cache it's calling class.
            MethodEntry methodEntry = PopMethodEntry();
            _lastCallingClass = methodEntry.CallingClassName;

            // If an outro message was provided, print it out before the final tab decrement.
            if( !endMessage.Equals( "" ) )
            {
                string introMarker = ApplyTextColor( $"<b>{OutMarkerSymbol}</b> ", GetHexColorForMethodEntry( methodEntry.IsFocused(), methodEntry.IsBlockStart() ) );
                PrintLog( $"{indent}{introMarker} {endMessage}", logType );
            }

            // Decrement the block tab once to realign with the log start.
            DecrementBlockTabLevel();

            // Construct and output the log end cap.
            indent = GetIndentString();
            string message = GetLogEndCapMessage( methodEntry, logBlockEnd, _lastCallingClass );
            PrintLog( $"{indent}{message}", logType );
            
            // If pad blocks or is focused is selected, print a blank line.
            if ( ( methodEntry.IsBlockStart() && padBlocks ) || methodEntry.IsFocused() ) BlankLine();

            if( _methodStack.Count == 0 ) _blockTabLevel = 0;
        }


        private string GetLogEndCapMessage( MethodEntry methodEntry, FormattedLoggerSymbol loggerSymbol, string callingClass )
        {
            string methodName = GetFormattedMethodName( methodEntry );
            string callingClassPrefix = "";
            string formattedBlockStart = "";
            string formattedBlockDivider = "";
            
            if( useClassPrefix )
                callingClassPrefix = $" {GetColoredString( NicifyVariableName( callingClass ), logPrefix.GetHexColor() )} :: ";
            
            // Get the string that goes before the method name.
            formattedBlockStart = ApplyTextColor( $"{loggerSymbol.GetSymbol()} ", loggerSymbol.GetHexColor() );

            if( methodEntry.IsBlockStart() )
                formattedBlockDivider = ApplyTextColor( blockDivider );

            return $"{formattedBlockStart} <b>{formattedBlockDivider}</b> {callingClassPrefix}{methodName} <b>{formattedBlockDivider}</b>";
        }

        // Set the styling of the method name for a start or end log.
        private string GetFormattedMethodName( MethodEntry methodEntry )
        {
            string methodName = methodEntry.MethodName;

            if( methodEntry.IsFocused() )
                methodName = $"{focusAccent.GetSymbol()} {methodName} {focusAccent.GetSymbol()}";

            // Should the name be bold?
            // string methodName = ApplyMethodStyle( methodEntry, methodEntry.MethodName );
            
            // Should the name be bold?
            methodName = ApplyMethodStyle( methodEntry, methodName );

            // Set the color
            methodName = methodEntry.IsFocused() 
                ? ApplyTextColor( methodName, focusAccent.GetHexColor() ) 
                : ApplyTextColor( methodName, ( methodEntry.IsBlockStart() ? _blockMethodHexColor : _methodHexColor ) );

            return methodName;
        }

        private string ApplyMethodStyle( MethodEntry methodEntry, string methodName )
        {
            if( methodEntry.IsFocused() )
                return $"{ApplyBoldStyle( methodName )}";

            // Should the name be bold?
            if( methodEntry.IsBlockStart() ) 
                return boldBlockMethods 
                    ? ApplyBoldStyle( methodName ) 
                    : methodName;

            return boldMethods ? ApplyBoldStyle( methodName ) : methodName;
        }
        
        private string ApplyBoldStyle( string text ) => $"<b>{text}</b>";
        
#endregion
        

#region ObjectValidityLogs

        public void LogObjectAssignmentResult( string objectName, Object targetObject ) => 
            LogObjectAssignmentResult( objectName, targetObject, FormattedLogType.Standard );

        public void LogObjectAssignmentResult( string objectName, Object targetObject, FormattedLogType logType ) => 
            LogObjectAssignmentResult( objectName, targetObject == null, logType );

        public void LogObjectAssignmentResult( string objectName, bool failedIf, FormattedLogType logType )
        {
            if( !LogAllowed() ) return;

            PrintLog( $"{GetIndentString()}Loading of {objectName} {( failedIf ? $"{GetColoredStringFireBrick( "failed" )}" : $"{GetColoredStringGreenYellow( "succeeded" )}" )}.", logType );
        }

#endregion


#region IndentingLogs

        public void LogIndentStart( string message, bool startHere = false, int incrementAmount = 1 )
        {
            if( !LogAllowed() ) return;

            FormattedLogType methodLogType = GetCurrentMethodEntryLogType();

            if( startHere )
            {
                IncrementMethodIndent( incrementAmount );
                PrintLog( $"{GetIndentString()}{message}", methodLogType );
                return;
            }

            PrintLog( $"{GetIndentString()}{message}", methodLogType );
            IncrementMethodIndent( incrementAmount );
        }

        /// <summary>
        ///     Print log where all preceding logs will be indented by 1. If startHere is true, this log will
        ///     also be indented.
        /// </summary>
        public void LogIndentStart( string message, FormattedLogType logType, bool startHere = false, int incrementAmount = 1 )
        {
            if( !LogAllowed() ) return;

            if( startHere )
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
            if( !LogAllowed() ) return;

            FormattedLogType methodLogType = GetCurrentMethodEntryLogType();

            if( startHere )
            {
                DecrementMethodIndent( decrementAmount );
                PrintLog( $"{GetIndentString()}{message}", methodLogType );
                return;
            }

            PrintLog( $"{GetIndentString()}{message}", methodLogType );
            DecrementMethodIndent( decrementAmount );
        }

        /// <summary>
        ///     Print log where all preceding logs will has their indent decreased by a default of 1 unless the amount is
        ///     specified via decrementAmount.
        /// </summary>
        public void LogIndentEnd( string message, FormattedLogType logType, bool startHere = false, int decrementAmount = 1 )
        {
            if( !LogAllowed() ) return;

            if( startHere )
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
            if( !LogAllowed() ) return;

            FormattedLogType methodLogType = GetCurrentMethodEntryLogType();

            IncrementMethodIndent( incrementAmount );
            PrintLog( $"{GetIndentString()}{message}", methodLogType );
            DecrementMethodIndent( incrementAmount );
        }

        /// <summary>
        ///     Print log that is indented by 1, but where the indent doesn't carry over to further logs.
        /// </summary>
        public void LogOneTimeIndent( string message, FormattedLogType logType, int incrementAmount = 1 )
        {
            if( !LogAllowed() ) return;

            IncrementMethodIndent( incrementAmount );
            PrintLog( $"{GetIndentString()}{message}", logType );
            DecrementMethodIndent( incrementAmount );
        }

#endregion

#endregion


#region Helpers

        private string BuildPrefixedLogMessage( FormattedLoggerSymbol loggerSymbol, string methodName ) => $"{ApplyTextColor( loggerSymbol.GetSymbol(), loggerSymbol.GetHexColor() )} {methodName}";

        private bool LogAllowed()
        {
            if( !showLogs ) return false;

            // if ( !IsTargetClass() || !IsTargetMethod() ) return false;
            // Debug.Log( $"Checking {GetColoredStringYellow( GetCurrentMethodNameRaw() )}." );
            // if ( GetCurrentMethodNameRaw().Equals( targetMethod ) ) return false;
            if( _methodStack == null ) _methodStack = new Stack<MethodEntry>();
            return true;
        }

        private bool IsTargetClass()
        {
            if( _methodStack.Count == 0 ) return true;
            return targetClass.Equals( "" ) || GetCurrentMethodEntry().CallingClassName.Equals( targetClass );
        }

        private bool IsTargetMethod()
        {
            if( _methodStack.Count == 0 ) return true;

            // Debug.Log( $"Comparing {GetColoredStringYellow(GetCurrentMethodEntry().MethodNameRaw)} to {GetColoredStringOrange(targetMethod)}" );
            return targetMethod.Equals( "" ) || GetCurrentMethodNameRaw().Equals( targetMethod ) || GetCurrentMethodName().Equals( targetMethod );
        }
        
        private string GetCurrentMethodName() => _methodStack.Count == 0 ? "N/A" : GetCurrentMethodEntry().MethodName;
        private string GetCurrentMethodNameRaw() => _methodStack.Count == 0 ? "N/A" : GetCurrentMethodEntry().MethodNameRaw;

#endregion


#region Method Entry Handling

        private void PushMethodEntry( bool blockStart, bool isFocused, FormattedLogType logType )
        {
            _methodStack.Push( 
                new MethodEntry( 
                    blockStart, 
                    nicifiedNames, 
                    isFocused,
                    logType, 
                    includeStackTrace, 
                    fullPathName, 
                    targetClass, 
                    targetMethod 
                    ) 
                );

            // PrintCurrentMethodEntryList();
        }

        private MethodEntry GetCurrentMethodEntry()
        {
            if( _methodStack.Count == 0 )
                throw new ArgumentOutOfRangeException( "Trying to peek an empty stack!" );
            return _methodStack.Peek();
        }

        private FormattedLogType GetCurrentMethodEntryLogType() => _methodStack.Count == 0 ? FormattedLogType.Standard : GetCurrentMethodEntry().GetLogType();
        
        private string GetCurrentCallingClassName() => _methodStack.Count == 0 ? _lastCallingClass : GetCurrentMethodEntry().CallingClassName;

        private MethodEntry PopMethodEntry()
        {
            if( _methodStack.Count <= 0 )
                throw new ArgumentOutOfRangeException( "Trying to pop an empty stack!" );
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
            if( !showLogs ) return;
            for ( int i = 0; i < amount; i++ )
            {
                IncrementMethodTabLevel();
            }
        }

        public void DecrementMethodIndent( int amount = 1 )
        {
            if( !showLogs ) return;
            for ( int i = 0; i < amount; i++ )
            {
                DecrementMethodTabLevel();
            }
        }

        private void DecrementMethodTabLevel() => GetCurrentMethodEntry().DecrementTabLevel();
        private void IncrementMethodTabLevel() => GetCurrentMethodEntry().IncrementTabLevel();

        private void ResetMethodTabLevelForLogEnd() => GetCurrentMethodEntry().ResetTabLevel();

        private void IncrementBlockTabLevel() => ++_blockTabLevel;
        private void DecrementBlockTabLevel() => _blockTabLevel = Mathf.Max( _blockTabLevel - 1, 0 );

        private string GetIndentString()
        {
            string indentString = "";

            // Iterate through methodEntry stack.
            //     Add a block divider for this method, then add a method divider for each tab level in this method.

            string formattedIndentMarker = ApplyTextColor( indentMarker );
            string formattedMethodDividers = ApplyTextColor( methodDividers );
            for ( int i = _methodStack.Count - 1; i >= 0; i-- )
            {
                MethodEntry methodEntry = _methodStack.ElementAt( i );
                indentString += $"{LoggerTabText}{formattedIndentMarker} ";
                for ( int j = 0; j < methodEntry.GetTabLevel(); j++ )
                {
                    indentString += $"{LoggerTabText}{formattedMethodDividers} ";
                }
            }


            return indentString;
        }

#endregion


#region Text Formatting

        // public string ApplyNameFormatting( string variableName ) => nicifiedNames ? NicifyVariableName( variableName ) : variableName;
        
        private string GetHexColorForMethodEntry( bool isFocused, bool blockStart ) => 
            isFocused 
                ? focusAccent.GetHexColor() 
                : ( blockStart 
                    ? _blockMethodHexColor 
                    : _methodHexColor );
        
        // private string GetHexColorForMethodEntry( bool isFocused, bool blockStart ) => blockStart ? _blockMethodHexColor : _methodHexColor;
        
        private Color GetMethodColor( MethodEntry methodEntry ) => ( methodEntry.IsFocused() && methodEntry.GetTabLevel() == 0 ) ? focusColor : methodColor;
        // private Color GetMethodColor( MethodEntry methodEntry ) => methodColor;

        private string ApplyPrefix( string message ) => $"<color={logPrefix.GetHexColor()}>{logPrefix.GetSymbol()}</color> : {message}";

        private string ApplyTextColor( string message, string hexColor ) => $"<color={hexColor}>{message}</color>";

        private string ApplyTextColor( FormattedLoggerSymbol loggerSymbol ) => $"<color={loggerSymbol.GetHexColor()}>{loggerSymbol.GetSymbol()}</color>";

        private string ApplyBlockSeparator( string message ) => $"{ApplyTextColor( blockDivider )} {message} {ApplyTextColor( blockDivider )}";

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
        
        
        /// <summary>
        ///     Sets all FormattedLogger settings to default values.
        /// </summary>
        public void SetValuesToDefault()
        {
            showLogs = true;
            useClassPrefix = true;
            boldMethods = false;
            boldBlockMethods = false;
            nicifiedNames = true;

            logPrefix = new FormattedLoggerSymbol(
                true,
                "Name_Here",
                new Color( 0.21226415f, 0.8490566f, 0.75683147f, 1f )
            );
            blockDivider = new FormattedLoggerSymbol(
                true,
                "―――――",
                new Color( 0.142f, 0.142f, 0.142f, 1f )
            );
            indentMarker = new FormattedLoggerSymbol(
                true,
                "|",
                new Color( 0.142f, 0.142f, 0.142f, 1f )
            );
            methodDividers = new FormattedLoggerSymbol(
                true,
                ".",
                new Color( 0.142f, 0.142f, 0.142f, 1f )
            );
            logBlockStart = new FormattedLoggerSymbol(
                true,

                // "→",
                // "――►",
                "►", // Note that the left and right arrows may not display the same size here but they match in Unity's debug output.
                new Color( 0.29803923f, 0.8f, 0.29803923f, 1f )
            );
            logBlockEnd = new FormattedLoggerSymbol(
                true,

                // "←", 
                // "◀――", 
                "◀",
                new Color( 0.29803923f, 0.6f, 0.9019608f, 1f )
            );
            logEventPrefix = new FormattedLoggerSymbol(
                true,
                "***",
                new Color( 0.9019608f, 0.5019608f, 0.2f, 1f )
            );
            focusAccent = new FormattedLoggerSymbol
            (
                true,
                "••••",
                new Color( 1f, 1f, 0f, 1f ) 
            );

            blockMethodColor = new Color( 1f, 1f, 0f, 1f );
            methodColor = new Color( 0.9019608f, 0.5019608f, 0.2f, 1f );
            unityEventsColor = new Color( 0f, 0f, 0f, 1f );
            focusColor = new Color( 0f, 1f, 1f, 1f );
        }

#endregion
    }
}