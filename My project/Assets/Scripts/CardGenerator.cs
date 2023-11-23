using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    //Prefab de la carta que se va a generar
    [SerializeField]
    private GameObject cardPrefab_;
    //Lista de sprites aleatorios que se van a cambiar
    [SerializeField]
    private Sprite[] sprites_;
    //Instancia la nueva carta que estará detrás del mazo
    //Le pone como primer hijo y cambia su sprite por un sprite aleatorio
    public void InstantiateCard()
    {
        GameObject newCard = Instantiate(cardPrefab_, transform, false);
        newCard.transform.SetAsFirstSibling();
        newCard.GetComponent<Image>().sprite = sprites_[Random.Range(0, 2)];
    }
}
