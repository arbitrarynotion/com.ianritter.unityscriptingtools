using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Editor.Services.AssetLoader;

namespace Packages.com.ianritter.unityscriptingtools.Editor.Services
{
    public static class CustomLoggerLoader
    {
        private const string DefaultLoggerName = "DefaultLogger";
        private const string DefaultLoggerFolderToSearch = "Assets/ScriptableObjects/Loggers";

        
        /// <summary>
        /// Load the custom logger from the assets folder. If the specified logger can't be found
        /// the default logger is returned so that a usable logger is always returned.
        /// </summary>
        public static CustomLogger GetLogger( string loggerName, string folderToSearch )
        {
            var logger = GetAssetByName<CustomLogger>( loggerName, folderToSearch );
            if ( logger != null ) return logger;
            Debug.LogWarning( $"Failed to load {loggerName}! Loading Default custom logger." );
            
            logger = GetAssetByName<CustomLogger>( DefaultLoggerName, DefaultLoggerFolderToSearch );
            if ( logger == null )
                Debug.LogWarning( $"Failed to load the default logger!!!" );
            return logger;
        }
    }
}
