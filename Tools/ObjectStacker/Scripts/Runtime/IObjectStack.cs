using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System;

namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime
{
    public interface IObjectStack
    {
        /// <summary>
        ///     Returns the next position in the stack, automatically incrementing the current position counter.<br/>
        ///     Use this to progress through the stack positions sequentially from 0 to n.
        /// </summary>
        PseudoTransform GetNextPosition();

        /// <summary>
        ///     Returns the index the current stack position is set to. This is the index that will be used on the next GetNextPosition call.
        /// </summary>
        int GetCurrentStackPosition();

        /// <summary>
        ///     Resets the internal stack counter back to 0. Use this to loop back to the beginning of the stack.
        /// </summary>
        void ResetStackCounter();
    }
}