namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime
{
    public static class FloatExtensionMethods
    {
        /// <summary>
        ///     Converts a [0,n] value to [-n,n], where n is the Maximum value.
        /// </summary>
        public static float Convert0ToMaxToNegMaxToPosMax( this float value, float max = 1f ) => value * 2 - max;
    }
}