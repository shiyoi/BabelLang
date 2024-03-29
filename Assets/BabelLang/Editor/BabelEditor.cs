/* Copyright(c) 2013 Matrix Bai @ Psychizen */
using UnityEngine;
using UnityEditor;

using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace BabelLang
{
public class BabelEditor : EditorWindow
{
    public const string URL = "https://docs.google.com/spreadsheet/pub?key=0AtM-Mwy5McQ2dFJlcHVzeDlBeDNBOUh6MGJXbGJLa2c&output=csv";

    public static BabelLangInfo langInfo = BabelLangInfo.Instance;

/*#region Setting font and material
    [MenuItem("Psychizen/BabelLang/Setting")]
    static void SettingFontMaterial()
    {
        EditorWindow.GetWindow(typeof(BabelEditor));
    }

    void OnGUI()
    {
        langInfo.langFonts[0] = (Font)EditorGUILayout.ObjectField(langInfo.langFonts[0], typeof(Font), false);
    }
#endregion*/

#region Update from Google Drive
    [MenuItem("Psychizen/BabelLang/Update")]
    static void UpdateLangFiles()
    {
        string langData = DownloadData(URL);
        Debug.Log("Downloaded data is " + langData);
        string[] lines = langData.Split(new char[] {'\n'});

        ParseGoogleDriveData(lines);
    }

    static string DownloadData(string url)
    {
        WWW downloadJob = new WWW(url);

        while (!downloadJob.isDone)
        {

        }

        Debug.Log("Download data is " + downloadJob.text);

        return downloadJob.text;
    }

    static void ParseGoogleDriveData(string[] lines)
    {
        Hashtable langTable = new Hashtable();
        Hashtable entryTable = new Hashtable();
        for (int lineIndex = 0, lineNum = lines.Length; lineIndex < lineNum; lineIndex++)
        {
            string line = lines[lineIndex];
            string[] columns = line.Split(new char[] {','});
            //First row is for lang info
            if (lineIndex ==  0)
            {
                for (int columnIndex = 1; columnIndex < columns.Length; columnIndex++)
                {
                    langTable[columnIndex] = columns[columnIndex];
                    entryTable[columnIndex] = new Hashtable();
                }
            }
            else
            {
                string entryID = columns[0];
                for (int columnIndex = 1; columnIndex < columns.Length; columnIndex++)
                {
                    Hashtable table = (Hashtable)entryTable[columnIndex];
                    Debug.Log("columnIndex " + columnIndex + " table " + table + " columns " + columns);
                    table[entryID] = columns[columnIndex];
                    Debug.Log("column item is " + columns[columnIndex]);
                }
            }
        }

        //Save Json files and lang infos
        string directory = GetLangDir();
        langInfo.langTable = new Hashtable();
        foreach (DictionaryEntry entry in langTable)
        {
            SaveFile(directory + "/" + (string)entry.Value + ".txt", ((Hashtable)entryTable[(int)entry.Key]).toJson());
            langInfo.langTable[(string)entry.Value] = (Hashtable)entryTable[(int)entry.Key];
        }

        //Save TextID for BabelLangObjectEditor
        foreach (DictionaryEntry entry in langTable)
        {
            langInfo.TextIDs = new string[((Hashtable)entryTable[(int)entry.Key]).Count];
            ((Hashtable)entryTable[(int)entry.Key]).Keys.CopyTo(langInfo.TextIDs, 0);
            Array.Sort(langInfo.TextIDs);
            break;
        }

        BabelLangInfo.LoadJson(langInfo.curLang);

        EditorUtility.SetDirty(langInfo);
        AssetDatabase.SaveAssets();
    }

    static string GetLangDir()
    {
        string[] subdirEntries = Directory.GetDirectories(Application.dataPath, "Resources", SearchOption.AllDirectories);

        for (int index = 0; index < subdirEntries.Length; index++)
        {
            if (subdirEntries[index].Contains("BabelLangs"))
            {
                return subdirEntries[index];
            }
        }

        return Application.dataPath + "/BabelLang/Resources/BabelLangs";
    }

    static void SaveFile(string fileName, string data)
    {
        Debug.Log("Save data " + data + " to " + fileName);
        if (File.Exists(fileName))
            File.Delete(fileName);
        try
        {
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Unicode);
            sw.Write(data);
            sw.Close();
        }
        catch (System.IO.IOException IOEx)
        {
            Debug.LogError("Can't save file, " + IOEx);
        }

        AssetDatabase.ImportAsset(fileName, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
        AssetDatabase.Refresh();
    }
#endregion

#region Switch language
    [MenuItem("Psychizen/BabelLang/Switch Language/EN")]
    static void SwitchToEN()
    {
        langInfo.curLang = LangCode.EN;
        UpdateScene();
    }

    [MenuItem("Psychizen/BabelLang/Switch Language/ZH_CN")]
    static void SwitchToZHCN()
    {
        langInfo.curLang = LangCode.ZH_CN;
        UpdateScene();
    }

    [MenuItem("Psychizen/BabelLang/Switch Language/ZH_TW")]
    static void SwitchToZHTW()
    {
        langInfo.curLang = LangCode.ZH_TW;
        UpdateScene();
    }

    static void UpdateScene()
    {
        langInfo.langTable = BabelLangInfo.LoadJson(langInfo.curLang);
        BabelLangObject[] babelObjs = GameObject.FindObjectsOfType(typeof(BabelLangObject)) as BabelLangObject[];
        Debug.Log("Update scene " + babelObjs.Length);
        for (int index = 0, n = babelObjs.Length; index < n; index++)
        {
            if (babelObjs[index].isLocalizable)
            {
                babelObjs[index].Text = langInfo.GetText(langInfo.curLang, babelObjs[index].TextID);
            }
        }
    }
#endregion
}

}