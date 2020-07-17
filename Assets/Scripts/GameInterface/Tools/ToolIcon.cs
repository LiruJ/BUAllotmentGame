using Assets.Scripts.Player.Tools;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tools
{
    public class ToolIcon : Toggle
    {
        #region Inspector Fields
        [SerializeField]
        private ToolType toolType = ToolType.None;
        #endregion

        #region Events
        [Serializable]
        public class ToolEvent : UnityEvent<ToolType> { }

        [SerializeField]
        private ToolEvent onSelected = new ToolEvent();

        public ToolEvent OnSelected => onSelected;
        #endregion

        #region Initialisation Functions
        protected override void Start()
        {
            base.Start();
            onValueChanged.AddListener((selected) => { if (selected) onSelected.Invoke(toolType); });
        }
        #endregion
    }
}