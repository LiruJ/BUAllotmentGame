using Assets.Scripts.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    /// <summary> Holds the player's <see cref="Tool"/>s. </summary>
    public class ToolBelt : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The player who owns this toolbelt.")]
        [SerializeField]
        private HumanPlayer player = null;

        [Tooltip("The object used to indicate the player's action.")]
        [SerializeField]
        private PlacementIndicator tileIndicator = null;
        #endregion

        #region Fields
        /// <summary> The <see cref="Tool"/>s keyed by <see cref="ToolType"/>. </summary>
        private readonly Dictionary<ToolType, Tool> toolsByType = new Dictionary<ToolType, Tool>();
        #endregion

        #region Properties
        /// <summary> The player who owns this toolbelt. </summary>
        public HumanPlayer Player => player;

        /// <summary> Gets the currently active <see cref="Tool"/>, or null if none is selected. </summary>
        public Tool CurrentTool { get; private set; }

        /// <summary> Gets the <see cref="ToolType"/> of the <see cref="CurrentTool"/>. </summary>
        public ToolType CurrentToolType
        {
            get => CurrentTool.ToolType;
            set
            {
                // If the tool exists within the dictionary, switch to it.
                if (toolsByType.TryGetValue(value, out Tool tool))
                {
                    // If a tool is currently selected, fire the deselected event.
                    if (CurrentTool != null) CurrentTool.OnDeselected();

                    // Switch the current tool to the one within the dictionary.
                    CurrentTool = tool;

                    // If the new tool exists, fire the selected event.
                    if (CurrentTool != null) CurrentTool.OnSelected();
                    // Otherwise; hide the indicators.
                    else { TileIndicator.ShowGridGhost = false; TileIndicator.ShowObjectGhost = false; }
                }
            }
        }

        /// <summary> Gets the object used to indicate object placement. </summary>
        public PlacementIndicator TileIndicator => tileIndicator;
        #endregion

        #region Initialisation Functions
        private void Awake()
        {
            // Add null as the none tool type, to signify no tool.
            toolsByType.Add(ToolType.None, null);

            // Add each tool in the children of the containing GameObject to the dictionary, keyed by its type.
            foreach (Tool tool in GetComponentsInChildren<Tool>())
                toolsByType.Add(tool.ToolType, tool);
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            // If a tool is currently selected, pass through the update function.
            if (CurrentTool != null) CurrentTool.HandleInput();
        }
        #endregion

        #region Screen Functions
        /// <summary> Calculates the position of the tile at the given <paramref name="screenPosition"/>. </summary>
        /// <param name="screenPosition"> The position on the screen. </param>
        /// <returns> The tile position at the given <paramref name="screenPosition"/>. </returns>
        public Vector3Int ScreenPositionToCell(Vector3 screenPosition)
        {
            // Create the ray from the position.
            Ray screenRay = Player.PlayerCamera.ScreenPointToRay(screenPosition);

            // Simple rearranging of the parametric form of the screen ray, where we know the y position will be the same as the map's y position.
            return Player.WorldMap.Grid.WorldToCell(screenRay.origin + (Player.WorldMap.transform.position.y - screenRay.origin.y) / screenRay.direction.y * screenRay.direction);
        }
        #endregion
    }
}