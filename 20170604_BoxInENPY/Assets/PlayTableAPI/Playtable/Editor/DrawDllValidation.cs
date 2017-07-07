using UnityEngine;
using UnityEditor;
using System;

namespace Playmove
{
    [CustomEditor(typeof(PlaytableWin32))]
    public class DrawDllValidation : Editor
    {
        static void OnSceneGUI()
        {
            if (!Playtable.Authentication.TrueValidation) return;

            Handles.BeginGUI();

            var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.fontSize = 20;
            GUILayout.Label(Playtable.Authentication.Message, centeredStyle);

            Handles.EndGUI();
        }
    }
}