using UnityEngine;

namespace Assets.Scripts.GameInterface
{
    [DisallowMultipleComponent]
    public class ModalWindowController : MonoBehaviour
    {
        #region Properties
        public GameObject CurrentModalWindow { get; private set; }
        #endregion

        #region Window Functions
        public T CreateModalWindow<T>(GameObject prefab) where T : Component
        {
            // If there's currently a modal window, do nothing.
            if (CurrentModalWindow != null) return null;

            // Create an instance of the prefab.
            GameObject window = Instantiate(prefab, transform);

            // Set the current modal to the created window.
            CurrentModalWindow = window;

            // Deactivate everything other than the window.
            

            // Return the found component.
            return window.GetComponent<T>();
        }
        #endregion
    }
}