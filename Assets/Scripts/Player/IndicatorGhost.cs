using UnityEngine;

namespace Assets.Scripts.Player
{
    public class IndicatorGhost : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Materials")]
        [Tooltip("The material to use to signify an invalid action.")]
        [SerializeField]
        private Material invalidMaterial = null;

        [Tooltip("The material to use to signify a valid action.")]
        [SerializeField]
        private Material validMaterial = null;
        #endregion

        #region Fields
        /// <summary> The attached <see cref="MeshRenderer"/> component. </summary>
        private MeshRenderer meshRenderer = null;
        #endregion

        #region Properties
        /// <summary> The material to use to signify an invalid action. </summary>
        public Material InvalidMaterial { get => invalidMaterial; set => invalidMaterial = value; }

        /// <summary> The material to use to signify a valid action. </summary>
        public Material ValidMaterial { get => validMaterial; set => validMaterial = value; }
        #endregion

        #region Initialisation Functions
        private void Start() => meshRenderer = GetComponent<MeshRenderer>();
        #endregion

        #region Ghost Functions
        /// <summary> Change the material of this ghost object based on the given <paramref name="canPlace"/>, if it is true, the material is valid, invalid otherwise. </summary>
        /// <param name="canPlace"> If the action is valid or not. </param>
        public void Change(bool canPlace) => changeMaterials(canPlace ? ValidMaterial : InvalidMaterial);

        public void ChangeToValid() => changeMaterials(ValidMaterial);

        public void ChangeToInvalid() => changeMaterials(InvalidMaterial);

        private void changeMaterials(Material material)
        {
            // If the material doesn't need to change, don't do anything.
            if (meshRenderer == null || meshRenderer.material == material) return;

            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < meshRenderer.materials.Length; i++)
                materials[i] = material;

            meshRenderer.materials = materials;
        }
        #endregion
    }
}