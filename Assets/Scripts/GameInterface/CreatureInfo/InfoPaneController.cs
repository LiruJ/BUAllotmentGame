using Assets.Scripts.BUCore.Camera;
using Assets.Scripts.BUCore.UI;
using Assets.Scripts.Creatures;
using Assets.Scripts.Player.Tools;
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

        [SerializeField]
        private CreatureInspector creatureInspector = null;

        [Header("Elements")]
        [SerializeField]
        private RawImage creatureIcon = null;

        [SerializeField]
        private Text creatureNameLabel = null;

        [SerializeField]
        private ProgressBar healthBar = null;

        [SerializeField]
        private Text healthLabel = null;

        [SerializeField]
        private Text speedLabel = null;
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
                // Activate the info pane.
                gameObject.SetActive(true);

                // Begin tracking the creature with the icon camera.
                iconCamera.TrackedObject = creature.EyeOrigin;

                // Set the creature name label.
                creatureNameLabel.text = $"{creature.Seed.CropTileName} generation {creature.Seed.Generation}";

                // Set the max value of the health bar to the max health of the creature.
                healthBar.Max = creature.MaxHealth;
            }
            else gameObject.SetActive(false);
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            creatureIcon.gameObject.SetActive(iconCamera.HasTrackedObject);
        }

        private void FixedUpdate()
        {
            if (creatureInspector.HasSelectedCreature)
            {
                // Activate the info pane.
                gameObject.SetActive(true);

                // Update the health bar and label.
                healthBar.Progress = creatureInspector.SelectedCreature.Health;
                healthLabel.text = $"{creatureInspector.SelectedCreature.Health:N0}/{creatureInspector.SelectedCreature.MaxHealth:N0}HP";

                // Update the speedometer.
                speedLabel.text = $"{creatureInspector.SelectedCreature.Rigidbody.velocity.magnitude:N4}ms";
            }
            else gameObject.SetActive(false);
        }
        #endregion
    }
}