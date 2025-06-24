using TouchScript.InputSources;
using UnityEditor;

namespace TouchScript.Editor.InputSources
{
#pragma warning disable 0618
    [CustomEditor(typeof(MouseInput), true)]
#pragma warning restore 0618
    internal sealed class MouseInputEditor : InputSourceEditor
    {
        private SerializedProperty disableOnMobilePlatforms;
        private SerializedProperty tags;

        protected override void OnEnable()
        {
            base.OnEnable();

            tags = serializedObject.FindProperty("Tags");
            disableOnMobilePlatforms = serializedObject.FindProperty("DisableOnMobilePlatforms");
        }

        public override void OnInspectorGUI()
        {
#if UNITY_5_6_OR_NEWER
            serializedObject.UpdateIfRequiredOrScript();
#else
			serializedObject.UpdateIfDirtyOrScript();
#endif

            EditorGUILayout.PropertyField(disableOnMobilePlatforms);

            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }

        protected override void drawAdvanced()
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(tags);
            EditorGUI.indentLevel--;
        }
    }
}