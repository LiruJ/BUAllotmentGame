using Assets.Scripts.Player.Tools;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tools
{
    public class ToolBar : MonoBehaviour
    {
        #region Events
        [Serializable]
        private class toolEvent : UnityEvent<ToolType> { }

        [SerializeField]
        private toolEvent onCurrentToolChanged = new toolEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            GetComponent<HorizontalLayoutGroup>().enabled = true;

            ToolIcon[] toolIcons = GetComponentsInChildren<ToolIcon>();

            foreach (ToolIcon toolIcon in toolIcons)
                toolIcon.OnSelected.AddListener((toolType) =>  onCurrentToolChanged.Invoke(toolType));
        }
        #endregion
    }
}