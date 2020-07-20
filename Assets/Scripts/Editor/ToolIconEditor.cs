using Assets.Scripts.UI.Tools;
using UnityEditor;
using UnityEditor.UI;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(ToolIcon))]
    [CanEditMultipleObjects]
    public class ToolIconEditor : ToggleEditor
    {
        #region Fields
        private SerializedProperty toolType = null;
        private SerializedProperty onSelectedEvent = null;
        private SerializedProperty onDeselectedEvent = null;
        #endregion

        #region Initialisation Functions
        protected override void OnEnable()
        {
            base.OnEnable();
            
            toolType = serializedObject.FindProperty("toolType");
            onSelectedEvent = serializedObject.FindProperty("onSelected");
            onDeselectedEvent = serializedObject.FindProperty("onDeselected");

        }
        #endregion

        #region Editor Functions
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            
            // Draw the toggle UI, then the on selected event.
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(toolType);
            EditorGUILayout.PropertyField(onSelectedEvent);
            EditorGUILayout.PropertyField(onDeselectedEvent);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}