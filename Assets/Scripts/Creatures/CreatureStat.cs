﻿using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Creatures
{
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    /// <summary> Represents a single stat of a single aspect of a single creature. </summary>
    [Serializable]
    public class CreatureStat
    {
        #region Inspector Fields
        [Tooltip("The name of this stat.")]
        [SerializeField]
        private string statName = string.Empty;

        [Range(0, 1)]
        [Tooltip("The percentage chance for this stat to mutate.")]
        [SerializeField]
        private float mutationChance = 0.1f;

        [Range(0.01f, 10)]
        [Tooltip("The max/min amount added to or removed from the stat value when mutated.")]
        [SerializeField]
        private float mutationStrength = 0.1f;

        [SerializeField]
        private List<StatSideEffect> sideEffects = new List<StatSideEffect>();
        #endregion

        #region Fields
        /// <summary> The value of the stat. </summary>
        private float value;
        #endregion

        #region Properties
        /// <summary> The value of the stat. </summary>
        public float Value
        {
            get => value;
            private set
            {
                // Save the old value and set the new value.
                float oldValue = this.value;
                this.value = value;

                // If the value changed, invoke the event.
                if (oldValue != value)
                {
                    // Invoke the main event first.
                    onValueChanged.Invoke(this.value);

                    // Invoke all side effect events.
                    foreach (StatSideEffect sideEffect in sideEffects)
                        sideEffect.Invoke(this.value);
                }
            }
        }
        #endregion

        #region Events
        [Tooltip("Is fired when this stat's value is changed.")]
        [SerializeField]
        private FloatEvent onValueChanged = new FloatEvent();
        #endregion

        #region Stat Functions
        /// <summary> Adds this stat's name and value into the given <paramref name="seed"/>'s genetic stats. </summary>
        /// <param name="seed"> The seed into which the stat is added. </param>
        public void PopulateSeed(Seed seed) => seed.GeneticStats.Add(statName, Value);

        /// <summary> Finds and takes the stat from the given <paramref name="stats"/> and sets this stat's value to it. </summary>
        /// <param name="stats"> The collection of stats. </param>
        public void InitialiseFromStats(IReadOnlyDictionary<string, float> stats)
        {
            // Make a new float value to minimise event invokations.
            float newValue;

            // If the stat exists, copy it over, otherwise; initialise it to a random value.
            if (stats.TryGetValue(statName, out float stat))
            {
                // Set the stat value.
                newValue = stat;

                // Roll for mutation.
                if (UnityEngine.Random.value >= mutationChance) newValue += UnityEngine.Random.Range(-mutationStrength, mutationStrength);
            }
            else newValue = UnityEngine.Random.Range(-mutationStrength, mutationStrength);

            // Set the main stat value, invoking the event.
            Value = newValue;
        }
        #endregion
    }
}
