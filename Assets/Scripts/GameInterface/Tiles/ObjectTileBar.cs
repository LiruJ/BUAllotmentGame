using Assets.Scripts.Objects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Tiles
{
    /// <summary> The object tile selection bar. </summary>
    public class ObjectTileBar : TileBar, ISelectionBar<TileIcon>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The tileset for the object tiles.")]
        [SerializeField]
        private ObjectTileset objectTileset = null;
        #endregion

        #region Events
        [Serializable]
        private class tileEvent : UnityEvent<ObjectTile> { }

        [Header("Events")]
        [Tooltip("Is fired when a tile is selected.")]
        [SerializeField]
        private tileEvent onSelected = new tileEvent();
        #endregion

        #region Initialisation Functions
        private void Start() => createIcons(objectTileset);
        #endregion

        #region Button Functions
        void ISelectionBar<TileIcon>.OnButtonSelected(TileIcon button)
        {
            // Invoke the event, logging an error if the tile is invalid.
            if (objectTileset.GetTileFromName(button.TileName) is ObjectTile objectTile) onSelected.Invoke(objectTile);
            else Debug.LogError($"Invalid object tile from tile with name: {button.TileName}", this);
        }
        #endregion
    }
}