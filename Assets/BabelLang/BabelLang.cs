/* Copyright(c) 2013 Matrix Bai @ Psychizen */
using UnityEngine;

using System.Collections;

namespace BabelLang
{
    public enum LangCode
    {
        EN,
        ZH_CN,
        ZH_TW
    }

    public class BabelLang : MonoBehaviour
    {
        private static BabelLang instance;
        public static BabelLang Instance
        {
            get
            {
               if (instance == null)
               {
                  instance = FindObjectOfType( typeof( BabelLang ) ) as BabelLang;
               }
               return instance;
            }
        }

        public static LangCode CurLangCode
        {
            get;
            private set;
        }

        static Hashtable langTable = new Hashtable();

        void Awake()
        {
            CurLangCode = GetSystemLang();
            LoadLang();
        }

        static void LoadLang()
        {
            TextAsset langFile = (TextAsset)Resources.Load("BabelLangs/" + CurLangCode.ToString(), typeof(TextAsset));
            Debug.Log("TextAsset is " + langFile.text);
            langTable = (Hashtable)MiniJSON.jsonDecode(langFile.text);
        }

        LangCode GetSystemLang()
        {
            LangCode sysLang = LangCode.EN;
            if (Application.systemLanguage == SystemLanguage.English)
            {
                sysLang = LangCode.EN;
            }
            else if (Application.systemLanguage == SystemLanguage.Chinese)
            {
                sysLang = LangCode.ZH_CN;
            }

            Debug.Log("SystemLanguage is " + sysLang);
            return sysLang;
        }

        public void SwitchLang(LangCode newLang)
        {
            if (newLang != CurLangCode)
            {
                CurLangCode = newLang;

                LoadLang();

                BabelLangObject[] babelObjs = GameObject.FindObjectsOfType(typeof(BabelLangObject)) as BabelLangObject[];
                for (int index = 0, n = babelObjs.Length; index < n; index++)
                {
                    babelObjs[index].UpdateLang();
                }
            }
        }

        public static string GetText(string textID)
        {
            if (langTable == null)
            {
                LoadLang();
            }

            return (string)langTable[textID];
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 100, 100), "test"))
            {
                SwitchLang(LangCode.ZH_CN);
            }
        }
    }
}