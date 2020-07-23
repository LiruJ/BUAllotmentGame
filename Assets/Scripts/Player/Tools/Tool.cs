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
        #endregion

        #region Fields
        /// <summary> The <see cref="ToolBelt"/> that holds this <see cref="Tool"/>. </summary>
        protected ToolBelt toolBelt;
        #endregion

        #region Properties
        /// <summary> The <see cref="Camera"/> used by the player. </summary>
        public Camera PlayerCamera => toolBelt.PlayerCamera;

        /// <summary> The main world map. </summary>
        public WorldMap WorldMap => toolBelt.WorldMap;

        /// <summary> The object used to indicate the player's action. </summary>
        public PlacementIndicator TileIndicator => toolBelt.TileIndicator;

        /// <summary> The type of tool this is. </summary>
        public ToolType ToolType => toolType;
        #endregion

        #region Initialisation Functions
        public virtual void Start() => toolBelt = GetComponentInParent<ToolBelt>();

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

        #region Update Functions
        /// <summary> Is fired every update when this tool is selected. </summary>
        public virtual void HandleInput() { }
        #endregion
    }
}