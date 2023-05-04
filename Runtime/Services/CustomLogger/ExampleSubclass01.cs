using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger
{
    public class ExampleSubclass01
    {
        private readonly CustomLogger _logger;

        public ExampleSubclass01( CustomLogger logger )
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
            _logger.LogStart( false, "This method section is marked as Warning." ,CustomLogType.Warning );
            _logger.LogIndentStart( "This is subclass level 2." );
            _logger.Log( "Subclass sub log 1" );
            _logger.Log( "Subclass sub log 2, overriding parent's type with Error.", CustomLogType.Error );
            _logger.Log( "Subclass sub log 3" );
            _logger.LogEnd();
        }
    }
}
