using Assets.Scripts.Map;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    [Serializable]
    public class Tool : MonoBehaviour
    {
        #region Inspector Fields
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

            TileIndicator.ChangeObjectGhost(null);
        }
        #endregion

        #region Update Functions
        public virtual void HandleInput() { }
        #endregion
    }
}