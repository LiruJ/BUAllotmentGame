using Assets.Scripts.Objects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Tiles
{
    public class ObjectTileBar : TileBar, ISelectionBar<TileIcon>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private ObjectTileset objectTileset = null;
        #endregion

        #region Events
        [Serializable]
        private class tileEvent : UnityEvent<ObjectTile> { }

        [SerializeField]
        private tileEvent onSelected = new tileEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            createIcons(objectTileset);
        }
        #endregion

        #region Button Functions
        void ISelectionBar<TileIcon>.OnButtonSelected(TileIcon button)
        {
            // Get the object tile with the tile name of the clicked button.
            ObjectTile objectTile = objectTileset.GetTileFromName(button.TileName) as ObjectTile;

            // Invoke the event.
            onSelected.Invoke(objectTile);
        }
        #endregion
    }
}