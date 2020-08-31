using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary> Defines any human-specific behaviour for the player. </summary>
    public class HumanPlayer : BasePlayer
    {
        #region Inspector Fields
        [Tooltip("The human player's camera.")]
        [SerializeField]
        private Camera playerCamera = null;
        #endregion

        #region Properties
        public Camera PlayerCamera => playerCamera;
        #endregion
    }
}