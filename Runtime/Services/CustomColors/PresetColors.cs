using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Services.CustomColors
{
    public static class PresetColors
    {
        public static CustomColor Black = new CustomColor( nameof( Black ), new Color(0, 0, 0 ) );
        public static CustomColor Grey = new CustomColor( nameof( Black ), new Color(0.5f, 0.5f, 0.5f ) );
        public static CustomColor White = new CustomColor( nameof( White ), new Color(1, 1, 1 ) );
        public static CustomColor Red = new CustomColor( nameof( Red ), new Color(.9f, .3f, .3f ) );
        public static CustomColor Green = new CustomColor( nameof( Green ), new Color(.3f, .8f, .3f ) );
        public static CustomColor Lime = new CustomColor( nameof( Green ), new Color(.63f, .77f, .2f ) );
        public static CustomColor Blue = new CustomColor( nameof( Blue ), new Color(.3f, .6f, .9f ) );
        public static CustomColor Teal = new CustomColor( nameof( Blue ), new Color(.21f, .85f, .76f ) );
        public static CustomColor Yellow = new CustomColor( nameof( Yellow ), new Color(.9f, .8f, .4f ) );
        public static CustomColor Orange = new CustomColor( nameof( Orange ), new Color(.9f, .5f, .2f ) );
        public static CustomColor Purple = new CustomColor( nameof( Purple ), new Color(.7f, .1f, .9f ) );

        public static List<CustomColor> GetAllColors()
        {
            IEnumerable<FieldInfo> fieldInfoArray = ( GetAllFields( typeof( PresetColors ) ) );
            return fieldInfoArray.Select( fieldInfo => ( (CustomColor) fieldInfo.GetValue( null ) ) ).ToList();
        }
        
        public static IEnumerable<FieldInfo> GetAllFields(Type type) {
            return type.GetNestedTypes().SelectMany(GetAllFields)
                .Concat(type.GetFields());
        }
    }
}
