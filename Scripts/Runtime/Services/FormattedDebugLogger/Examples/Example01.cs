using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Enums;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger.Examples
{
    /// <summary>
    ///     Customlogger is a scriptable object class so you need to make a new instance in the assets folder by right clicking<br/>
    ///     then selecting Create/Custom Logger.<br/>
    ///     There are two ways to get that FormattedLogger instance into the class where you intend to use it.<br/>
    ///     <br/>
    ///     Case 1a. The target class is either a Monobehaviour or a ScriptableObject class:<br/>
    ///     Make a FormattedLogger field and either make it public or private with a [SerializeField] attribute. This will<br/>
    ///     make the field show up in the inspector where you select the game object in the hierarchy that has the target<br/>
    ///     script as a component and assign the FormattedLogger instance to that field.<br/>
    ///     <br/>
    ///     Case 1b. The target class is either a Monobehaviour or a ScriptableObject class but you want to auto-load the logger:<br/>
    ///     Make a FormattedLogger field and either make it public or private with a [SerializeField] attribute.<br/>
    ///     Make a Custom Editor script for that script and have it load the logger using the AssetLoader, making sure that it<br/>
    ///     successfully loads.
    ///     <br/>
    ///     Case 2. The custom class is not a Monobehaviour or ScriptableObject so it doesn't have a direct way to expose its fields to<br/>
    ///     the inspector:<br/>
    ///     Make a FormattedLogger field, it can be private if needed.<br/>
    ///     Make a public method dedicated to loading the FormattedLogger instance and use the AssetLoader and be sure to check<br/>
    ///     if it's null in case the loading fails.<br/>
    ///     As the target class is not part of the Unity script life cycle, this FormattedLogger loading method will need to be<br/>
    ///     called by a script that is a Monobehaviour or a Scriptable object. Typically, the best place to do this is in<br/>
    ///     the OnEnable() method.<br/>
    ///     Important! Note that this means a non-editor script will be using UnityEditor which will break the game if you build it<br/>
    ///     without first filtering that code out. In this case, you can filter it using the #if UNITY_EDITOR/#endif directive, though<br/>
    ///     that's bad practice.<br/>
    ///     <br/>
    ///     Limitations on when the logger can be used:<br/>
    ///     In Monobehaviours and ScriptableObjects, the FormattedLogger can not be used in the constructor. The script life<br/>
    ///     cycle goes Constructor -> Awake -> OnEnable -> Start -> etc. So it can be used any time after the constructor.<br/>
    ///     In custom classes that are instantiated by a Monobehaviours and ScriptableObject, if the custom class's<br/>
    ///     instantiation can be safely postponed until Awake, the FormattedLogger will have been initialized by that point<br/>
    ///     so you're free to use it in the sub class's constructor.<br/>
    ///     If you must instantiate the sub class in the parent class's constructor, then you can't use the FormattedLogger<br/>
    ///     in its constructor either.<br/>
    /// </summary>
    [ExecuteInEditMode]
    public class Example01 : MonoBehaviour
    {
        private ExampleSubclass01 _exampleSubclass01;

        // Case 1a:
        [SerializeField] private FormattedLogger logger;

        public Example01()
        {
            // Don't use the FormattedLogger here (constructor) as it has not yet been initialized.
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
            logger.Log( "This is an independent log, not called between LogStart and LogEnd." );
            logger.LogStart( true );
            logger.Log( "Running FormattedLogger Test..." );
            TestMethodLevel_1();
            logger.Log( "FormattedLogger Test Complete." );
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
            logger.LogIndentStart( "This is level 2, starting a new indent." );
            logger.LogIndentStart( "Sub log 1, starting another new indent." );
            logger.LogIndentEnd( "Sub log 2, ending 1 indent." );
            TestMethodLevel_3();
            logger.LogIndentEnd( "Sub log 3" );

            // logger.DecrementMethodIndent();
            logger.Log( "Sub log 4." );
            logger.LogEnd();
        }

        private void TestMethodLevel_3()
        {
            logger.LogStart();
            logger.Log( "This is level 3." );
            _exampleSubclass01.SubClassMethodLevel_1();
            logger.LogEvent( "This is an event log." );
            logger.Log( "This is a warning log.", FormattedLogType.Warning );
            logger.Log( "This is an error log.", FormattedLogType.Error );
            logger.LogEnd( "This is another end log message. Good for providing an explanation in a method with multiple exits." );
        }
    }
}