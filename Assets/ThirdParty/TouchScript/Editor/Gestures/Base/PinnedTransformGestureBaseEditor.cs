/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using TouchScript.Gestures.Base;
using UnityEditor;
using UnityEngine;

namespace TouchScript.Editor.Gestures.Base
{
    internal class PinnedTransformGestureBaseEditor : GestureEditor
    {
        public static readonly GUIContent TYPE = new("Transform Type",
            "Specifies what gestures should be detected: Rotation, Scaling.");

        public static readonly GUIContent TYPE_ROTATION = new("Rotation", "Rotating with two or more fingers.");
        public static readonly GUIContent TYPE_SCALING = new("Scaling", "Scaling with two or more fingers.");

        public static readonly GUIContent SCREEN_TRANSFORM_THRESHOLD = new("Movement Threshold (cm)",
            "Minimum distance in cm touch points must move for the gesture to begin.");

        protected SerializedProperty screenTransformThreshold;

        protected SerializedProperty type;

        protected override void OnEnable()
        {
            base.OnEnable();

            type = serializedObject.FindProperty("type");
            screenTransformThreshold = serializedObject.FindProperty("screenTransformThreshold");
        }

        public override void OnInspectorGUI()
        {
#if UNITY_5_6_OR_NEWER
            serializedObject.UpdateIfRequiredOrScript();
#else
            serializedObject.UpdateIfDirtyOrScript();
#endif

            var typeValue = type.intValue;
            var newType = 0;
            EditorGUILayout.LabelField(TYPE);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.ToggleLeft(TYPE_ROTATION,
                    (typeValue & (int)TransformGestureBase.TransformType.Rotation) != 0, GUILayout.Width(80)))
                newType |= (int)TransformGestureBase.TransformType.Rotation;
            EditorGUI.indentLevel--;
            if (EditorGUILayout.ToggleLeft(TYPE_SCALING,
                    (typeValue & (int)TransformGestureBase.TransformType.Scaling) != 0, GUILayout.Width(70)))
                newType |= (int)TransformGestureBase.TransformType.Scaling;
            type.intValue = newType;
            EditorGUILayout.EndHorizontal();

            doInspectorGUI();

            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }

        protected virtual void doInspectorGUI()
        {
        }

        protected override void drawAdvanced()
        {
            EditorGUIUtility.labelWidth = 160;
            EditorGUILayout.PropertyField(screenTransformThreshold, SCREEN_TRANSFORM_THRESHOLD);

            base.drawAdvanced();
        }
    }
}