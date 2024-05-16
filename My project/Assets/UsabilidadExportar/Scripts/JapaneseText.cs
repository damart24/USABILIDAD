using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(JapaneseTextAttributes))]
public class JapaneseText : TextMeshProUGUI
{
    private int valueToRotate = -90;
    private int valueToRotateNotJapanese = 0;
    private string rotateText = "<rotate=90>";
    protected override void Start()
    {
        base.Start();
        JapaneseTextAttributes atributes = GetComponent<JapaneseTextAttributes>();
        fontSize = atributes.japaneseSize;
        font = atributes.japaneseFont;
        if (L10nManager.Instance.GetLanguage() == Language.Japanese && atributes.vertical){
            
            text = rotateText + text;
            rectTransform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, valueToRotate);
            characterSpacing = atributes.characterSpacingValue;
            characterSpacing *= atributes.japaneseAugmentValue;
        }
        else
        {
            text = base.text.Replace(rotateText, "");
            rectTransform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, valueToRotateNotJapanese);
            characterSpacing = atributes.characterSpacingValue;
            characterSpacing /= atributes.japaneseAugmentValue;
            fontSize /= atributes.japaneseAugmentValue;
        }
    }
}