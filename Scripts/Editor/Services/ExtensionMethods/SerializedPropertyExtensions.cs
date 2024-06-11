using System;
using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.UI;
using UnityEditor;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Editor.Services.ExtensionMethods
{
    public static class SerializedPropExtensions
    {
        /// <summary>
        ///     Returns the ID of the Unity object, or 0 if is doesn't have one.
        /// </summary>
        /// <param name="property">Serialized Property that contains the value to be extracted.</param>
        /// <returns>The Unity Object ID of the property as an int.</returns>
        public static int GetUnityObjectId( this SerializedProperty property )
        {
            if( property.propertyType == SerializedPropertyType.ObjectReference &&
                property.objectReferenceValue != null )
                return property.objectReferenceValue.GetInstanceID();
            return 0;
        }

        /// <summary>
        ///     Get the value of the serialized property as a string.
        /// </summary>
        /// <param name="property">Serialized Property that contains the value to be extracted.</param>
        /// <returns>The value of the property as a string.</returns>
        public static string GetValueAsString( this SerializedProperty property )
        {
            return property.propertyType switch
            {
                SerializedPropertyType.String => property.stringValue,

                // This will never be called as chars are typed as ints.
                SerializedPropertyType.Character => ( property.type == "char"
                    ? Convert.ToChar( property.intValue ).ToString()
                    : property.intValue.ToString() ),
                SerializedPropertyType.Integer => ( property.type == "char"
                    ? Convert.ToChar( property.intValue ).ToString()
                    : property.intValue.ToString() ),
                SerializedPropertyType.ObjectReference =>
                ( property.objectReferenceValue != null
                    ? property.objectReferenceValue.GetType() == typeof( MonoScript )
                        ? "MonoScript"
                        : property.objectReferenceValue.ToString()
                    : "(null)" ),
                SerializedPropertyType.Boolean => property.boolValue.ToString(),
                SerializedPropertyType.Float => property.floatValue.ToString( "N" ),
                SerializedPropertyType.Color => $"{property.colorValue.ToString()}: {TextFormatting.GetColoredString( "Example", property.colorValue )}",
                SerializedPropertyType.Vector2 => property.vector2Value.ToString(),
                SerializedPropertyType.Vector2Int => property.vector2IntValue.ToString(),
                SerializedPropertyType.Vector3 => property.vector3Value.ToString(),
                SerializedPropertyType.Vector3Int => property.vector3IntValue.ToString(),
                SerializedPropertyType.Vector4 => property.vector4Value.ToString(),
                SerializedPropertyType.Rect => property.rectValue.ToString(),
                SerializedPropertyType.RectInt => property.rectIntValue.ToString(),
                SerializedPropertyType.ArraySize =>

                // At this properties level, the array itself can't be accesses so printing arraySize will
                // throw an error. Print at parent level only.
                string.Empty,
                SerializedPropertyType.Bounds => property.boundsValue.ToString(),
                SerializedPropertyType.BoundsInt => property.boundsIntValue.ToString(),
                SerializedPropertyType.Quaternion => property.quaternionValue.ToString(),
                SerializedPropertyType.Generic =>

                // Skip if "Array" as it will just echo the print from it's parent.
                ( property.isArray && !property.name.Equals( "Array" )
                    ? $"This is an array of {property.arraySize.ToString()} '{property.arrayElementType}' elements"
                    : string.Empty ),
                SerializedPropertyType.ExposedReference => string.Empty,
                SerializedPropertyType.FixedBufferSize => string.Empty,
                SerializedPropertyType.Gradient => string.Empty,
                SerializedPropertyType.AnimationCurve => string.Empty,
                SerializedPropertyType.LayerMask => string.Empty,
                SerializedPropertyType.Enum => string.Empty,
                SerializedPropertyType.ManagedReference => string.Empty,
                _ => string.Empty
            };
        }
    }
}