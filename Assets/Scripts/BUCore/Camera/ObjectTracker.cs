using System;
using UnityEngine;

namespace Assets.Scripts.BUCore.Camera
{
    public class ObjectTracker : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("The object to track.")]
        [SerializeField]
        private Transform trackedObject = null;

        [Tooltip("The distance at which to track.")]
        [Range(0, 100)]
        [SerializeField]
        private float trackDistance = 0;
        #endregion

        #region Fields

        #endregion

        #region Properties
        public Transform TrackedObject { get => trackedObject; set => trackedObject = value; }

        public bool HasTrackedObject => TrackedObject != null;
        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {

        }
        #endregion

        #region Update Functions
        private void LateUpdate()
        {
            if (trackedObject != null)
            {
                transform.position = trackedObject.position + trackedObject.forward * trackDistance;
                transform.LookAt(trackedObject);
            }
        }

        private void FixedUpdate()
        {

        }
        #endregion
    }
}