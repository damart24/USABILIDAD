using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab_;
    [SerializeField]
    private Sprite[] sprites_;
    public void InstantiateCard()
    {
        GameObject newCard = Instantiate(cardPrefab_, transform, false);
        newCard.transform.SetAsFirstSibling();
        int r = Random.Range(0, 2);
        Debug.Log(r);
        newCard.GetComponent<Image>().sprite = sprites_[r];
    }
}
