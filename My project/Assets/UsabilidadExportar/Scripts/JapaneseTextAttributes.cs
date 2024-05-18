using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Componente que solo contiene variables que nos interesa exponer en el editor para el componente JapaneseText,
/// ya que al heredar de TextMeshProUGUI, ya tiene un script de Editor declarado, y no podemos acceder a él directamente, esto es un workaround
/// </summary>
public class JapaneseTextAttributes : MonoBehaviour
{
    public bool vertical;
    public float japaneseAugmentValue;
    public float characterSpacingValue;
    public float japaneseSize;
    public TMPro.TMP_FontAsset japaneseFont;
}
