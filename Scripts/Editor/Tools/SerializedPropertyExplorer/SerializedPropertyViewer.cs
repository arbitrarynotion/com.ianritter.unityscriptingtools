using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Tools.SerializedPropertyExplorer
{
    public class SerializedPropertyViewer : EditorWindow
    {
        public static GUIStyle richTextStyle;
        private List<SPData> data;

        private bool debugMode;
        private bool dirty = true;

        private Object obj;

        private Vector2 scrollPos;
        private string searchStr = "";
        private string searchStrRep;

        [MenuItem( "Tools/SP Viewer" )]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (SerializedPropertyViewer) GetWindow( typeof( SerializedPropertyViewer ) );
            window.titleContent = new GUIContent( "SP Viewer" );
            window.Show();
        }


        private void OnGUI()
        {
            if( richTextStyle == null )
            {
                //EditorStyles does not exist in Constructor??
                richTextStyle = new GUIStyle( EditorStyles.label );
                richTextStyle.richText = true;
            }

            Object newObj = EditorGUILayout.ObjectField( "Object:", obj, typeof( Object ), true );
            debugMode = EditorGUILayout.Toggle( "Debug Mode", debugMode );
            if( GUILayout.Button( "Refresh" ) ) obj = null;

            string newSearchStr = EditorGUILayout.TextField( "Search:", searchStr );
            if( newSearchStr != searchStr )
            {
                searchStr = newSearchStr;
                searchStrRep = "<color=green>" + searchStr + "</color>";
                dirty = true;
            }

            if( obj != newObj )
            {
                obj = newObj;
                dirty = true;
            }

            if( data == null ) dirty = true;
            if( dirty )
            {
                dirty = false;
                searchObject( obj );
            }

            scrollPos = EditorGUILayout.BeginScrollView( scrollPos );
            foreach ( SPData line in data )
            {
                EditorGUI.indentLevel = line.depth;
                if( line.oid > 0 ) GUILayout.BeginHorizontal();
                EditorGUILayout.SelectableLabel( line.info, richTextStyle, GUILayout.Height( 20 ) );
                if( line.oid > 0 )
                {
                    if( GUILayout.Button( ">>", GUILayout.Width( 50 ) ) ) Selection.activeInstanceID = line.oid;

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            if( GUILayout.Button( "Copy To Clipboard" ) )
            {
                var sb = new StringBuilder();
                Dictionary<int, string> paddingHash = new Dictionary<int, string>();
                string padding = "";
                for ( int i = 0; i < 40; i++ )
                {
                    paddingHash[i] = padding;
                    padding += " ";
                }

                foreach ( SPData line in data )
                {
                    sb.Append( paddingHash[line.depth] );
                    sb.Append( line.info );
                    sb.Append( "\n" );
                }

                EditorGUIUtility.systemCopyBuffer = sb.ToString();
            }
        }

        private void searchObject( Object obj )
        {
            data = new List<SPData>();
            if( obj == null ) return;
            var so = new SerializedObject( obj );
            if( debugMode )
            {
                PropertyInfo inspectorModeInfo = typeof( SerializedObject ).GetProperty( "inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance );
                inspectorModeInfo.SetValue( so, InspectorMode.Debug, null );
            }

            SerializedProperty iterator = so.GetIterator();
            search( iterator, 0 );
        }

        private void search( SerializedProperty prop, int depth )
        {
            logProperty( prop );
            while ( prop.Next( true ) )
            {
                logProperty( prop );
            }
        }


        private void logProperty( SerializedProperty prop )
        {
            string strVal = getStringValue( prop );
            string propDesc = prop.propertyPath + " type:" + prop.type + " name:" + prop.name + " val:" + strVal + " isArray:" + prop.isArray;
            if( searchStr.Length > 0 ) propDesc = propDesc.Replace( searchStr, searchStrRep );
            data.Add( new SPData( prop.depth, propDesc, strVal, getObjectID( prop ) ) );
        }

        private int getObjectID( SerializedProperty prop )
        {
            if( prop.propertyType == SerializedPropertyType.ObjectReference &&
                prop.objectReferenceValue != null ) return prop.objectReferenceValue.GetInstanceID();
            return 0;
        }

        private string getStringValue( SerializedProperty prop )
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.String:
                    return prop.stringValue;
                case SerializedPropertyType.Character: //this isn't really a thing, chars are ints!
                case SerializedPropertyType.Integer:
                    if( prop.type == "char" ) return Convert.ToChar( prop.intValue ).ToString();
                    return prop.intValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    if( prop.objectReferenceValue != null )
                        return prop.objectReferenceValue.ToString();
                    else
                        return "(null)";
                case SerializedPropertyType.Float:
                    return prop.floatValue.ToString();
                default:
                    return "";
            }
        }

        public class SPData
        {
            public int depth;
            public string info;
            public int oid;
            public string val;

            public SPData( int d, string i, string v, int o )
            {
                if( d < 0 ) d = 0;
                depth = d;
                info = i;
                val = v;
                oid = o;
            }
        }

        /* Example of copy to clipboard with ObjectStackerSettingsSO object:
         
        type:MonoBehaviour name:Base val: isArray:False
        m_ObjectHideFlags type:uint name:m_ObjectHideFlags val:0 isArray:False
        m_CorrespondingSourceObject type:PPtr<EditorExtension> name:m_CorrespondingSourceObject val:(null) isArray:False
         m_CorrespondingSourceObject.m_FileID type:int name:m_FileID val:0 isArray:False
         m_CorrespondingSourceObject.m_PathID type:long name:m_PathID val:0 isArray:False
        m_PrefabInstance type:PPtr<PrefabInstance> name:m_PrefabInstance val:(null) isArray:False
         m_PrefabInstance.m_FileID type:int name:m_FileID val:0 isArray:False
         m_PrefabInstance.m_PathID type:long name:m_PathID val:0 isArray:False
        m_PrefabAsset type:PPtr<Prefab> name:m_PrefabAsset val:(null) isArray:False
         m_PrefabAsset.m_FileID type:int name:m_FileID val:0 isArray:False
         m_PrefabAsset.m_PathID type:long name:m_PathID val:0 isArray:False
        m_GameObject type:PPtr<GameObject> name:m_GameObject val:(null) isArray:False
         m_GameObject.m_FileID type:int name:m_FileID val:0 isArray:False
         m_GameObject.m_PathID type:long name:m_PathID val:0 isArray:False
        m_Enabled type:bool name:m_Enabled val: isArray:False
        m_EditorHideFlags type:uint name:m_EditorHideFlags val:0 isArray:False
        m_Script type:PPtr<MonoScript> name:m_Script val:using UnityEngine;
        using UnityEngine.Events;

        namespace Packages.com.ianritter.unityscriptingtools.Tools.ObjectStacker.Scripts.Runtime
        {
            [CreateAssetMenu(menuName = "Utilities/Object Stacker Settings")]
            public class ObjectStackerSettingsSO : ScriptableObject
            {

        #region DataMembers
                
        #region NoiseSettings

                public bool showNoiseMeter = true;
                public int seed = 31;
                [Range( -5.0f, 5.0f )] public float noiseOffsetHorizontal = 0f;
                [Range( -5.0f, 5.0f )] public float noiseOffsetVertical = 0f;
                [Range( 1.1f, 50.0f )] public float noiseScale = 12.7f;

                [Range( 1.0f, 8.0f )] public int octaves = 7;
                [Range( 0.5f, 1.0f )] public float persistence = 1f;
                [Range( 0.5f, 1.5f )] public float lacunarity = 1.433f;
                
        #endregion

        #region NoiseDrivenEffects

        #region RotationSkew

                public AnimationCurve noiseDampeningCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
                [Range(0f, 50f)]
                public float noiseMultiplier = 1;
                

        #endregion

        #region PositionShift

                [Range( -0.01f, 0.01f )] 
                public float posXNoiseShift = 0f;
                public AnimationCurve posXNoiseCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

                [Range( -0.01f, 0.01f )] 
                public float posZNoiseShift = 0f;
                public AnimationCurve posZNoiseCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

        #endregion

        #endregion


        #region ManualAdjustments

        #region Rotation

                [Range( -10f, 10f )] public float yRotOffset = 0.05f;
                [Range( -0.5f, 0.5f )] public float deckYRotSkew = -0.244f;
                [Range( 0f, 1f )] public float topDownSkewPercent = 0f;
                [Range( -50f, 50f )] public float topCardsYRotSkew = 39f;
                public AnimationCurve rotationDampeningCurve = new AnimationCurve( new []
                    {
                        new Keyframe( time: 
                            -0.0035514832f,
                            0.0022468567f,
                            0.02133862f,
                            0.02133862f,
                            inWeight: 0,
                            outWeight: 0.69152474f 
                        ),
                        new Keyframe(
                            0.1272265f,
                            0.32834244f,
                            8.195747f,
                            8.195747f,
                            0.33333334f,
                            0.33333334f
                        ),
                        new Keyframe(
                            0.16060005f,
                            0.7679692f,
                            5.6655655f,
                            5.6655655f,
                            0.33333334f,
                            0.33333334f
                        ),
                        new Keyframe(
                            0.3245624f,
                            0.46598676f,
                            -3.7884986f,
                            -3.7884986f,
                            0.22430015f,
                            0.09893375f
                        ),
                        new Keyframe(
                            0.61950034f,
                            0.06995833f,
                            -0.27939424f,
                            -0.27939424f,
                            0.21457842f,
                            0.16766956f
                        ),
                        new Keyframe(
                            1.00354f,
                            0.0022468567f,
                            -0.111717f,
                            -0.111717f,
                            0.20951769f,
                            0f
                        )
                    } 
                );
                public bool faceUp = false;

        #endregion

        #region Position

                public float modelHeight = 1f;
                [Range( 0, 0.001f )] public float verticalOffset = 0.0003f;

        #endregion
        #endregion
        #endregion

                
        #region Events

                [SerializeField]
                public UnityAction onSettingsUpdated;
                private void RaiseOnSettingsUpdated() => onSettingsUpdated?.Invoke();

        #endregion


        #region LifeCycle

                private void OnValidate()
                {
                    // Debug.Log( "ObjectStackerSettingsSO OnValidate called." );
                    RaiseOnSettingsUpdated();
                }

        #endregion
            }
        } isArray:False
         m_Script.m_FileID type:int name:m_FileID val:5266 isArray:False
         m_Script.m_PathID type:long name:m_PathID val:0 isArray:False
        m_Name type:string name:m_Name val:ObjectStackerSettings_01 isArray:True
        m_Name.Array type:Array name:Array val: isArray:True
         m_Name.Array.size type:ArraySize name:size val: isArray:False
         m_Name.Array.data[0] type:char name:data val:O isArray:False
         m_Name.Array.data[1] type:char name:data val:b isArray:False
         m_Name.Array.data[2] type:char name:data val:j isArray:False
         m_Name.Array.data[3] type:char name:data val:e isArray:False
         m_Name.Array.data[4] type:char name:data val:c isArray:False
         m_Name.Array.data[5] type:char name:data val:t isArray:False
         m_Name.Array.data[6] type:char name:data val:S isArray:False
         m_Name.Array.data[7] type:char name:data val:t isArray:False
         m_Name.Array.data[8] type:char name:data val:a isArray:False
         m_Name.Array.data[9] type:char name:data val:c isArray:False
         m_Name.Array.data[10] type:char name:data val:k isArray:False
         m_Name.Array.data[11] type:char name:data val:e isArray:False
         m_Name.Array.data[12] type:char name:data val:r isArray:False
         m_Name.Array.data[13] type:char name:data val:S isArray:False
         m_Name.Array.data[14] type:char name:data val:e isArray:False
         m_Name.Array.data[15] type:char name:data val:t isArray:False
         m_Name.Array.data[16] type:char name:data val:t isArray:False
         m_Name.Array.data[17] type:char name:data val:i isArray:False
         m_Name.Array.data[18] type:char name:data val:n isArray:False
         m_Name.Array.data[19] type:char name:data val:g isArray:False
         m_Name.Array.data[20] type:char name:data val:s isArray:False
         m_Name.Array.data[21] type:char name:data val:_ isArray:False
         m_Name.Array.data[22] type:char name:data val:0 isArray:False
         m_Name.Array.data[23] type:char name:data val:1 isArray:False
        m_EditorClassIdentifier type:string name:m_EditorClassIdentifier val: isArray:True
        m_EditorClassIdentifier.Array type:Array name:Array val: isArray:True
         m_EditorClassIdentifier.Array.size type:ArraySize name:size val: isArray:False
        showNoiseMeter type:bool name:showNoiseMeter val: isArray:False
        seed type:int name:seed val:42 isArray:False
        noiseOffsetHorizontal type:float name:noiseOffsetHorizontal val:0.56 isArray:False
        noiseOffsetVertical type:float name:noiseOffsetVertical val:0 isArray:False
        noiseScale type:float name:noiseScale val:4.4 isArray:False
        octaves type:int name:octaves val:6 isArray:False
        persistence type:float name:persistence val:1 isArray:False
        lacunarity type:float name:lacunarity val:0.634 isArray:False
        noiseDampeningCurve type:AnimationCurve name:noiseDampeningCurve val: isArray:False
         noiseDampeningCurve.m_Curve type:vector name:m_Curve val: isArray:True
         noiseDampeningCurve.m_Curve.Array type:Array name:Array val: isArray:True
          noiseDampeningCurve.m_Curve.Array.size type:ArraySize name:size val: isArray:False
          noiseDampeningCurve.m_Curve.Array.data[0] type:Keyframe name:data val: isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].time type:float name:time val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].value type:float name:value val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].inSlope type:float name:inSlope val:2 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].outSlope type:float name:outSlope val:2 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].tangentMode type:int name:tangentMode val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].weightedMode type:int name:weightedMode val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].inWeight type:float name:inWeight val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[0].outWeight type:float name:outWeight val:0 isArray:False
          noiseDampeningCurve.m_Curve.Array.data[1] type:Keyframe name:data val: isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].time type:float name:time val:1 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].value type:float name:value val:1 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].inSlope type:float name:inSlope val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].outSlope type:float name:outSlope val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].tangentMode type:int name:tangentMode val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].weightedMode type:int name:weightedMode val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].inWeight type:float name:inWeight val:0 isArray:False
           noiseDampeningCurve.m_Curve.Array.data[1].outWeight type:float name:outWeight val:0 isArray:False
         noiseDampeningCurve.m_PreInfinity type:int name:m_PreInfinity val:2 isArray:False
         noiseDampeningCurve.m_PostInfinity type:int name:m_PostInfinity val:2 isArray:False
         noiseDampeningCurve.m_RotationOrder type:int name:m_RotationOrder val:4 isArray:False
        noiseMultiplier type:float name:noiseMultiplier val:10.9 isArray:False
        posXNoiseShift type:float name:posXNoiseShift val:0 isArray:False
        posXNoiseCurve type:AnimationCurve name:posXNoiseCurve val: isArray:False
         posXNoiseCurve.m_Curve type:vector name:m_Curve val: isArray:True
         posXNoiseCurve.m_Curve.Array type:Array name:Array val: isArray:True
          posXNoiseCurve.m_Curve.Array.size type:ArraySize name:size val: isArray:False
          posXNoiseCurve.m_Curve.Array.data[0] type:Keyframe name:data val: isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].time type:float name:time val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].value type:float name:value val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].inSlope type:float name:inSlope val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].outSlope type:float name:outSlope val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].tangentMode type:int name:tangentMode val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].weightedMode type:int name:weightedMode val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].inWeight type:float name:inWeight val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[0].outWeight type:float name:outWeight val:0 isArray:False
          posXNoiseCurve.m_Curve.Array.data[1] type:Keyframe name:data val: isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].time type:float name:time val:1 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].value type:float name:value val:1 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].inSlope type:float name:inSlope val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].outSlope type:float name:outSlope val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].tangentMode type:int name:tangentMode val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].weightedMode type:int name:weightedMode val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].inWeight type:float name:inWeight val:0 isArray:False
           posXNoiseCurve.m_Curve.Array.data[1].outWeight type:float name:outWeight val:0 isArray:False
         posXNoiseCurve.m_PreInfinity type:int name:m_PreInfinity val:2 isArray:False
         posXNoiseCurve.m_PostInfinity type:int name:m_PostInfinity val:2 isArray:False
         posXNoiseCurve.m_RotationOrder type:int name:m_RotationOrder val:4 isArray:False
        posZNoiseShift type:float name:posZNoiseShift val:0 isArray:False
        posZNoiseCurve type:AnimationCurve name:posZNoiseCurve val: isArray:False
         posZNoiseCurve.m_Curve type:vector name:m_Curve val: isArray:True
         posZNoiseCurve.m_Curve.Array type:Array name:Array val: isArray:True
          posZNoiseCurve.m_Curve.Array.size type:ArraySize name:size val: isArray:False
          posZNoiseCurve.m_Curve.Array.data[0] type:Keyframe name:data val: isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].time type:float name:time val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].value type:float name:value val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].inSlope type:float name:inSlope val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].outSlope type:float name:outSlope val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].tangentMode type:int name:tangentMode val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].weightedMode type:int name:weightedMode val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].inWeight type:float name:inWeight val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[0].outWeight type:float name:outWeight val:0 isArray:False
          posZNoiseCurve.m_Curve.Array.data[1] type:Keyframe name:data val: isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].time type:float name:time val:1 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].value type:float name:value val:1 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].inSlope type:float name:inSlope val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].outSlope type:float name:outSlope val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].tangentMode type:int name:tangentMode val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].weightedMode type:int name:weightedMode val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].inWeight type:float name:inWeight val:0 isArray:False
           posZNoiseCurve.m_Curve.Array.data[1].outWeight type:float name:outWeight val:0 isArray:False
         posZNoiseCurve.m_PreInfinity type:int name:m_PreInfinity val:2 isArray:False
         posZNoiseCurve.m_PostInfinity type:int name:m_PostInfinity val:2 isArray:False
         posZNoiseCurve.m_RotationOrder type:int name:m_RotationOrder val:4 isArray:False
        yRotOffset type:float name:yRotOffset val:2.2 isArray:False
        deckYRotSkew type:float name:deckYRotSkew val:-0.121 isArray:False
        topDownSkewPercent type:float name:topDownSkewPercent val:0.4 isArray:False
        topCardsYRotSkew type:float name:topCardsYRotSkew val:29.6 isArray:False
        rotationDampeningCurve type:AnimationCurve name:rotationDampeningCurve val: isArray:False
         rotationDampeningCurve.m_Curve type:vector name:m_Curve val: isArray:True
         rotationDampeningCurve.m_Curve.Array type:Array name:Array val: isArray:True
          rotationDampeningCurve.m_Curve.Array.size type:ArraySize name:size val: isArray:False
          rotationDampeningCurve.m_Curve.Array.data[0] type:Keyframe name:data val: isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].time type:float name:time val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].value type:float name:value val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].inSlope type:float name:inSlope val:4.315628 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].outSlope type:float name:outSlope val:4.315628 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].tangentMode type:int name:tangentMode val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].weightedMode type:int name:weightedMode val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].inWeight type:float name:inWeight val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[0].outWeight type:float name:outWeight val:0.03905325 isArray:False
          rotationDampeningCurve.m_Curve.Array.data[1] type:Keyframe name:data val: isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].time type:float name:time val:0.1211313 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].value type:float name:value val:0.9760041 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].inSlope type:float name:inSlope val:1.458603 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].outSlope type:float name:outSlope val:1.458603 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].tangentMode type:int name:tangentMode val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].weightedMode type:int name:weightedMode val:3 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].inWeight type:float name:inWeight val:0.3333333 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[1].outWeight type:float name:outWeight val:0.1134556 isArray:False
          rotationDampeningCurve.m_Curve.Array.data[2] type:Keyframe name:data val: isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].time type:float name:time val:0.4663244 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].value type:float name:value val:1.031123 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].inSlope type:float name:inSlope val:-0.00914129 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].outSlope type:float name:outSlope val:-0.00914129 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].tangentMode type:int name:tangentMode val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].weightedMode type:int name:weightedMode val:3 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].inWeight type:float name:inWeight val:0.7457459 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[2].outWeight type:float name:outWeight val:0.4528379 isArray:False
          rotationDampeningCurve.m_Curve.Array.data[3] type:Keyframe name:data val: isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].time type:float name:time val:0.8152103 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].value type:float name:value val:1.026275 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].inSlope type:float name:inSlope val:-0.2040597 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].outSlope type:float name:outSlope val:-0.2040597 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].tangentMode type:int name:tangentMode val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].weightedMode type:int name:weightedMode val:3 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].inWeight type:float name:inWeight val:0.08599859 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[3].outWeight type:float name:outWeight val:0.2470995 isArray:False
          rotationDampeningCurve.m_Curve.Array.data[4] type:Keyframe name:data val: isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].time type:float name:time val:1 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].value type:float name:value val:0.9955861 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].inSlope type:float name:inSlope val:-0.09413677 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].outSlope type:float name:outSlope val:-0.09413677 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].tangentMode type:int name:tangentMode val:0 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].weightedMode type:int name:weightedMode val:1 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].inWeight type:float name:inWeight val:0.3883101 isArray:False
           rotationDampeningCurve.m_Curve.Array.data[4].outWeight type:float name:outWeight val:0 isArray:False
         rotationDampeningCurve.m_PreInfinity type:int name:m_PreInfinity val:2 isArray:False
         rotationDampeningCurve.m_PostInfinity type:int name:m_PostInfinity val:2 isArray:False
         rotationDampeningCurve.m_RotationOrder type:int name:m_RotationOrder val:4 isArray:False
        faceUp type:bool name:faceUp val: isArray:False
        modelHeight type:float name:modelHeight val:1 isArray:False
        verticalOffset type:float name:verticalOffset val:0.00026 isArray:False

         */
    }
}