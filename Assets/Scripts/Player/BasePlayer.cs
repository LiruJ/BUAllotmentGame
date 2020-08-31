using Assets.Scripts.Creatures;
using Assets.Scripts.Map;
using Assets.Scripts.Seeds;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary> Defines basic references to a player's objects. </summary>
    public abstract class BasePlayer : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The player's world map.")]
        [SerializeField]
        private WorldMap worldMap = null;

        [Tooltip("The player's creatures.")]
        [SerializeField]
        private CreatureManager creatureManager = null;

        [Tooltip("The player's seeds.")]
        [SerializeField]
        private SeedManager seedManager = null;
        #endregion

        #region Properties
        public WorldMap WorldMap => worldMap;

        public CreatureManager CreatureManager => creatureManager;

        public SeedManager SeedManager => seedManager;
        #endregion
    }
}