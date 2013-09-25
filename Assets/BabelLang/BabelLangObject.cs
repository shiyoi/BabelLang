/* Copyright(c) 2013 Matrix Bai @ Psychizen */
using UnityEngine;
using System.Collections;

namespace BabelLang
{
    public class BabelLangObject : MonoBehaviour
    {
        private TextMesh textMesh;
        public string Text
        {
            get
            {
                if (textMesh == null)
                {
                    textMesh = GetComponent<TextMesh>();
                }
                return textMesh.text;
            }
            set
            {
                if (textMesh == null)
                {
                    textMesh = GetComponent<TextMesh>();
                }
                textMesh.text = value;
            }
        }

        public string textID;
        public string TextID
        {
            get
            {
                return textID;
            }
            set
            {
                textID = value;
                Text = BabelLang.Instance.GetText(TextID);
            }
        }

        void OnEnable()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMesh>();
            }

            UpdateLang();
        }

        public void UpdateLang()
        {
            Text = BabelLang.Instance.GetText(TextID);
        }
    }
}