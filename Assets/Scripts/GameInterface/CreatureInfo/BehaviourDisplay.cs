using Assets.Scripts.Creatures;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameInterface.CreatureInfo
{
    public class BehaviourDisplay : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The main info pane controller.")]
        [SerializeField]
        private InfoPaneController infoPaneController = null;

        [Tooltip("The pane where the genetic stat displays are stored.")]
        [SerializeField]
        private RectTransform geneticStatsContainer = null;

        [Tooltip("The pane where the lifetime stat displays are stored.")]
        [SerializeField]
        private RectTransform lifetimeStatsContainer = null;

        [Header("Settings")]
        [Tooltip("The name of the behaviour to display, without the \"Behaviour\" suffix.")]
        [SerializeField]
        private string behaviourName = string.Empty;
        #endregion

        #region Fields
        private readonly Dictionary<string, IKeyedValueDisplay> geneticStatDisplaysByName = new Dictionary<string, IKeyedValueDisplay>();

        private readonly Dictionary<string, IKeyedValueDisplay> lifetimeStatDisplaysByName = new Dictionary<string, IKeyedValueDisplay>();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            foreach (IKeyedValueDisplay traitDisplay in geneticStatsContainer.GetComponentsInChildren<IKeyedValueDisplay>())
                geneticStatDisplaysByName.Add(traitDisplay.Key, traitDisplay);

            foreach (IKeyedValueDisplay traitDisplay in lifetimeStatsContainer.GetComponentsInChildren<IKeyedValueDisplay>())
                lifetimeStatDisplaysByName.Add(traitDisplay.Key, traitDisplay);
        }
        #endregion

        #region Display Functions
        public void DisplayCreature(Creature creature)
        {
            // If the creature is null, do nothing.
            if (creature == null) return;

            // Get the desired behaviour from the creature.
            if (creature.CreatureBehaviours.TryGetValue(behaviourName, out CreatureBehaviour creatureBehaviour))
            {
                // Show this display.
                gameObject.SetActive(true);

                // Go over each genetic stat within the behaviour. If the stat has a display associated with it, display the stat.
                foreach (CreatureStat creatureStat in creatureBehaviour.CreatureStats)
                    if (geneticStatDisplaysByName.TryGetValue(creatureStat.Name, out IKeyedValueDisplay traitDisplay)) traitDisplay.Value = creatureStat.Value;


            }
            // If the creature does not have this behaviour, hide this display.
            else gameObject.SetActive(false);
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            // If no creature is selected, do nothing.
            if (!infoPaneController.CreatureInspector.HasSelectedCreature) return;

            // Get the behaviour from the creature and update the lifetime stats.
            if (infoPaneController.CreatureInspector.SelectedCreature.CreatureBehaviours.TryGetValue(behaviourName, out CreatureBehaviour creatureBehaviour))
                foreach (KeyValuePair<string, float> statNameValue in creatureBehaviour.LifetimeStats)
                    if (lifetimeStatDisplaysByName.TryGetValue(statNameValue.Key, out IKeyedValueDisplay traitDisplay)) traitDisplay.Value = statNameValue.Value;
        }
        #endregion
    }
}