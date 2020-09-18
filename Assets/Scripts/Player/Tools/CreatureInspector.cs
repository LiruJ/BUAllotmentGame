using Assets.Scripts.Creatures;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Tools
{
    public class CreatureInspector : Tool
    {
        #region Inspector Fields

        #endregion

        #region Fields
        private Creature selectedCreature = null;
        #endregion

        #region Properties
        public Creature SelectedCreature
        {
            get => selectedCreature;
            private set
            {
                selectedCreature = value;
                onCreatureSelected.Invoke(selectedCreature);
            }
        }

        public bool HasSelectedCreature => SelectedCreature != null && SelectedCreature.gameObject != null;
        #endregion

        #region Events
        [Serializable]
        private class creatureEvent : UnityEvent<Creature> { }

        [SerializeField]
        private creatureEvent onCreatureSelected = new creatureEvent();
        #endregion

        #region Initialisation Functions

        #endregion

        #region Update Functions
        public override void HandleInput()
        {
            // If the UI is not being moused over, take input.
            if (!eventSystem.IsPointerOverGameObject())
            {
                // If the player left clicks, change the currently selected creature.
                if (Input.GetMouseButtonDown(0))
                {
                    // If a creature was under the mouse, select it.
                    if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 200f, LayerMask.GetMask("Creatures")) && hitInfo.collider.gameObject.TryGetComponent(out Creature creature))
                        SelectedCreature = creature;
                    // Otherwise; deselect the current creature.
                    else SelectedCreature = null;
                }

            }
        }
        #endregion
    }
}