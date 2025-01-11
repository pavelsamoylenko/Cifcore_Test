using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _App.Runtime.UI.Data
{
    /// <summary>
    /// Setup of screens in navigation menu. Can be used to control screen sets dynamically (e.g. when new screens are added, changing orders or for A/B testing).
    /// </summary>
    [CreateAssetMenu(menuName = "Create ScreensSetup", fileName = "ScreensSetup", order = 0)]
    public class ScreensSetup : ScriptableObject
    {
        [Header("Dependencies")]
        [SerializeField] private ScreensDatabase database;
        
        [Header("Config")]
        [SerializeField] private List<string> screensIds = new();
        
        public IReadOnlyList<string> ScreensIds => screensIds;

        [ContextMenu("Validate")]
        private void ValidateIds()
        {
            if (!database)
            {
                Debug.LogError("No database reference assigned", this);
                return;
            }

            if (screensIds != null)
            {
                var counter = 0;

                bool duplicatedExits = ScreensIds.Count != ScreensIds.Distinct().Count();
                if(duplicatedExits)
                {
                    Debug.LogError($"Duplicates exist", this);
                    return;
                }

                foreach (string screenId in screensIds)
                {
                    var screenPrefab = database.Screens.FirstOrDefault(s => screenId == s.Id);
                    if (screenPrefab == null)
                    {
                        Debug.LogError($"No screen prefab exist for id: {screenId}");
                        counter++;
                    }
                }

                if (counter == 0)
                {
                    Debug.Log($"Validation success", this);
                }
                else
                {
                    Debug.LogError($"Ids mismatch for {counter} items", this);

                }
            }
        }
    }
}