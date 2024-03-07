using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Examples
{
    public class ExampleSubclass01
    {
        private readonly FormattedLogger _logger;

        public ExampleSubclass01( FormattedLogger logger )
        {
            _logger = logger;

            // Debug.Log( "ExampleSubclass01's constructor was called." );
            logger.LogStart();
            logger.Log( "ExampleSubclass01 Start was called." );
            logger.LogEnd();
        }

        public void SubClassMethodLevel_1()
        {
            _logger.LogStart( true );
            _logger.Log( "This is subclass level 1." );
            SubClassMethodLevel_2();
            _logger.LogEnd();
        }

        private void SubClassMethodLevel_2()
        {
            _logger.LogStart( false, "This is a LogStart log. This method section is set to type Warning.", FormattedLogType.Warning );
            _logger.LogIndentStart( "This is subclass level 2." );
            _logger.Log( "Subclass sub log 1" );
            _logger.Log( "Subclass sub log 2, overriding parent's type with Error.", FormattedLogType.Error );
            _logger.Log( "Subclass sub log 3" );
            _logger.LogOneTimeIndent( "Subclass sub log 4, with one-time indent and overriding parent's type with Standard.", FormattedLogType.Standard );
            _logger.Log( "Subclass sub log 5" );
            _logger.LogIndentEnd( "Subclass sub log 6, ending one level of indent starting with itself.", 1, true );
            _logger.Log( "Subclass sub log 7" );
            _logger.IncrementMethodIndent( 3 );
            _logger.Log( "Subclass sub log 8, after manual increment of 3." );
            _logger.Log( "Subclass sub log 9." );
            _logger.DecrementMethodIndent();
            _logger.Log( "Subclass sub log 10, after manual decrement 0f 1." );

            _logger.LogEnd( "This is a LogEnd log. Lacks other log options but good for quickly stating reason for method exit." );
        }
    }
}