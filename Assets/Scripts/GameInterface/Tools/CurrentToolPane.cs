using Assets.Scripts.Player.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Tools
{
    public class CurrentToolPane : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [Tooltip("The icon used to represent the current tool.")]
        [SerializeField]
        private Image toolImage = null;

        [Tooltip("The text displaying the name of the current tool.")]
        [SerializeField]
        private TextMeshProUGUI currentToolLabel = null;
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {

        }
        #endregion

        #region Tool Functions
        public void OnToolChanged(Tool currentTool)
        {
            if (currentTool != null)
            {
                toolImage.sprite = currentTool.Icon;
                currentToolLabel.text = $"Tool: {currentTool.ToolName}";
            }
            else
            {
                toolImage.sprite = null;
                currentToolLabel.text = string.Empty;
            }

            gameObject.SetActive(currentTool != null);
        }
        #endregion
    }
}