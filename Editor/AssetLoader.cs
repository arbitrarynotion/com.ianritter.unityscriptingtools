using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Editor
{
    public static class AssetLoader
    {
        public static T GetAssetByName<T>( string assetName )
        {
            return GetAssetByName<T>( assetName, null );
        }

        public static T GetAssetByName<T>( string assetName, params string[] searchInFolders )
        {
            string[] guids = AssetDatabase.FindAssets( assetName, searchInFolders );
            
            if (guids.Length <= 0)
                return (T) (object) null;

            string path = AssetDatabase.GUIDToAssetPath( guids[0] );
            var settings = (T) (object) AssetDatabase.LoadAssetAtPath( path, typeof(T) );
            return settings;
        }
        
        public static List<T> GetAssetsByType<T>( params string[] searchPaths ) where T : Object
        {
            string type = typeof( T ).ToString().Split( '.' ).Last();
            string[] guids = AssetDatabase.FindAssets( $"t:{type}", searchPaths );
            
            List<T> assets = new List<T>();
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath( guid );
                var asset = AssetDatabase.LoadAssetAtPath<T>( path );
                if ( asset != null )
                {
                    assets.Add( asset );
                }
            }

            return assets;
        }
    }
    
    
}