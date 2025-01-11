using System.Collections.Generic;
using _App.Runtime.UI.Base;
using UnityEngine;

namespace _App.Runtime.UI.Data
{
    [CreateAssetMenu(menuName = "Create ScreensDataBase", fileName = "ScreensData", order = 0)]
    public class ScreensDatabase : ScriptableObject
    {
        [SerializeField] private List<BaseScreen> screens;
        public IReadOnlyList<BaseScreen> Screens => screens;
    }
}