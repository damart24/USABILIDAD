using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(JapaneseTextAttributes))]
public class JapaneseText : TextMeshProUGUI
{
    private int valueToRotate = -90;
    private int valueToRotateNotJapanese = 0;
    private string rotateText = "<rotate=90>";

    bool changedTextProcessed = false;

    private Dictionary<KeyValuePair<int, int>, string> kanjiAndReadings;

    private Dictionary<KeyValuePair<int, int>, JapaneseText> readingsObjects;

    protected override void Awake()
    {
        base.Awake();
        kanjiAndReadings = new();
        readingsObjects = new();
        //Registramos un callback cada vez que el texto cambie
        RegisterDirtyLayoutCallback(OnTextChanged);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnregisterDirtyLayoutCallback(OnTextChanged);
    }

    private void OnTextChanged()
    {
        //No queremos que nuestro propio codigo se llame a si mismo, tenemos este bool
        if (changedTextProcessed) { return; }
        changedTextProcessed = true;

        //Reseteamos las explicaciones
        kanjiAndReadings.Clear();
        foreach (var explanation in readingsObjects)
        {
            Destroy(explanation.Value.gameObject);
        }
        readingsObjects.Clear();

        JapaneseTextAttributes atributes = GetComponent<JapaneseTextAttributes>();
        fontSize = atributes.japaneseSize;
        font = atributes.japaneseFont;
        if (L10nManager.Instance && L10nManager.Instance.GetLanguage() == Language.Japanese)
        {

            if (atributes.vertical)
            {
                m_text = rotateText + m_text;
                rectTransform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, valueToRotate);
            }
            characterSpacing = atributes.characterSpacingValue;
            characterSpacing *= atributes.japaneseAugmentValue;

            //Si estando en japones tiene kanjis utilizando el formato que se usa...
            if (TextHasKanji(m_text))
            {
                //Se limpia el texto y reiniciamos los textos con las explicaciones
                CleanTextAndInitializeReadings();
                lineSpacing = atributes.lineSpacingValue;

                //Actualizamos la posición de todos los textos con las explicaciones para que estén encima de su(s) kanji
                if (kanjiAndReadings != null)
                {
                    foreach (var kan in readingsObjects)
                        UpdateReadingsText(kan.Key);
                }
            }
        }
        else
        {
            m_text = base.m_text.Replace(rotateText, "");
            rectTransform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, valueToRotateNotJapanese);
            characterSpacing = atributes.characterSpacingValue;
            characterSpacing /= atributes.japaneseAugmentValue;
            fontSize /= atributes.japaneseAugmentValue;
        }

        changedTextProcessed = false;
    }

    /// <summary>
    /// Comprobamos si el texto tiene los caracteres de nuestro convenio para dibujar los kanji con descripciones
    /// </summary>
    private bool TextHasKanji(string t)
    {
        if (t == null) return false;
        int colonsCount = t.Length - t.Replace(":", "").Length;
        int openSquareBracketCount = t.Length - t.Replace("[", "").Length;
        int closeSquareBracketCount = t.Length - t.Replace("]", "").Length;

        return colonsCount > 0 && openSquareBracketCount > 0 && closeSquareBracketCount > 0;
    }

    private void CleanTextAndInitializeReadings()
    {
        string cleanText = m_text;
        //Mientras el texto tenga cosas de nuestros kanji...
        while (TextHasKanji(cleanText))
        {
            int indexOfColon = cleanText.IndexOf(':');
            int indexOfopenSquareBracket = cleanText.IndexOf('[');
            int indexOfcloseSquareBracket = cleanText.IndexOf(']');

            //Si hay un ':' fuera de los brackets, lo quitamos y continuamos, se que no es lo mejor pero prefiero que no pete :vv
            if (indexOfopenSquareBracket > indexOfColon || indexOfcloseSquareBracket < indexOfColon)
            {

                cleanText = cleanText.Remove(indexOfColon, 1);
                continue;
            }

            string kanji = cleanText.Substring(indexOfopenSquareBracket + 1, indexOfColon - indexOfopenSquareBracket - 1);
            string expla = cleanText.Substring(indexOfColon + 1, indexOfcloseSquareBracket - indexOfColon - 1);

            cleanText = cleanText.Remove(indexOfColon, indexOfcloseSquareBracket - indexOfColon + 1);
            cleanText = cleanText.Remove(indexOfopenSquareBracket, 1);

            //Esto son los indices de el primer caracter kanji y el ultimo que tienen la explicación
            KeyValuePair<int, int> kanjiIndexes = new KeyValuePair<int, int>(indexOfopenSquareBracket, indexOfColon - 2);

            if (!kanjiAndReadings.ContainsKey(kanjiIndexes))
            {
                AddReadingText(kanjiIndexes, expla);

                Debug.Log (kanji + "|" +  expla);
            }

        }
        text = cleanText;
    }

    private void AddReadingText(KeyValuePair<int, int> indexes, string text)
    {
        kanjiAndReadings.Add(indexes, text);

        GameObject txtObject = new GameObject("expl_txt_" + readingsObjects.Count, typeof(RectTransform));
        JapaneseTextAttributes atributes = GetComponent<JapaneseTextAttributes>();
        txtObject.transform.SetParent(transform);

        var txtComp = txtObject.AddComponent<JapaneseText>();

        var attr = txtObject.GetComponent<JapaneseTextAttributes>();
        attr.japaneseSize = atributes.japaneseSize * 0.33f;
        attr.japaneseFont = atributes.japaneseFont;
        attr.characterSpacingValue = atributes.characterSpacingValue * 0.5f;
        attr.japaneseAugmentValue = atributes.japaneseAugmentValue;

        txtComp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        txtComp.SetText(text);

        readingsObjects.Add(indexes, txtComp);
    }

    private void UpdateReadingsText(KeyValuePair<int, int> kanji)
    {
        ForceMeshUpdate();

        TMP_TextInfo txtInfo = GetTextInfo(m_text);

        Vector3 topLeftBorder = txtInfo.characterInfo[kanji.Key].topLeft;
        Vector3 bottomRightBorder = txtInfo.characterInfo[kanji.Value].bottomRight;
        Vector3 topRightBorder = txtInfo.characterInfo[kanji.Value].topRight;

        RectTransform exRectTransform = readingsObjects[kanji].GetComponent<RectTransform>();


        float height = topLeftBorder.y - bottomRightBorder.y;
        exRectTransform.anchorMin = Vector2.one * 0.5f;
        exRectTransform.anchorMax = Vector2.one * 0.5f;
        exRectTransform.anchoredPosition = new Vector2((topLeftBorder.x + topRightBorder.x) * 0.5f, Mathf.Max(topLeftBorder.y, topRightBorder.y));

        exRectTransform.localScale = Vector3.one;

        var explanationAttr = exRectTransform.GetComponent<JapaneseTextAttributes>();

        explanationAttr.japaneseSize = fontSize * 0.33f;

        exRectTransform.sizeDelta = new Vector2((topRightBorder.x - topLeftBorder.x), height);

        exRectTransform.GetComponent<JapaneseText>().ForceMeshUpdate();
    }
}