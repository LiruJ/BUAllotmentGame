using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    public class AvailableStatItem : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [SerializeField]
        private Toggle toggle = null;

        [SerializeField]
        private Text text = null;
        #endregion

        #region Properties
        public string StatName { get; private set; }
        #endregion

        #region Initialisation Functions
        public void CreateFrom(ToggleGroup toggleGroup, string statName)
        {
            toggle.group = toggleGroup;
            text.text = statName;
            StatName = statName;
        }
        #endregion
    }
}