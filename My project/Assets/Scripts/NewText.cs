using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewText : TextMeshProUGUI
{ 
    protected override void Start()
    {
        base.Start();
        text = base.text.Replace("<rotate=90>", "");
        text = "<rotate=90>" + text;
        rectTransform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -90);
    }
}
