using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.MetaData;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger
{
    internal class MethodEntry
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
                
            string methodName = MetaDataGathering.GetMethodName( 2 );
            MethodName = nicifyName ? TextFormat.NicifyVariableName( methodName ) : methodName;
            CallingClassName = MetaDataGathering.GetCallingClassName( StackTraceIndex );

            if ( !MethodName.Equals( "ctor" ) ) return;
                
            // Replace constructor name for readability.
            MethodName = $"Constructor ({TextFormat.NicifyVariableName( CallingClassName )})";
        }

        public bool IsBlockStart() => _blockStart;

        public void IncrementTabLevel() => ++_tabLevel;
        public void DecrementTabLevel() => _tabLevel = Mathf.Max( 0, ( _tabLevel - 1 ) );

        public int GetTabLevel() => _tabLevel;
    }
}