using System;
using UnityEngine;

namespace _App.Runtime.UI.Data
{
    [Serializable]
    public class NavigationBarButtonData
    {
        [SerializeField] private Color buttonColor = Color.blue;
        [SerializeField] private Sprite icon;

        public Color Color => buttonColor;
        public Sprite Icon => icon;
    }
}