using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NewText : TextMeshProUGUI
{
    const float japaneseAugmentValue = 1.2f;
    const float characterSpacingValue = 20f;
    private int valueToRotate = -90;
    private string rotateText = "<rotate=90>";
    protected override void Start()
    {
        base.Start();
        text = base.text.Replace(rotateText, "");
        text = rotateText + text;
        rectTransform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, valueToRotate);
        characterSpacing = characterSpacingValue;
        characterSpacing *= japaneseAugmentValue;
    }
}
