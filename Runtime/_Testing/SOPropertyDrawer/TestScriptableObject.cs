using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime._Testing.SOPropertyDrawer
{
    [CreateAssetMenu(menuName = "TestScriptableObject")]
    public class TestScriptableObject : ScriptableObject
    {
        [Range( 0f, 1f )]
        public float intSlider = 1f;
    }
}