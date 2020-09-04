using Assets.Scripts.GameInterface;
using Assets.Scripts.Player.Tools;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tools
{
    public class ToolBar : MonoBehaviour, ISelectionBar<ToolIcon>
    {
        #region Events
        [Serializable]
        private class toolEvent : UnityEvent<ToolType> { }

        [SerializeField]
        private toolEvent onCurrentToolChanged = new toolEvent();
        #endregion

        #region Initialisation Functions
        private void Start() => GetComponent<HorizontalLayoutGroup>().enabled = true;

        public void OnButtonSelected(ToolIcon button) => onCurrentToolChanged.Invoke(button.ToolType);
        #endregion
    }
}