using UnityEngine;

namespace Assets.Scripts.GameInterface
{
    /// <summary> Handles keeping a modal window active. </summary>
    [DisallowMultipleComponent]
    public class ModalWindowController : MonoBehaviour
    {
        #region Properties
        /// <summary> The currently active modal window, or null if none exists. </summary>
        public GameObject CurrentModalWindow { get; private set; }
        #endregion

        #region Window Functions
        /// <summary> Creates an instance of the given <paramref name="prefab"/>, deactivates every other UI element, and returns the component of type <typeparamref name="T"/> found from the instance. </summary>
        /// <typeparam name="T"> The type of <see cref="Component"/> to return. </typeparam>
        /// <param name="prefab"> The UI element to create an instance of. </param>
        /// <returns> The <see cref="Component"/> of type <typeparamref name="T"/> obtained from the created instance. </returns>
        public T CreateModalWindow<T>(GameObject prefab) where T : Component
        {
            // If there's currently a modal window, do nothing.
            if (CurrentModalWindow != null) return null;

            // Deactivate everything before the window is created.
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);

            // Create an instance of the prefab.
            GameObject window = Instantiate(prefab, transform);

            // Set the current modal to the created window.
            CurrentModalWindow = window;

            // Return the found component.
            return window.GetComponent<T>();
        }
        
        /// <summary> Destroy and unset the current modal window. </summary>
        public void DestroyCurrentModalWindow()
        {
            // If there is no current modal window, do nothing.
            if (CurrentModalWindow == null) return;

            // Destroy the current window.
            Destroy(CurrentModalWindow);

            // Activate everything.
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
        }
        #endregion
    }
}