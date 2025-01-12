using Sirenix.OdinInspector;
using UnityEngine;

namespace _App.Runtime.UI.Utils
{
    
    public class UIRotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100f;

        private void LateUpdate()
        {
            Rotate();
        }

        [Button]
        private void Rotate()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}