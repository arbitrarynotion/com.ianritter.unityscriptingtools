namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.ExtensionMethods
{
    public static class TwoDimensionalFloatArrayExtensionMethods
    {
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