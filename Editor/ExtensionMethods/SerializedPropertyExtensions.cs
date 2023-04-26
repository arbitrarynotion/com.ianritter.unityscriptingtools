using System;
using Services.TextFormatting;
using UnityEditor;

namespace Editor.ExtensionMethods
{
    public static class SerializedPropExtensions
    {
        
        /// <summary>
        ///     Returns the ID of the Unity object, or 0 if is doesn't have one.
        /// </summary>
        /// <param name="prop">Serialized Property that contains the value to be extracted.</param>
        /// <returns>The Unity Object ID of the property as an int.</returns>
        public static int GetUnityObjectID( this SerializedProperty prop )
        {
            if ( prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue != null )
                return prop.objectReferenceValue.GetInstanceID();
            return 0;
        }
        
        /// <summary>
        ///     Get the value of the serialized property as a string.
        /// </summary>
        /// <param name="property">Serialized Property that contains the value to be extracted.</param>
        /// <returns>The value of the property as a string.</returns>
        public static string GetValueAsString( this SerializedProperty property )
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return property.stringValue;
                
                // This will never be called as chars are typed as ints.
                case SerializedPropertyType.Character:
                    
                case SerializedPropertyType.Integer:
                    return ( property.type == "char" ) ? Convert.ToChar( property.intValue ).ToString() : property.intValue.ToString();

                case SerializedPropertyType.ObjectReference:
                    return ( property.objectReferenceValue != null ) ? property.objectReferenceValue.ToString() : "(null)";
                
                case SerializedPropertyType.Boolean:
                    return property.boolValue.ToString();
                
                case SerializedPropertyType.Float:
                    return property.floatValue.ToString( "N");
                
                case SerializedPropertyType.Color:
                    return $"{property.colorValue.ToString()}: {TextFormat.GetColoredString( "Example", property.colorValue )}";
                
                case SerializedPropertyType.Vector2:
                    return property.vector2Value.ToString();
                case SerializedPropertyType.Vector2Int:
                    return property.vector2IntValue.ToString();
                
                case SerializedPropertyType.Vector3:
                    return property.vector3Value.ToString();
                case SerializedPropertyType.Vector3Int:
                    return property.vector3IntValue.ToString();
                
                case SerializedPropertyType.Vector4:
                    return property.vector4Value.ToString();
                
                case SerializedPropertyType.Rect:
                    return property.rectValue.ToString();
                case SerializedPropertyType.RectInt:
                    return property.rectIntValue.ToString();
                
                case SerializedPropertyType.ArraySize:
                    // A this properties level, the array itself can't be accesses so printing arraySize will
                    // throw an error. Print at parent level only.
                    return string.Empty;
                
                case SerializedPropertyType.Bounds:
                    return property.boundsValue.ToString();
                case SerializedPropertyType.BoundsInt:
                    return property.boundsIntValue.ToString();
                
                case SerializedPropertyType.Quaternion:
                    return property.quaternionValue.ToString();
                
                case SerializedPropertyType.Generic:
                    // Skip if "Array" as it will just echo the print from it's parent.
                    return property.isArray && !property.name.Equals( "Array" ) ? $"This is an array of {property.arraySize.ToString()} '{property.arrayElementType}' elements" : string.Empty;
                    
                case SerializedPropertyType.ExposedReference:
                case SerializedPropertyType.FixedBufferSize:
                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.AnimationCurve:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.ManagedReference:
                default:
                    return "";
            }
        }
    }
}