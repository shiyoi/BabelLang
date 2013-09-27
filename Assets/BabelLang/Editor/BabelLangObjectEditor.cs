/* Copyright(c) 2013 Matrix Bai @ Psychizen */

using UnityEngine;
using UnityEditor;

using System;

namespace BabelLang
{
[CanEditMultipleObjects]
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
        bool isLocalizable = Target.isLocalizable;
        isLocalizable = EditorGUILayout.Toggle("Localizable", isLocalizable);
        if (isLocalizable != Target.isLocalizable)
        {
            Target.isLocalizable = isLocalizable;
        }

        if (isLocalizable)
        {
            BabelLangInfo langInfo = BabelEditor.langInfo;
            string[] allTextIDs = langInfo.TextIDs;

            IdIndex = Array.IndexOf(allTextIDs, Target.TextID);
            IdIndex = EditorGUILayout.Popup("TextID", IdIndex, langInfo.TextIDs);
            if (IdIndex != Array.IndexOf(allTextIDs, Target.TextID) && IdIndex >= 0)
            {
                Target.TextID = langInfo.TextIDs[IdIndex];
                Target.Text = langInfo.GetText(langInfo.curLang, Target.TextID);
            }
        }

        EditorUtility.SetDirty(Target);
        AssetDatabase.SaveAssets();
    }

}
}