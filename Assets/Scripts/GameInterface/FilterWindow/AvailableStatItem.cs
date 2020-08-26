using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    /// <summary> Controls the display of an available stat item. </summary>
    public class AvailableStatItem : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [Tooltip("The text box used to display the name of the stat.")]
        [SerializeField]
        private Text text = null;
        #endregion

        #region Properties
        /// <summary> The name of the stat that this item represents. </summary>
        public string StatName { get; private set; }
        #endregion

        #region Initialisation Functions
        /// <summary> Initialises this stat item using the given <paramref name="statName"/>. </summary>
        /// <param name="statName"> The name of the stat to display and use. </param>
        public void CreateFrom(string statName)
        {
            text.text = statName;
            StatName = statName;
        }
        #endregion
    }
}