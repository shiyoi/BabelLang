/* Copyright(c) 2013 Matrix Bai @ Psychizen */
using UnityEngine;

using System.Collections;

namespace BabelLang
{
    public enum LangCode
    {
        NONE,
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

        public void Init(LangCode language)
        {
            if (language == LangCode.NONE)
            {
                CurLangCode = GetSystemLang(); 
            }
            else
            {
                CurLangCode = language;
            }

            LoadLang();
        }

        static void LoadLang()
        {
            TextAsset langFile = (TextAsset)Resources.Load("BabelLangs/" + CurLangCode.ToString(), typeof(TextAsset));
            langTable = (Hashtable)MiniJSON.jsonDecode(langFile.text);
        }

        LangCode GetSystemLang()
        {
            LangCode sysLang = LangCode.EN;
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Application.systemLanguage == SystemLanguage.English)
            {
                sysLang = LangCode.EN;
            }
            else if (Application.systemLanguage == SystemLanguage.Chinese)
            {
                sysLang = LangCode.ZH_CN;
            }
#elif UNITY_IPHONE
            string iosLanguage = PlayerPrefs.GetString("language");
            if (iosLanguage == "zh-Hant")
            {
                sysLang = LangCode.ZH_TW;
            }
            else if (iosLanguage == "zh-Hans")
            {
                sysLang = LangCode.ZH_CN;
            }

#elif UNITY_ANDROID
            AndroidJavaObject androidSysLang = new AndroidJavaObject("com.psychizen.systemlanguage.AndroidSysLang");
            androidSysLang.CallStatic("GetSysLanguage");
            string androidLanguage = androidSysLang.GetStatic<string>("langCode");
            Debug.Log("Android sys lang is " + androidLanguage);
            if (androidLanguage == "zh_CN" || androidLanguage == "zh")
            {
                sysLang = LangCode.ZH_CN;
            }
            else if (androidLanguage == "zh_TW")
            {
                sysLang = LangCode.ZH_TW;
            }      
#endif
            Debug.Log("Unity SystemLanguage is " + sysLang);
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

        /*void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 100, 100), "test"))
            {
                if (CurLangCode == LangCode.EN)
                {
                    SwitchLang(LangCode.ZH_CN);
                }
                else if (CurLangCode == LangCode.ZH_CN)
                {
                    SwitchLang(LangCode.ZH_TW);
                }
                else if (CurLangCode == LangCode.ZH_TW)
                {
                    SwitchLang(LangCode.EN);
                }
            }
        }*/
    }
}