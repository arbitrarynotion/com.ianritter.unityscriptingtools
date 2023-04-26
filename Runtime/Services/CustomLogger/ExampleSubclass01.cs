namespace Services.CustomLogger
{
    public class ExampleSubclass01
    {
        private CustomLogger _logger;

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
            _logger.LogStart();
            _logger.LogIndentStart( "This is subclass level 2." );
            _logger.Log( "Subclass sub log 1" );
            _logger.Log( "Subclass sub log 2" );
            _logger.Log( "Subclass sub log 3" );
            _logger.LogEnd();
        }
    }
}
