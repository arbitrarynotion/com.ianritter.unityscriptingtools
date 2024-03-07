using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.MetaData;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger
{
    internal class MethodEntry
    {
        // private int _logCount;

        // 0 = GetMethodName, 1 = MethodEntry.ctor, 2 = PushMethodEntry, 3 = LogStart, 4 = CallingClassMethod
        private const int StackTraceIndex = 4;
        private readonly bool _blockStart;
        private readonly FormattedLogType _logType;
        public readonly string CallingClassName;
        public readonly string CallingClassNameRaw;

        public readonly string MethodName;

        // The rawName is not currently necessary but may be used to include the current method rawName.
        public readonly string MethodNameRaw;
        private int _tabLevel;

        // Had to do nicify names first so the constructor modification didn't get processed by that algorithm.
        public MethodEntry
        ( bool blockStart, bool nicifyName, FormattedLogType logType = FormattedLogType.Standard,
            bool printStackTrace = false, bool fullPathName = false, string targetClassName = "", string targetMethodName = ""
        )
        {
            _blockStart = blockStart;
            _logType = logType;

            string methodName = MetaDataGathering.GetMethodName( StackTraceIndex, printStackTrace, fullPathName, targetMethodName );
            MethodNameRaw = methodName;
            MethodName = nicifyName ? TextFormatting.NicifyVariableName( methodName ) : methodName;
            CallingClassName = MetaDataGathering.GetCallingClassName( StackTraceIndex, printStackTrace, fullPathName, targetClassName );

            if( !MethodName.Equals( "ctor" ) ) return;

            // Replace constructor name for readability.
            MethodName = $"Constructor ({TextFormatting.NicifyVariableName( CallingClassName )})";
        }

        public bool IsBlockStart() => _blockStart;

        public void IncrementTabLevel()
        {
            // Debug.Log( $"{MethodName}: Incrementing tab level." );
            ++_tabLevel;
        }

        // public void IncrementLogCount() => _logCount++;
        //
        // public int GetLogCount() => _logCount;

        public void DecrementTabLevel() => _tabLevel = Mathf.Max( 0, _tabLevel - 1 );

        public int GetTabLevel() => _tabLevel;

        public void ResetTabLevel() => _tabLevel = 0;

        public FormattedLogType GetLogType() => _logType;
    }
}