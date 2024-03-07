using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;
using UnityEngine.Events;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    [Serializable]
    public class SubscriptionsHandler
    {
        [SerializeField] private FormattedLogger logger;

        public void Initialize( FormattedLogger newLogger ) => logger = newLogger;

        /// <summary>
        ///     This class handles when to subscribedAction and unsubscribe from objects that can be swapped out like a settings SO. It should be called during any Unity<br/>
        ///     event in which the swappable object reference could be changed (either to null or to another subscribable object).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SubscribableSO UpdateSubscriptions( SubscribableSO previous, SubscribableSO current, UnityAction subscribedAction )
        {
            logger.LogStart( true );

            // Let: previous = P, and current = C

            if( previous == null &&
                current == null )
            {
                // Case 1: P is null and C is null.
                //     This means settings have not been set so we do nothing.
                logger.LogEnd( "Case 1: No valid objects, aborting." );
                return previous;
            }

            if( previous == null &&
                current != null )
            {
                // Case 2: P is null and C is null.
                //     This means this is the first run so no subscriptions have taken place so we need to:
                //         - set P equal to C
                //         - subscribedAction to C

                previous = current;
                SubscribeToEvents( current, subscribedAction );

                logger.LogEnd( $"Case 2: New settings set, subscribing to {GetColoredStringYellow( current.name )}." );
                return previous;
            }

            if( previous != null &&
                current == null )
            {
                // Case 3: P is not null but C is null.
                //     This means that C has been set to null in the editor so we need to:
                //         - unsubscribe from P
                //         - set P to null
                UnsubscribeToEvents( previous, subscribedAction );

                logger.LogEnd( $"Case 2: Settings cleared. Unsubscribing from {GetColoredStringYellow( previous.name )}" );
                // previous = null;
                return null;
            }


            // if( previous == null ||
            //     current == null ) throw new ArgumentOutOfRangeException();

            // Case 4: neither P or C are null
            //     This means we're in one of two states:
            //         Case 4a: P is equal to C meaning no change has occured so we do nothing.
            //         Case 4b: P is not equal to C so we need to:
            //             - unsubscribe from P
            //             - set P equal to C
            //             - subscribedAction to C
            logger.Log( $"Case 4: Settings changed. Unsubscribing from {GetColoredStringYellow( previous.name )} " +
                            $"and subscribing to {GetColoredStringOrange( current.name )}." );

            UnsubscribeToEvents( previous, subscribedAction );

            previous = current;
            SubscribeToEvents( current, subscribedAction );
            
            logger.LogEnd();
            return previous;
        }

        private void SubscribeToEvents( SubscribableSO so, UnityAction subscribe )
        {
            logger.LogStart( false,  $"Subscribing to {GetColoredStringBurlyWood( so.name )}" );

            so.onSettingsUpdated += subscribe;

            logger.LogEnd();
        }

        private void UnsubscribeToEvents( SubscribableSO so, UnityAction subscribe )
        {
            logger.LogStart( false, $"Unsubscribing from {GetColoredStringYellow( so.name )}" );

            if( so.onSettingsUpdated == null ) return;
            so.onSettingsUpdated -= subscribe;

            logger.LogEnd();
        }
    }
}