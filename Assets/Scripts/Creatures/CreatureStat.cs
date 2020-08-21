using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Creatures
{
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [Serializable]
    public class CreatureStat
    {
        #region Inspector Fields
        [SerializeField]
        private string statName = string.Empty;

        [Range(0, 1)]
        [SerializeField]
        private float mutationChance = 0.1f;

        [Range(0.01f, 10)]
        [SerializeField]
        private float mutationStrength = 0.1f;
        #endregion

        #region Fields

        #endregion

        #region Properties
        public float Value
        {
            get => value;
            private set
            {
                float oldValue = this.value;
                this.value = value;

                if (oldValue != value) onValueChanged.Invoke(this.value);
            }
        }
        #endregion

        #region Events
        [SerializeField]
        private FloatEvent onValueChanged = new FloatEvent();
        private float value;
        #endregion

        #region Stat Functions
        public void PopulateSeed(Seed seed) => seed.GeneticStats.Add(statName, Value);

        public void InitialiseFromStats(IReadOnlyDictionary<string, float> stats)
        {
            float newValue;

            // If the stat exists, copy it over, otherwise; initialise it to a random value.
            if (stats.TryGetValue(statName, out float stat))
            {
                newValue = stat;

                // Roll for mutation.
                if (UnityEngine.Random.value >= mutationChance) newValue += UnityEngine.Random.Range(-mutationStrength, mutationStrength);
            }
            else newValue = UnityEngine.Random.Range(-mutationStrength, mutationStrength);

            Value = newValue;
        }
        #endregion
    }
}
