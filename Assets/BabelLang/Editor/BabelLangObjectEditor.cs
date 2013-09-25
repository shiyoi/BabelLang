/* Copyright(c) 2013 Matrix Bai @ Psychizen */

using UnityEngine;
using UnityEditor;

using System;

namespace BabelLang
{
[CustomEditor(typeof(BabelLangObject))]
public class BabelLangObjectEditor : Editor
{
    public int IdIndex;

    public BabelLangObject Target
    {
        get
        {
            return (BabelLangObject)target;
        }
    }

    public override void OnInspectorGUI()
    {
        BabelLangInfo langInfo = BabelEditor.langInfo;
        string[] allTextIDs = langInfo.TextIDs;

        IdIndex = Array.IndexOf(allTextIDs, Target.TextID);
        IdIndex = EditorGUILayout.Popup("TextID", IdIndex, langInfo.TextIDs);
        if (IdIndex != Array.IndexOf(allTextIDs, Target.TextID))
        {
            Target.TextID = langInfo.TextIDs[IdIndex];
            Target.Text = langInfo.GetText(langInfo.curLang, Target.TextID);
        }
    }

    public void UpdateLangObject()
    {
        Target.Text = BabelEditor.langInfo.GetText(BabelEditor.langInfo.curLang, Target.TextID);
    }

}
}