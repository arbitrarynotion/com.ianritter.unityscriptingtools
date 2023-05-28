using System.IO;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;

namespace Packages.com.ianritter.unityscriptingtools.Editor.ExtensionMethods
{
    public static class AssetDatabaseWrapper
    {
        public static bool CreateAssetSafely( Object asset, string path, string fileName )
        {
            if ( AssetWithNameExistsAtPathLocation( fileName, path ) ) return false;
            
            path = Path.Combine( path, fileName );
            if ( !path.Contains( ".asset" ) ) path += ".asset";
            Debug.Log( $"Result of path combination: {path}" );
            AssetDatabase.CreateAsset( asset, path );
            return true;
        }
    }
}