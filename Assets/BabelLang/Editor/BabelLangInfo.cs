/* Copyright(c) 2013 Matrix Bai @ Psychizen (bluegobin@gmail.com)*/

using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BabelLang
{
    public class BabelLangInfo : ScriptableObject
    {
        private static BabelLangInfo instance;
        public static BabelLangInfo Instance
        {
            get
            {
               if (instance == null)
               {
                    instance = Load();
               }
               return instance;
            }
        }

        public string[] TextIDs;

        public Hashtable langTable;
        public LangCode curLang = LangCode.EN;

        private const string BabelLangPath = "Assets/BabelLang/Editor/BabelLangInfo.asset";

        public static BabelLangInfo Load()
        {
            BabelLangInfo info = (BabelLangInfo)AssetDatabase.LoadAssetAtPath(BabelLangPath, typeof(BabelLangInfo));

            if (info == null)
            {
                info = (BabelLangInfo)ScriptableObject.CreateInstance(typeof(BabelLangInfo));
                info.langTable = LoadJson(info.curLang);

                AssetDatabase.CreateAsset(info, BabelLangPath);
            }

            return info;
        }

        public static Hashtable LoadJson(LangCode langCode)
        {
            TextAsset langFile = (TextAsset)Resources.Load("BabelLangs/" + langCode.ToString(), typeof(TextAsset));
            Debug.Log("TextAsset is " + langFile.text);
            return (Hashtable)MiniJSON.jsonDecode(langFile.text);
        }

        public string GetText(LangCode langCode, string textID)
        {
            if (langTable == null)
            {
                langTable = LoadJson(langCode);
            }

            return (string)langTable[textID];
        }
    }
}