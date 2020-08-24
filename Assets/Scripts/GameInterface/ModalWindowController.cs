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