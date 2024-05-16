using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L10nObject : MonoBehaviour
{
    [SerializeField]
    GameObject spanishObject;
    [SerializeField]
    GameObject japaneseObject;
    void Start()
    {
        switch (L10nManager.Instance.GetLanguage())
        {
            case Language.Spanish:
                spanishObject.SetActive(true);
                japaneseObject.SetActive(false);
                break;
            case Language.Japanese:
                spanishObject.SetActive(false);
                japaneseObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Devuelve el objeto que está activo, si ninguno de ellos está activo devuelve null
    /// </summary>
    /// <returns></returns>
    public GameObject getActiveObject()
    {
        return spanishObject.activeSelf ? spanishObject : 
            japaneseObject.activeSelf ? japaneseObject :
            null;
    }
}
