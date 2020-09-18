using Assets.Scripts.BUCore.Camera;
using Assets.Scripts.Creatures;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.CreatureInfo
{
    public class InfoPaneController : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private ObjectTracker iconCamera = null;

        [Header("Elements")]
        [SerializeField]
        private RawImage creatureIcon = null;

        [SerializeField]
        private Text creatureNameLabel = null;
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

        #region Display Functions
        public void DisplayCreature(Creature creature)
        {
            if (creature != null)
            {
                iconCamera.TrackedObject = creature.EyeOrigin;
            }
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            creatureIcon.gameObject.SetActive(iconCamera.HasTrackedObject);
        }

        private void FixedUpdate()
        {

        }
        #endregion
    }
}