using System.Collections.Generic;
using System.Linq;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    public static class AssetLoader
    {
        public static T GetAssetByName<T>( string assetName )
        {
            return GetAssetByName<T>( assetName, null );
        }

        public static bool LoadAssetToPropertyByNameWithDefault( SerializedProperty assetProperty, string targetAssetName, string defaultAssetName, string defaultAssetSearchPath )
        {
            CustomLogger asset = LoadAssetToPropertyByNameWithDefault( targetAssetName, defaultAssetName, defaultAssetSearchPath );
            if ( asset == null ) return false;
            
            assetProperty.objectReferenceValue = asset;
            assetProperty.serializedObject.ApplyModifiedProperties();
            return true;
        }

        public static CustomLogger LoadAssetToPropertyByNameWithDefault( string targetAssetName, string defaultAssetName, string defaultAssetSearchPath )
        {
            // Debug.Log( $"Property's type is: {assetProperty.propertyType.ToString()}" );
            // Debug.Log( $"AMRE|Awake: loading asset: {GetColoredStringYellow(targetAssetName)}" );
            var asset = GetAssetByName<CustomLogger>( targetAssetName );
            if ( asset != null ) return asset;
            
            Debug.LogWarning( $"Loading of asset '{GetColoredStringYellow(targetAssetName)}' failed! Loading default asset {GetColoredStringDeepPink(defaultAssetName)}." );
            asset = GetAssetByName<CustomLogger>( defaultAssetName, defaultAssetSearchPath );
            if ( asset == null )
            {
                Debug.LogError( $"Failed to load default asset: {defaultAssetName}!" );
            }

            return asset;
        }
        
        public static bool AssetWithNameExistsAtPathLocation( string assetName, params string[] searchInFolders )
        {
            string[] guids = AssetDatabase.FindAssets( assetName, searchInFolders );
            
            return guids.Length > 0;
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