using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Creatures
{
    [Serializable]
    public class StatSideEffect
    {
        #region Fields
        [Tooltip("Is not used for anything other than naming the list item in the inspector.")]
        [SerializeField]
        private string name = string.Empty;

        [Tooltip("For every 1 unit of this stat, how many units of the property should be set?")]
        [Range(-100, 100)]
        [SerializeField]
        private float multiplier = 1.0f;

        [Tooltip("The minimum value that is allowed.")]
        [SerializeField]
        private float minimum = 0;
        #endregion

        #region Events
        [Tooltip("Is called when the attached stat's value changes, with the value being calculated from this side-effect.")]
        [SerializeField]
        private FloatEvent onValueChanged = new FloatEvent();

        [Serializable]
        private class vector3Event : UnityEvent<Vector3> { }

        [Tooltip("Is called when the attached stat's value changes, with the value being calculated from this side-effect as a vector3 with each value being the calculated value.")]
        [SerializeField]
        private vector3Event onValueChangedVector = new vector3Event();
        #endregion

        #region Event Functions
        public void Invoke(float statValue)
        {
            onValueChanged.Invoke(Mathf.Max(statValue * multiplier, minimum));
            onValueChangedVector.Invoke(Vector3.one * Mathf.Max(statValue * multiplier, minimum));
        }
        #endregion
    }
}
