using System;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger
{
    [Serializable]
    public class CustomLoggerSymbol
    {
        [SerializeField]
        private bool toggle;
        // [SerializeField]
        // private string symbol;
        [SerializeField]
        private CustomColor customColor;
        
        private string _hexColor;
        

        public CustomLoggerSymbol( bool toggle, string symbol, Color customColor )
        {
            this.toggle = toggle;
            // this.symbol = symbol;
            this.customColor = new CustomColor( symbol, customColor );
            UpdateHexColor();
        }
        
        public string GetHexColor() => _hexColor;
        public void UpdateHexColor() => _hexColor = customColor.GetHex();

        public string GetSymbol() => toggle ? customColor.name : string.Empty;
        
    }
}