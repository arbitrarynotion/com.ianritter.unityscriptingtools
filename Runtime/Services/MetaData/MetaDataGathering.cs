using System;
using System.Diagnostics;
using System.Linq;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using Debug = UnityEngine.Debug;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.MetaData
{
    public static class MetaDataGathering
    {
        public static  string GetCallingClassName( int stackTraceIndex )
        {
            var stackTrace = new StackTrace( true );
            StackFrame[] stackFrames = stackTrace.GetFrames();

            if ( stackFrames == null )
                throw new NullReferenceException( "Failed to get stack trace frames when seeking class name." );
            
            // Note that the calling class is on step back in the stacktrace CallingClass -> LogStart(), so index is 1.
            // However, the trace has the entire path rawName so it needs to be Split by '\', then the ".cs" has to be
            // removed from the end of the class rawName using another Split.
            // return stackTrace.GetFrames()[2].GetFileName().Split( '\\' ).Last().Split( '.' )[0];
            return ExtractFilenameStringFromPath( stackFrames[stackTraceIndex].GetFileName() );
        }
        
        public static string GetMethodName( int stackTraceIndex )
        {
            var stackTrace = new StackTrace( true );
            StackFrame[] stackFrames = stackTrace.GetFrames();

            if ( stackFrames == null )
                throw new NullReferenceException( "Failed to get stack trace frames when seeking method name." );

            string fullMethodName = stackFrames[stackTraceIndex].GetMethod().ToString();
            string methodNameWithoutReturn = fullMethodName.Split( ' ' ).Last();
            return methodNameWithoutReturn.Split( '(' )[0];
        }
        
        public static void PrintStackTrace()
        {
            var stackTrace = new StackTrace( true );
            foreach ( StackFrame frame in stackTrace.GetFrames() )
            {
                Debug.Log( $"FileName: {GetColoredStringOrange( ExtractFilenameStringFromPath( frame.GetFileName() ) )}");
                Debug.Log( $"    Method: {GetColoredStringYellow( frame.GetMethod().ToString() )}, ");
                Debug.Log( $"    Line: {frame.GetFileLineNumber().ToString()}, ");
                Debug.Log( $"    Column: {frame.GetFileColumnNumber().ToString()}" );
            }
        }
        
        public static string ExtractFilenameStringFromPath( string fullPathOfFilename ) => 
            fullPathOfFilename.Split( '\\' ).Last().Split( '.' )[0];
    }
}
