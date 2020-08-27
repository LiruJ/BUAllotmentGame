using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    /// <summary> Exposes the basic behaviour functionality. </summary>
    public interface ICreatureBehaviour
    {
        /// <summary> The creature's <see cref="GameObject"/>. </summary>
        GameObject gameObject { get; }

        /// <summary> Sets the stats of this behaviour from the given <paramref name="stats"/> dictionary. </summary>
        /// <param name="creature"> The base creature. </param>
        /// <param name="stats"> The stats of the creature. </param>
        void InitialiseFromStats(Creature creature, IReadOnlyDictionary<string, float> stats);

        /// <summary> Adds each stat to the given <paramref name="seed"/>. </summary>
        /// <param name="seed"> The seet to fill with stats. </param>
        void PopulateSeed(Seed seed);
    }
}
