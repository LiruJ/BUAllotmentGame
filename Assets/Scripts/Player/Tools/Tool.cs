using Assets.Scripts.Map;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player.Tools
{
    [Serializable]
    public class Tool : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        protected EventSystem eventSystem = null;

        [Header("Tool Settings")]
        [SerializeField]
        private ToolType toolType = ToolType.None;
        #endregion

        #region Fields
        protected ToolBelt toolBelt;
        #endregion

        #region Properties
        public Camera PlayerCamera => toolBelt.PlayerCamera;

        public WorldMap WorldMap => toolBelt.WorldMap;

        public PlacementIndicator TileIndicator => toolBelt.TileIndicator;

        public ToolType ToolType => toolType;
        #endregion

        #region Initialisation Functions
        public virtual void Start() => toolBelt = GetComponentInParent<ToolBelt>();

        public virtual void OnSelected() { }

        public virtual void OnDeselected() 
        {
            TileIndicator.ShowGridGhost = false;
            TileIndicator.ShowObjectGhost = false;

            TileIndicator.ObjectGhost = null;
        }
        #endregion

        #region Update Functions
        public virtual void HandleInput() { }
        #endregion
    }
}