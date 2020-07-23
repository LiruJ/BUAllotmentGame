using Assets.Scripts.Player.Tools;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tools
{
    /// <summary> Handles easy selection of the base <see cref="Toggle"/> and holds a <see cref="ToolType"/>. </summary>
    public class ToolIcon : Toggle
    {
        #region Inspector Fields
        [Header("Tool Icon Settings")]
        [Tooltip("The type of tool to select when this toggle is selected.")]
        [SerializeField]
        private ToolType toolType = ToolType.None;
        #endregion

        #region Events
        [Serializable]
        public class ToolEvent : UnityEvent<ToolType> { }

        [Tooltip("Is fired when this toggle is selected.")]
        [SerializeField]
        private ToolEvent onSelected = new ToolEvent();

        /// <summary> Is fired when this toggle is selected. </summary>
        public ToolEvent OnSelected => onSelected;

        [Tooltip("Is fired when this toggle is deselected.")]
        [SerializeField]
        private ToolEvent onDeselected = new ToolEvent();

        /// <summary> Is fired when this toggle is deselected. </summary>
        public ToolEvent OnDeselected => onDeselected;
        #endregion

        #region Initialisation Functions
        protected override void Start()
        {
            // Initialise the toggle class first.
            base.Start();
            
            // Every time this toggle's value is changed, fire the selected or deselected event based on if the toggle is being selected or deselected.
            onValueChanged.AddListener((selected) => { if (selected) onSelected.Invoke(toolType); else onDeselected.Invoke(toolType); });
        }
        #endregion
    }
}