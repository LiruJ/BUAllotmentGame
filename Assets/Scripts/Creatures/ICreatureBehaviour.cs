using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public interface ICreatureBehaviour
    {
        GameObject gameObject { get; }

        void InitialiseFromStats(Creature creature, IReadOnlyDictionary<string, float> stats);
        void PopulateSeed(Seed seed);
    }
}
