using UnityEngine;

namespace Assets.Scripts.BUCore.UI
{
    /// <summary> Allows a UI element to be collapsed. </summary>
    public class CollapsibleBar : MonoBehaviour
    {
        #region Types
        public enum CollapseDirection { Left, Up, Right, Down }
        #endregion

        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The transform used for its size in order to position the entire object when collapsed.")]
        [SerializeField]
        private RectTransform sizeTransform = null;

        [Header("Settings")]
        [Tooltip("If this object should collapse upon being loaded.")]
        [SerializeField]
        private bool startCollapsed = false;

        [Tooltip("Which direction to collapse into.")]
        [SerializeField]
        public CollapseDirection collapseDirection = CollapseDirection.Down;
        #endregion

        #region Fields
        private RectTransform rectTransform = null;
        #endregion

        #region Properties
        public bool IsCollapsed { get; private set; }
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Get this transform as a rect transform and save it.
            rectTransform = transform as RectTransform;

            // If this object should start collapsed, collapse.
            if (startCollapsed) Collapse();
        }
        #endregion

        #region Collapse Functions
        public void ToggleCollapsed()
        {
            // Collapse if not already, otherwise return.
            if (IsCollapsed) Return();
            else Collapse();
        }

        public void Collapse()
        {
            // If this object is already collapsed, do nothing.
            if (IsCollapsed) return;
            IsCollapsed = true;

            // Calculate the positional change, then apply it to the offsets.
            Vector2 positionalChange = calculatePositionalChange();
            rectTransform.offsetMax += positionalChange;
            rectTransform.offsetMin += positionalChange;
        }

        public void Return()
        {
            // If this object is already not collapsed, do nothing.
            if (!IsCollapsed) return;
            IsCollapsed = false;

            // Calculate the positional change, then apply it to the offsets.
            Vector2 positionalChange = calculatePositionalChange();
            rectTransform.offsetMax -= positionalChange;
            rectTransform.offsetMin -= positionalChange;
        }

        private Vector2 calculatePositionalChange()
        {
            // Calculate and return the change based on the direction.
            switch (collapseDirection)
            {
                case CollapseDirection.Left: return new Vector2(-sizeTransform.rect.width, 0);
                case CollapseDirection.Up: return new Vector2(0, sizeTransform.rect.height);
                case CollapseDirection.Right: return new Vector2(sizeTransform.rect.width, 0);
                case CollapseDirection.Down: return new Vector2(0, -sizeTransform.rect.height);
                default:
                    Debug.LogError("Invalid direction.", this);
                    return Vector2.zero;
            }
        }
        #endregion
    }
}