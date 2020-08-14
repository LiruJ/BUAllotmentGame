using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Seeds
{
    public class SeedDetails : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [SerializeField]
        private Text seedName = null;

        [SerializeField]
        private Image seedIcon = null;

        [SerializeField]
        private Button button = null;
        #endregion

        #region Properties
        public Button Button => button;
        #endregion

        #region Initialisation Functions
        public void InitialiseFromSeed(CropTileset cropTileset, Seed seed)
        {
            seedName.text = seed.CropTileName;
            seedIcon.sprite = (cropTileset.GetTileFromName(seed.CropTileName) as CropTile).Icon;
        }
        #endregion
    }
}