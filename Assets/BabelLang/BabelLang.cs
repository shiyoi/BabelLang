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

        public LangCode CurLangCode
        {
            get;
            private set;
        }

        Hashtable langTable = new Hashtable();

        void Awake()
        {
            CurLangCode = GetSystemLang();
            LoadLang();
        }

        void LoadLang()
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

        public string GetText(string textID)
        {
            return (string)langTable[textID];
        }
    }
}