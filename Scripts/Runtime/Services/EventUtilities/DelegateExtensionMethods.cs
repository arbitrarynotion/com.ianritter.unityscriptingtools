using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    public static class DelegateExtensionMethods
    {
        public static void PrintMySubscribers( this Delegate myEvent, string ownersName, FormattedLogger logger, string eventTypeName, string eventName )
        {
            EventAnalyzer.PrintSubscribersForEvent( ownersName, logger, myEvent, eventTypeName, eventName );
        }
    }
}