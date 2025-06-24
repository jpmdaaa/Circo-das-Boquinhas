/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using UnityEngine;

namespace TouchScript.Editor.Utils
{
    internal static class GUIElements
    {
        private static readonly GUIStyle headerStyle;

        static GUIElements()
        {
            BoxStyle = new GUIStyle(GUI.skin.box);
            BoxStyle.margin = new RectOffset(0, 0, 1, 0);
            BoxStyle.padding = new RectOffset(0, 0, 0, 0);
            BoxStyle.contentOffset = new Vector2(0, 0);
            BoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
            BoxStyle.alignment = TextAnchor.MiddleCenter;

            BoxLabelStyle = new GUIStyle(GUI.skin.label);
            BoxLabelStyle.fontSize = 9;
            BoxLabelStyle.padding = new RectOffset(0, 0, 5, 0);

            FoldoutStyle = new GUIStyle(GUI.skin.FindStyle("ShurikenModuleBg"));
            FoldoutStyle.padding = new RectOffset(10, 10, 10, 10);

            headerStyle = new GUIStyle(GUI.skin.FindStyle("ShurikenModuleTitle"));
            headerStyle.contentOffset = new Vector2(3, -2);
        }

        public static GUIStyle BoxStyle { get; }

        public static GUIStyle BoxLabelStyle { get; }

        public static GUIStyle FoldoutStyle { get; }

        public static GUIStyle HeaderStyle => FoldoutStyle;

        public static bool BeginFoldout(bool open, GUIContent header)
        {
            GUILayout.BeginVertical("ShurikenEffectBg", GUILayout.MinHeight(16f));

            return GUI.Toggle(GUILayoutUtility.GetRect(0, 16), open, header, headerStyle);
        }

        public static void EndFoldout()
        {
            GUILayout.EndVertical();
        }
    }
}