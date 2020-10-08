using Assets.Scripts.BUCore.Camera;
using Assets.Scripts.BUCore.UI;
using Assets.Scripts.Creatures;
using Assets.Scripts.Player.Tools;
using TMPro;
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
        private TextMeshProUGUI creatureNameLabel = null;

        [SerializeField]
        private ProgressBar healthBar = null;

        [SerializeField]
        private TextMeshProUGUI healthLabel = null;

        [SerializeField]
        private TextMeshProUGUI speedLabel = null;

        [SerializeField]
        private TextMeshProUGUI aliveTimeLabel = null;
        #endregion

        #region Fields
        private BehaviourDisplay[] behaviourDisplays = null;
        #endregion

        #region Properties
        public CreatureInspector CreatureInspector => creatureInspector;
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            behaviourDisplays = GetComponentsInChildren<BehaviourDisplay>();
        }
        #endregion

        #region Display Functions
        public void DisplayCreature(Creature creature)
        {
            if (creature != null)
            {
                // Activate the info pane.
                gameObject.SetActive(true);

                // Go over each behaviour and display it.
                foreach (BehaviourDisplay behaviourDisplay in behaviourDisplays)
                    behaviourDisplay.DisplayCreature(creature);

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

            if (creatureInspector.HasSelectedCreature)
            {
                // Activate the info pane.
                gameObject.SetActive(true);

                // Update the health bar and label.
                healthBar.Progress = creatureInspector.SelectedCreature.Health;
                healthLabel.text = $"{creatureInspector.SelectedCreature.Health:N0}/{creatureInspector.SelectedCreature.MaxHealth:N0}HP";

                // Update the speedometer.
                speedLabel.text = $"{creatureInspector.SelectedCreature.Rigidbody.velocity.magnitude:N4}ms";

                // Update the alive time label.
                aliveTimeLabel.text = $"{creatureInspector.SelectedCreature.LifetimeStats["AliveTime"]:N4}s";
            }
            else gameObject.SetActive(false);
        }
        #endregion
    }
}