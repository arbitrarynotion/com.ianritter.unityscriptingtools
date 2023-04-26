using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger
{
    [ExecuteInEditMode]
    public class Example01 : MonoBehaviour
    {
        /* Customlogger is a scriptable object class so you need to make a new instance in the assets folder by right clicking
         * then selecting Create/Custom Logger.
         * There are two ways to get that CustomLogger instance into the class where you intend to use it.
         * 
         * Case 1. The target class is either a Monobehaviour or a ScriptableObject class:
         * Make a CustomLogger field and either make it public or private with a [SerializeField] attribute. This will
         * make the field show up in the inspector when you select the game object in the hierarchy that has the target
         * script as a component and assign the CustomLogger instance to that field.
         * 
         * Case 2. The custom class is not a Monobehaviour or ScriptableObject so it doesn't have a direct way to expose its fields to
         * the inspector:
         * Make a CustomLogger field, it can be private if needed.
         * Make a public method dedicated to loading the CustomLogger instance and use the AssetLoader and be sure to check
         * if it's null in case the loading fails.
         * As the target class is not part of the Unity script life cycle, this CustomLogger loading method will need to be
         * called by a script that is a Monobehaviour or a Scriptable object. Typically, the best place to do this is in
         * the OnEnable() method.
         *
         * Limitations on when the logger can be used:
         * In Monobehaviours and ScriptableObjects, the CustomLogger can not be used in the constructor. The script life
         * cycle goes Constructor -> Awake -> OnEnable -> Start -> etc. So it can be used any time after the constructor.
         * In custom classes that are instantiated by a Monobehaviours and ScriptableObject, if the custom class's
         * instantiation can be safely postponed until Awake, the CustomLogger will have been initialized by that point
         * so you're free to use it in the sub class's constructor.
         * If you must instantiate the sub class in the parent class's constructor, then you can't use the CustomLogger
         * in its constructor either.
         */

        // Case 1:
        [SerializeField] private CustomLogger logger;
        private ExampleSubclass01 _exampleSubclass01;

        public Example01()
        {
            // Don't use the CustomLogger here (constructor) as it has not yet been initialized.
            // Do not initialize sub classes here if they depend on the logger.
            Debug.Log( "CustomLoggerExample's constructor was called." );
        }

        public void Awake()
        {
            logger.LogStart();
            logger.Log( "CustomLoggerExample Awake was called." );
            logger.LogEnd();
        }

        private void Start()
        {
            logger.LogStart();
            logger.Log( "CustomLoggerExample Start was called." );
            logger.LogEnd();
        }

        private void OnEnable()
        {
            _exampleSubclass01 = new ExampleSubclass01( logger );
            logger.LogStart( true );
            logger.Log( "Running CustomLogger Test..." );
            TestMethodLevel_1();
            logger.Log( "CustomLogger Test Complete." );
            logger.LogEnd();
        }


        private void TestMethodLevel_1()
        {
            logger.LogStart();
            logger.Log( "This is level 1." );
            TestMethodLevel_2();
            logger.LogEnd();
        }

        private void TestMethodLevel_2()
        {
            logger.LogStart();
            logger.LogIndentStart( "This is level 2." );
            logger.LogIndentStart( "Sub log 1." );
            logger.Log( "Sub log 2." );
            TestMethodLevel_3();
            logger.Log( "Sub log 3." );
            logger.DecrementMethodIndent();
            logger.Log( "Sub log 4." );
            logger.LogEnd();
        }
    
        private void TestMethodLevel_3()
        {
            logger.LogStart();
            logger.Log( "This is level 3." );
            _exampleSubclass01.SubClassMethodLevel_1();
            logger.LogEvent( "This is an event log." );
            logger.LogWarning( "This is a warning log." );
            logger.LogError( "This is an error log." );
            logger.LogEnd();
        }
    
    }
}
