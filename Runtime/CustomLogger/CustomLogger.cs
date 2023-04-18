// using UnityEditor;

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using static Packages.com.ianritter.unityscriptingtools.Runtime.ToolingConstants;
using static Packages.com.ianritter.unityscriptingtools.Runtime.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.CustomLogger
{
    [Serializable]
    public class CustomLoggerSymbol
    {
        [SerializeField]
        private bool toggle;
        [SerializeField]
        private string symbol;
        [SerializeField]
        private Color color;
        
        private string _hexColor;
        

        public CustomLoggerSymbol( bool toggle, string symbol, Color color )
        {
            this.toggle = toggle;
            this.symbol = symbol;
            this.color = color;
            UpdateHexColor();
        }
        
        public string GetHexColor() => _hexColor;
        public void UpdateHexColor() => _hexColor = $"#{ColorUtility.ToHtmlStringRGB( color )}";

        public string GetSymbol() => toggle ? symbol : string.Empty;
        
    }


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
            // The name is not currently necessary but may be used to include the current method name.
            private readonly string _methodName;
            private int _tabLevel;

            public MethodEntry( string methodName ) => _methodName = methodName;

            public void IncrementTabLevel() => ++_tabLevel;
            public void DecrementTabLevel() => _tabLevel = Mathf.Max( 0, ( _tabLevel - 1 ) );

            public int GetTabLevel() => _tabLevel;
        }
        
        [SerializeField] private bool showLogs;
        
        [SerializeField] private bool nicifiedNames = true;

        [Header("Log Symbols")]
        [SerializeField] private CustomLoggerSymbol logPrefix;
        [SerializeField] private CustomLoggerSymbol blockDivider;
        [SerializeField] private CustomLoggerSymbol blockDividers;
        [SerializeField] private CustomLoggerSymbol methodDividers;
        [SerializeField] private CustomLoggerSymbol logBlockStart;
        [SerializeField] private CustomLoggerSymbol logBlockEnd;
        [SerializeField] private CustomLoggerSymbol logEventPrefix;

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
            _blockTabLevel = 0;
        }

        private void OnValidate() => UpdatePrefixColor();
        
        private void UpdatePrefixColor()
        {
            logPrefix.UpdateHexColor();
            logBlockStart.UpdateHexColor();
            blockDividers.UpdateHexColor();
            methodDividers.UpdateHexColor();
            logBlockEnd.UpdateHexColor();
            logEventPrefix.UpdateHexColor();
        }


#region Log Methods

        public void LogStart( MethodBase methodBase, bool blockStart = false )
        {
            if ( !PreProcessLog() ) return;

            PushMethodEntry( methodBase.Name );
            
            // string message = $"{ApplyTextColor( $"{logBlockStart.GetSymbol()} ", logBlockStart.GetHexColor() )} {NicifyVariableName( methodBase.Name )}";
            string message = BuildLogMessage( logBlockStart, methodBase.Name );
            if ( blockStart ) message = ApplyBlockSeparator( message );
            PrintStandardLog( message );
            
            IncrementTabLevel();
        }

        public void LogEnd( MethodBase methodBase, bool blockEnd = false )
        {
            if ( !PreProcessLog() ) return;
            
            // string message = $"{ApplyTextColor($"{logBlockEnd.GetSymbol()} ", logBlockEnd.GetHexColor() )} {NicifyVariableName( methodBase.Name )}";
            string message = BuildLogMessage( logBlockEnd, methodBase.Name );
            if ( blockEnd ) message = ApplyBlockSeparator( message );
            
            DecrementTabLevel();
            
            PopMethodEntry();
            
            PrintStandardLog( message );
        }


        public void LogEvent( MethodBase methodBase, bool increaseTabLevel = false )
        {
            if ( !PreProcessLog() ) return;
            if ( increaseTabLevel ) IncrementMethodTabLevel();
            // PrintStandardLog( $"{ApplyTextColor( $"{logEventPrefix.GetSymbol()} ", logEventPrefix.GetHexColor() )} {NicifyVariableName( methodBase.Name )}" );
            PrintStandardLog( BuildLogMessage( logEventPrefix, methodBase.Name ) );
        }

        public void Log( string message, bool increaseTabLevel = false, bool oneTimeIncrement = false )
        {
            if ( !PreProcessLog() ) return;
            if ( increaseTabLevel ) IncrementMethodTabLevel();
            PrintStandardLog( message );
            if ( oneTimeIncrement ) DecrementMethodTabLevel();
        }

        public void LogWarning( string message, bool increaseTabLevel = false )
        {
            if ( !PreProcessLog() ) return;
            if ( increaseTabLevel ) IncrementMethodTabLevel();
            PrintWarningLog( message );
        }

        public void LogError( string message, bool increaseTabLevel = false )
        {
            if ( !PreProcessLog() ) return;
            if ( increaseTabLevel ) IncrementMethodTabLevel();
            PrintErrorLog( message );
        }

        private string BuildLogMessage( CustomLoggerSymbol loggerSymbol, string methodName ) =>
            $"{ApplyTextColor( $"{loggerSymbol.GetSymbol()} ", loggerSymbol.GetHexColor() )} {NicifyVariableName( methodName )}";

        private bool PreProcessLog()
        {
            if ( !showLogs ) return false;
            if ( _methodStack == null ) _methodStack = new Stack<MethodEntry>();
            return true;
        }
        
        


#endregion

        
        


#region Method Entry Handling

        private void PushMethodEntry( string methodName ) => _methodStack.Push( new MethodEntry( methodName ) );

        private MethodEntry GetCurrentMethodEntry()
        {
            if ( _methodStack.Count == 0 )
                throw new ArgumentOutOfRangeException($"Trying to peek an empty stack!");
            return _methodStack.Peek();
        }

        private void PopMethodEntry()
        {
            if ( _methodStack.Count <= 0 )
                throw new ArgumentOutOfRangeException($"Trying to pop an empty stack!");
            _methodStack.Pop();
        }

#endregion
        



#region Tabbing
        
        public void DecrementMethodTabLevel() => GetCurrentMethodEntry().DecrementTabLevel();
        public void IncrementMethodTabLevel() => GetCurrentMethodEntry().IncrementTabLevel();
        
        private void IncrementTabLevel() => ++_blockTabLevel;
        
        private void DecrementTabLevel() => _blockTabLevel = Mathf.Max( ( _blockTabLevel - 1 ), 0 );

        private string GetBlockIndent() => GetIndent( blockDividers, _blockTabLevel );
        
        private string GetMethodIndent()
        {
            if ( _methodStack == null ) Debug.LogWarning( "_methodStack is null!" );
            return GetIndent( methodDividers, ( _methodStack.Count > 0 ? GetCurrentMethodEntry().GetTabLevel() : 0 ) );
        }

        private string GetIndent( CustomLoggerSymbol loggerSymbol, int tabLevel )
        {
            string divider = ApplyTextColor( $"{loggerSymbol.GetSymbol()} ", loggerSymbol.GetHexColor() );
            // string totalIndent = divider;
            string totalIndent = "";
            for (int i = 0; i < tabLevel; i++)
            {
                // string symbol = ( i < ( tabLevel - 1 ) ) ? divider : "";
                totalIndent += $"{LoggerTabText}{divider}";
            }
            return totalIndent;
        }
        
        private string ApplyIndent( string message ) => $"{GetBlockIndent()}{GetMethodIndent()}{message}";

#endregion


#region Text Formatting
        
        public string ApplyNameFormatting( string variableName ) => nicifiedNames ? NicifyVariableName( variableName ) : variableName;

        private string ApplyPrefix( string message ) => $"<color={logPrefix.GetHexColor()}>{logPrefix.GetSymbol()}</color> {message}";
        
        private string ApplyTextColor( string message, string hexColor ) => $"<color={hexColor}>{message}</color>";
        
        private string ApplyTextColor( CustomLoggerSymbol loggerSymbol ) => $"<color={loggerSymbol.GetHexColor()}>{loggerSymbol.GetSymbol()}</color>";
        
        private string ApplyBlockSeparator( string message ) => $"{ApplyTextColor( blockDivider )} {message} {ApplyTextColor( blockDivider )}";
            // $"{GetColoredStringBlack( $"----------" )} {message} {GetColoredStringBlack( "----------" )}";

#endregion


#region Log Output

        private void PrintStandardLog( string message ) =>
            Debug.Log( ApplyPrefix( $"{ApplyIndent( message )}" ), _sender );

        private void PrintWarningLog( string message ) =>
            Debug.LogWarning( ApplyPrefix( $"{ApplyIndent( message )}" ), _sender );

        private void PrintErrorLog( string message ) =>
            Debug.LogError( ApplyPrefix( $"{ApplyIndent( message )}" ), _sender );

#endregion
    }
}
