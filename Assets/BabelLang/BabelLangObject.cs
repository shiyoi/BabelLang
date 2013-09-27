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

        public Font DynamicFont
        {
            set
            {
                textMesh.font = value;
            }
        }

        public Material FontMat
        {
            set
            {
                renderer.material = value;
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
                Text = BabelLang.GetText(TextID);
            }
        }

        public bool isLocalizable = true;

        void OnEnable()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMesh>();
            }

            UpdateLang();
        }

        public void UpdateLang(Font font, Material mat, string text)
        {
            DynamicFont = font;
            FontMat = mat;
            Text = text;
        }

        public void UpdateLang()
        {
            if (isLocalizable)
            {
                Text = BabelLang.GetText(TextID);
            }
        }
    }
}