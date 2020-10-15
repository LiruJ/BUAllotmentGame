using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BUCore.Time
{
    public class TimeManager : MonoBehaviour
    {
        #region Properties
        public float TimeScale { get => UnityEngine.Time.timeScale; set => UnityEngine.Time.timeScale = value; }
        #endregion

        #region Time Functions
        public void TogglePause() => TimeScale = TimeScale == 0 ? 1 : 0;
        #endregion
    }
}