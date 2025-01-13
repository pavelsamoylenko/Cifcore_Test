using System.Collections.Generic;
using _App.Runtime.Screens.Base;
using UnityEngine;

namespace _App.Runtime.Screens.Data
{
    [CreateAssetMenu(menuName = "Create ScreensDataBase", fileName = "ScreensData", order = 0)]
    public class ScreensDatabase : ScriptableObject
    {
        [SerializeField] private List<BaseScreen> screens;
        public IReadOnlyList<BaseScreen> Screens => screens;
    }
}