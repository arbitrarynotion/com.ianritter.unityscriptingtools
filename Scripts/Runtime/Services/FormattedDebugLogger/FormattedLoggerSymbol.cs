using System;

using Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Graphics.CustomColors;

using UnityEngine;

namespace Packages.com.ianritter.unityscriptingtools.Scripts.Runtime.Services.FormattedDebugLogger
{
    [Serializable]
    public class FormattedLoggerSymbol
    {
        private string _hexColor;

        [SerializeField] private CustomColor customColor;

        [SerializeField] private bool toggle;


        public FormattedLoggerSymbol( bool toggle, string symbol, Color customColor )
        {
            this.toggle = toggle;
            this.customColor = new CustomColor( symbol, customColor );
            UpdateHexColor();
        }

        public string GetHexColor() => _hexColor;
        public void UpdateHexColor() => _hexColor = customColor.GetHex();

        public string GetSymbol() => toggle ? customColor.name : string.Empty;
    }
}