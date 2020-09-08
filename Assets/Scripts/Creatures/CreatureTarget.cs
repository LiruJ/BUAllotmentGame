using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public struct CreatureTarget
    {
        #region Properties
        public Creature Target { get; set; }

        public Transform Transform => Target.transform;

        public Rigidbody TargetRigidbody { get; set; }

        public Collider TargetCollider { get; set; }

        public float NormalisedDistance { get; set; }

        public float NormalisedHealth { get; set; }


        #endregion
    }
}
