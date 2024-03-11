using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.MetaData;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger
{
    internal class MethodEntry
    {
        // private int _logCount;

        // 0 = GetMethodName, 1 = MethodEntry.ctor, 2 = PushMethodEntry, 3 = LogStart, 4 = LogStart, 5 = CallingClassMethod
        private const int StackTraceIndex = 5;

        private readonly bool _blockStart;
        private readonly bool _isFocused;
        private readonly FormattedLogType _logType;
        public readonly string CallingClassName;
        public readonly string CallingClassNameRaw;

        public readonly string MethodName;

        // The rawName is not currently necessary but may be used to include the current method rawName.
        public readonly string MethodNameRaw;
        private int _tabLevel;

        // Had to do nicify names first so the constructor modification didn't get processed by that algorithm.
        public MethodEntry
        (
            bool blockStart,
            bool nicifyName,
            bool isFocused,
            FormattedLogType logType,
            bool printStackTrace,
            bool fullPathName,
            string targetClassName,
            string targetMethodName
        )
        {
            _blockStart = blockStart;
            _isFocused = isFocused;
            _logType = logType;


            string methodName = MetaDataGathering.GetMethodName( StackTraceIndex, printStackTrace, fullPathName, targetMethodName );
            MethodNameRaw = methodName;
            MethodName = nicifyName ? NicifyVariableName( methodName ) : methodName;
            // if( isFocused ) MethodName = $"<b>!!!! {MethodName} !!!!</b>";
            CallingClassName = MetaDataGathering.GetCallingClassName( StackTraceIndex, printStackTrace, fullPathName, targetClassName );

            // if ( isFocused ) Debug.LogWarning( $"{GetColoredStringMagenta( MethodName )}'s focus was set to true in Method Entry!" );

            if( !MethodName.Equals( "ctor" ) ) return;

            // Replace constructor name for readability.
            MethodName = $"Constructor ({NicifyVariableName( CallingClassName )})";
        }

        public bool IsBlockStart() => _blockStart;
        public bool IsFocused() => _isFocused;

        public void IncrementTabLevel() => ++_tabLevel;
        public void DecrementTabLevel() => _tabLevel = Mathf.Max( 0, _tabLevel - 1 );
        public int GetTabLevel() => _tabLevel;
        public void ResetTabLevel() => _tabLevel = 0;

        public FormattedLogType GetLogType() => _logType;
    }
}