using System;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors
{
    [Serializable]
    public class CustomColor
    {
        public Color color;
        public string name;

        public CustomColor( string name, Color color )
        {
            this.name = name;
            this.color = color;
        }

        public string GetHex() => $"#{ColorUtility.ToHtmlStringRGBA( color )}";
    }
}