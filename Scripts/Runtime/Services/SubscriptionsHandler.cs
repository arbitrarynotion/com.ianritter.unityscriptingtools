using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;
using UnityEngine;
using UnityEngine.Events;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI.TextFormatting;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services
{
    // public interface ISubscribable
    // {
    //     void Subscribe( Action subscriberAction );
    //     bool HasSubscribers();
    //     void Unsubscribe( Action subscriberAction );
    // }
    //
    // public interface ISubscriber
    // {
    //     void OnSubscribableUpdated();
    // }

    public class SubscriptionsHandler
    {
        [SerializeField] private FormattedLogger _logger;

        public void Initialize( FormattedLogger logger ) => _logger = logger;

        /// <summary>
        ///     This class handles when to subscribedAction and unsubscribe from objects that can be swapped out like a settings SO. It should be called during any Unity<br/>
        ///     event in which the swappable object reference could be changed (either to null or to another subscribable object).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UpdateSubscriptions( ref SubscribableSO previous, ref SubscribableSO current, UnityAction subscribedAction )
        {
            _logger.LogStart();

            // Let: previous = P, and current = C

            if( previous == null &&
                current == null )
            {
                // Case 1: P is null and C is null.
                //     This means settings have not been set so we do nothing.
                _logger.LogEnd( "Case 1: No valid objects, aborting." );
                return;
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

                // subscribedAction.Invoke( current );
                _logger.LogEnd( $"Case 2: New settings set, subscribing to {GetColoredStringYellow( current.name )}." );
                return;
            }

            if( previous != null &&
                current == null )
            {
                // Case 3: P is not null but C is null.
                //     This means that C has been set to null in the editor so we need to:
                //         - unsubscribe from P
                //         - set P to null
                UnsubscribeToEvents( previous, subscribedAction );

                // unsubscribe.Invoke( previous );
                _logger.LogEnd( $"Settings cleared. Unsubscribing from {GetColoredStringYellow( previous.name )}" );
                previous = null;
                return;
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
            UnsubscribeToEvents( previous, subscribedAction );

            // unsubscribe.Invoke( previous );
            previous = current;
            SubscribeToEvents( current, subscribedAction );

            // subscribedAction.Invoke( current );
            _logger.LogEnd( $"Settings changed. Unsubscribing from {GetColoredStringYellow( previous.name )} " +
                            $"and subscribing to {GetColoredStringYellow( current.name )}." );
        }

        private void SubscribeToEvents( SubscribableSO so, UnityAction subscribe )
        {
            _logger.LogStart();

            so.onSettingsUpdated += subscribe;

            _logger.LogEnd();
        }

        private void UnsubscribeToEvents( SubscribableSO so, UnityAction subscribe )
        {
            _logger.LogStart( false, $"Unsubscribing from {GetColoredStringYellow( so.name )}" );

            if( so.onSettingsUpdated == null ) return;
            so.onSettingsUpdated -= subscribe;

            _logger.LogEnd();
        }
    }
}