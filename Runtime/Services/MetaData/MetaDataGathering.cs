using System;
using System.Diagnostics;
using System.Linq;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using Debug = UnityEngine.Debug;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.MetaData
{
    public static class MetaDataGathering
    {
        public static  string GetCallingClassName( int stackTraceIndex, bool printStackTrace = false, bool fullPathName = false, string targetClassName = "" )
        {
            var stackTrace = new StackTrace( true );
            StackFrame[] stackFrames = stackTrace.GetFrames();

            if ( stackFrames == null )
                throw new NullReferenceException( "Failed to get stack trace frames when seeking class name." );
            
            // Note that the calling class is on step back in the stacktrace CallingClass -> LogStart(), so index is 1.
            // However, the trace has the entire path rawName so it needs to be Split by '\', then the ".cs" has to be
            // removed from the end of the class rawName using another Split.
            // return stackTrace.GetFrames()[2].GetFileName().Split( '\\' ).Last().Split( '.' )[0];

            string callingClassName = ExtractFilenameStringFromPath( stackFrames[stackTraceIndex].GetFileName() );
            
            if ( printStackTrace ) OutputStackTrace( callingClassName, targetClassName, fullPathName );
            
            return callingClassName;
        }
        
        public static string GetMethodName( int stackTraceIndex, bool printStackTrace = false, bool fullPathName = false, string targetMethodName = "" )
        {
            var stackTrace = new StackTrace( true );
            StackFrame[] stackFrames = stackTrace.GetFrames();

            if ( stackFrames == null )
                throw new NullReferenceException( "Failed to get stack trace frames when seeking method name." );

            string fullMethodName = stackFrames[stackTraceIndex].GetMethod().ToString();
            string methodNameWithoutReturn = fullMethodName.Split( ' ' ).Last();
            string outputMethodName = methodNameWithoutReturn.Split( '(' )[0];

            if ( printStackTrace ) OutputStackTrace( outputMethodName, targetMethodName, fullPathName );

            return outputMethodName;
        }

        private static void OutputStackTrace( string name, string targetName, bool fullPathName )
        {
            if ( name.Equals( targetName ) || targetName.Equals( "" ) ) PrintStackTrace( name, fullPathName );
        }
        
        public static void PrintStackTrace( string name, bool fullPathName )
        {
            Debug.Log( $"Stack Trace ({name}):" );

            string tab = "    ";
            string space = "    ";
            
            var stackTrace = new StackTrace( true );
            foreach ( StackFrame frame in stackTrace.GetFrames() )
            {
                if ( frame?.GetFileName() == null ) continue;

                string fileName = fullPathName ? frame.GetFileName() : ExtractFilenameStringFromPath( frame.GetFileName() );
                
                Debug.Log( $"{space}{GetColoredStringOrange( fileName )} :: " + 
                           $"{GetColoredStringYellow( frame.GetMethod().ToString() )}");
                // Debug.Log( $"        Line: {frame.GetFileLineNumber().ToString()}, ");
                // Debug.Log( $"        Column: {frame.GetFileColumnNumber().ToString()}" );
                space += tab;
            }
        }
        
        public static string ExtractFilenameStringFromPath( string fullPathOfFilename )
        {
            if ( fullPathOfFilename.Equals( "..." ) ) return fullPathOfFilename;
            
            // Debug.Log( $">>>> Cleaning up filename: {fullPathOfFilename}..." );
            return fullPathOfFilename.Split( '\\' ).Last().Split( '.' )[0];
        }
    }
}
