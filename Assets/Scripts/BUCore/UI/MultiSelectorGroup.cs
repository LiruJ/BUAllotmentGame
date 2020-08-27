using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BUCore.UI
{
    /// <summary> Allows for multiple toggles to be selected at once. </summary>
    public class MultiSelectorGroup : MonoBehaviour
    {
        #region Group Functions
        /// <summary> Gets all selected toggles of this group. </summary>
        /// <returns> All selected toggles of this group. </returns>
        public IEnumerable<Toggle> GetAllSelected() => GetComponentsInChildren<Toggle>().Where((toggle) => toggle.isOn);

        /// <summary> Gets components of type <typeparamref name="T"/> from all selected toggles of this group. </summary>
        /// <typeparam name="T"> The type of Monobehaviour component to get and return. </typeparam>
        /// <returns> Components of type <typeparamref name="T"/> from all selected toggles of this group. </returns>
        public IEnumerable<T> GetAllSelected<T>() where T : MonoBehaviour => GetComponentsInChildren<T>().Where((child) => child.GetComponentInChildren<Toggle>() is Toggle toggle && toggle.isOn);
        #endregion
    }
}