using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BUCore.UI
{
    public class MultiSelectorGroup : MonoBehaviour
    {
        #region Group Functions
        public IEnumerable<Toggle> GetAllSelected() => GetComponentsInChildren<Toggle>().Where((toggle) => toggle.isOn);

        public IEnumerable<T> GetAllSelected<T>() where T : MonoBehaviour => GetComponentsInChildren<T>().Where((child) => child.GetComponentInChildren<Toggle>() is Toggle toggle && toggle.isOn);
        #endregion
    }
}