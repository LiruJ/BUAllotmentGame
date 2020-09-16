using Assets.Scripts.Creatures;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public struct ProjectileHitInfo
    {
        #region Properties
        public float Speed { get; set; }

        public Vector3 OriginPosition { get; set; }

        public Vector3 TargetPosition { get; set; }

        public Vector3 HitPosition { get; set; }

        public Creature TargetCreature { get; set; }

        public Creature HitCreature { get; set; }

        public Collider HitCollder { get; set; }
        #endregion
    }
}
