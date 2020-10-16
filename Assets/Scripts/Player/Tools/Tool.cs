using Assets.Scripts.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player.Tools
{
    /// <summary> The base tool. </summary>
    public abstract class Tool : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The UI event system, used to ensure tools do not work when the UI is being used.")]
        [SerializeField]
        protected EventSystem eventSystem = null;

        [Header("Tool Settings")]
        [Tooltip("The type of tool this is.")]
        [SerializeField]
        private ToolType toolType = ToolType.None;

        [Tooltip("The name of this tool.")]
        [SerializeField]
        private string toolName = string.Empty;

        [Tooltip("The icon representing this tool.")]
        [SerializeField]
        private Sprite icon = null;
        #endregion

        #region Fields
        /// <summary> The <see cref="ToolBelt"/> that holds this <see cref="Tool"/>. </summary>
        protected ToolBelt toolBelt;
        #endregion

        #region Properties
        /// <summary> The <see cref="Camera"/> used by the player. </summary>
        protected Camera playerCamera => toolBelt.Player.PlayerCamera;

        /// <summary> The main world map. </summary>
        protected WorldMap worldMap => toolBelt.Player.WorldMap;

        /// <summary> The object used to indicate the player's action. </summary>
        public PlacementIndicator TileIndicator => toolBelt.TileIndicator;

        /// <summary> The type of tool this is. </summary>
        public ToolType ToolType => toolType;

        /// <summary> The name of this tool. </summary>
        public virtual string ToolName => toolName;

        /// <summary> The icon representing this tool. </summary>
        public virtual Sprite Icon => icon;
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Set the toolbelt.
            toolBelt = GetComponentInParent<ToolBelt>();

            // Call the initialise tool function for the derived tool type.
            initialiseTool();
        }

        /// <summary> This is fired after the tool's underlying references have been solved and any tool-specific logic can be initialised. </summary>
        protected virtual void initialiseTool() { }

        /// <summary> Is fired when this tool is selected. </summary>
        public virtual void OnSelected() { }

        /// <summary> Is fired when this tool is deselected. Hides the placement indicator by default. </summary>
        public virtual void OnDeselected() 
        {
            TileIndicator.ShowGridGhost = false;
            TileIndicator.ShowObjectGhost = false;

            TileIndicator.ObjectGhost = null;
        }
        #endregion

        #region Tool Functions
        public void SetAsCurrentTool() => toolBelt.CurrentToolType = toolType;
        #endregion

        #region Update Functions
        /// <summary> Is fired every update when this tool is selected. </summary>
        public virtual void HandleInput() { }
        #endregion
    }
}