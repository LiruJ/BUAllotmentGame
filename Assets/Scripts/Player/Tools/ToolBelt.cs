using Assets.Scripts.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    public class ToolBelt : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The player's camera.")]
        [SerializeField]
        private Camera playerCamera = null;

        [Tooltip("The main world map.")]
        [SerializeField]
        private WorldMap worldMap = null;

        [Tooltip("The object used to indicate the player's action.")]
        [SerializeField]
        private PlacementIndicator tileIndicator = null;
        #endregion

        #region Fields
        private readonly Dictionary<ToolType, Tool> toolsByType = new Dictionary<ToolType, Tool>();
        #endregion

        #region Properties
        public Tool CurrentTool { get; private set; }

        public ToolType CurrentToolType => CurrentTool.ToolType;

        public Camera PlayerCamera => playerCamera;

        public WorldMap WorldMap => worldMap;

        public PlacementIndicator TileIndicator => tileIndicator;
        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {
            foreach (Tool tool in GetComponentsInChildren<Tool>())
                toolsByType.Add(tool.ToolType, tool);

            // Add null as the none tool type, to signify no tool.
            toolsByType.Add(ToolType.None, null);
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            if (CurrentTool != null)
            {
                CurrentTool.HandleInput();
            }
        }

        private void FixedUpdate()
        {

        }
        #endregion

        #region Tool functions
        public void SwitchTool(ToolType toolType)
        {
            if (toolsByType.TryGetValue(toolType, out Tool tool))
            {
                if (CurrentTool != null) CurrentTool.OnDeselected();
                CurrentTool = tool;

                if (CurrentTool != null) CurrentTool.OnSelected();
                else { TileIndicator.ShowGridGhost = false; TileIndicator.ShowObjectGhost = false; }
            }
        }
        #endregion

        #region Screen Functions
        public Vector3Int ScreenPositionToCell(Vector3 screenPosition)
        {
            // Create the ray from the position.
            Ray screenRay = playerCamera.ScreenPointToRay(screenPosition);

            // Simple rearranging of the parametric form of the screen ray, where we know the y position will be the same as the map's y position.
            return worldMap.Grid.WorldToCell(screenRay.origin + (worldMap.transform.position.y - screenRay.origin.y) / screenRay.direction.y * screenRay.direction);
        }
        #endregion
    }
}