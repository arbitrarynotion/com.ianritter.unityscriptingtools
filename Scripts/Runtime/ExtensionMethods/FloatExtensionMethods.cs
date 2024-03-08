using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.ExtensionMethods
{
    public static class FloatExtensionMethods
    {
        /// <summary>
        ///     Converts a [0,n] value to [-n,n], where n is the Maximum value.
        /// </summary>
        public static float Convert0ToMaxToNegMaxToPosMax( this float value, float max = 1f ) => value * 2 - max;
        
        public static float Normalize( this float value, float min, float max ) => Mathf.InverseLerp( min, max, value );

        public static float[,] Normalize( this float[,] twoDimensionalArray, float min, float max )
        {
            for ( int y = 0; y < twoDimensionalArray.GetLength( 1 ); y++ )
            {
                for ( int x = 0; x < twoDimensionalArray.GetLength( 0 ); x++ )
                {
                    // Use InverseLerp to convert the noise values to a [0, 1] value based on the min and max noise values.
                    twoDimensionalArray[x,y] = twoDimensionalArray[x, y].Normalize( min, max ); 
                }
            }

            return twoDimensionalArray;
        }
    }
}