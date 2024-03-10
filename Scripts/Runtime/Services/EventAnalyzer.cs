using System;
using System.Reflection;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;
using Object = System.Object;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    public class EventAnalyzer : MonoBehaviour
    {
        // public static bool IsSubscribed( Delegate targetEvent, Object target )
        // {
        //     if ( targetEvent == null )
        //         return false;
        //     
        //     Delegate[] eventList = targetEvent.GetInvocationList();
        //     if ( eventList.Length == 0 )
        //         return false;
        //
        //     foreach ( Delegate currentDelegate in targetEvent.GetInvocationList() )
        //     {
        //         if( currentDelegate.Target == target )
        //             return true;
        //         
        //         currentDelegate.Method.MemberType
        //     }
        //
        //     return false;
        //
        //     // return targetType == targetEvent.Target.GetType();
        // }
        
        // public static bool IsSubscribed( Delegate targetEvent, Type targetType )
        // {
        //     if ( targetEvent == null )
        //         return false;
        //     
        //     Delegate[] eventList = targetEvent.GetInvocationList();
        //     if ( eventList.Length == 0 )
        //         return false;
        //
        //     return targetType == targetEvent.Target.GetType();
        // }
        
        public static void PrintSubscribersForEvent( string ownersName, FormattedLogger logger, Delegate myEvent, string eventTypeName, string eventName )
        {
            logger.LogStart( true );
            logger.LogIndentStart( $"Subscribers of {GetColoredStringOrange( NicifyVariableName( ownersName ) )}:" );
            
            logger.Log( $"• {GetColoredStringGreenYellow( NicifyVariableName( eventName ) )} ({GetColoredStringDimGray( eventTypeName )}):" );
            
            if ( myEvent == null || myEvent.GetInvocationList().Length == 0 )
            {
                logger.LogOneTimeIndent( "None" );
                return;
            }
            
            foreach ( Delegate currentDelegate in myEvent.GetInvocationList() )
            {
                var target = (UnityEngine.Object) currentDelegate.Target;

                logger.LogOneTimeIndent( $"• {GetColoredStringAqua( NicifyVariableName( target.GetType().Name ) )}:" +
                                         $"{GetColoredStringYellow( currentDelegate.Method.Name )} " +
                                         $"(target {GetColoredStringGoldenrod( target.name )}), " );
                // logger.LogOneTimeIndent( $"    Method name:{GetColoredStringYellow( currentDelegate.Method.Name )}" );
                // logger.LogOneTimeIndent( $"    Method member type:{GetColoredStringYellow( currentDelegate.Method.MemberType.ToString() )}" );
                // logger.LogOneTimeIndent( $"    Target name:{GetColoredStringYellow( currentDelegate.Target.ToString() )}" );

            }
            
            logger.LogEnd();
        }
        
        // public void PrintMySubscribers( FormattedLogger logger, Delegate myEvent )
        // {
        //     logger.LogStart( true );
        //     logger.LogIndentStart( $"Subscribers of {GetColoredStringOrange( NicifyVariableName( name ) )}:" );
        //
        //     PrintSubscribersForEvent( logger, myEvent, nameof(OnDataUpdated), "Readers" );
        //     logger.LogEnd();
        // }
    }
}
