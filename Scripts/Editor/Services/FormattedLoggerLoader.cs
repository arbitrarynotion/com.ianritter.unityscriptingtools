using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger;

using UnityEngine;

using static Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.AssetLoader;
using static Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.System.SystemConstants;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services
{
    public static class FormattedLoggerLoader
    {
        // private const string DefaultLoggerName = "DefaultLogger";
        // private const string DefaultLoggerFolderToSearch = "Assets/ScriptableObjects/Loggers";

        /* Example of how to use:
        ``` 
        private FormattedLogger _logger;
        private const string loggerName = "Logger asset name goes here"
        private const string loggerPath = "Logger search path goes here"
        
        private void OnEnable()
        {
            if ( _logger == null )
                _logger = FormattedLoggerLoader.GetLogger( loggerName, loggerPath );
        }
        ```
        This will load the the requested logger, if it can find it. Otherwise, it will load the default system logger
        so a usable logger will always be returned.
        
        If you're logger is in a package directory, note its actual path won't match Unity's folder structure.
        Example:
        Path based on folders in Unity: 
            "Packages/Unity Scripting Tools/ScriptableObjects/FormattedLoggers/"
        Actual Windows file path: 
            "Packages/com.ianritter.unityscriptingtools/ScriptableObjects/FormattedLoggers/"
        */

        /// <summary>
        ///     Important: Only use with editor scripts! This method accesses the AssetDatabase which is in UnityEditor.<br/>
        ///     Load the formatted logger from the assets folder. If the specified logger can't be found<br/>
        ///     the default logger is returned so that a usable logger is always returned.
        /// </summary>
        public static FormattedLogger GetLogger( string loggerName, string folderToSearch )
        {
            var logger = GetAssetByName<FormattedLogger>( loggerName, folderToSearch );
            if( logger != null ) return logger;
            Debug.LogWarning( $"Failed to load {loggerName}! Loading Default logger." );

            logger = GetAssetByName<FormattedLogger>( DefaultLoggerPath, DefaultLoggerPath );
            if( logger == null )
                Debug.LogWarning( "Failed to load the default logger!!!" );
            return logger;
        }
    }
}