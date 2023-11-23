using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondImage : MonoBehaviour
{
    //El sprite de la parte de atrás que se pasará por el editor
    [SerializeField]
    private Sprite backSprite;
    //Aquí se guardará el sprite de frente que se pasará al script SwipeCard
    //Se pasa por código y cuando se gire 180º se cambiará
    private Sprite frontSprite;
    //Instancia del swipeCard para acceder a la otra carta, ya que será la única carta con este script 
    //Además de para pasarle el otro sprite
    private SwipeCard swipeCard_;
    private GameObject firstCard_;
    // Start is called before the first frame update
    void Start()
    {
        //Intercambian sprites y se guarda el frontal
        frontSprite = GetComponent<Image>().sprite;
        GetComponent<Image>().sprite = backSprite;

        //Conseguimos la referencia a la carta volteada
        swipeCard_ = transform.parent.GetChild(1).GetComponent<SwipeCard>();
        //for (int i = 0; i < transform.parent.childCount; i++)
        //{
        //    if (transform.parent.GetChild(i) != this.transform)
        //    {
        //        swipeCard_ = transform.parent.GetComponentInChildren<SwipeCard>();
        //        break;
        //    }
        //}

        //Guardamos el gameobject y añadimos el método al evento de swipeCard
        firstCard_ = swipeCard_.gameObject;
        swipeCard_.cardMoved += cardMovedFront;
    }

    //Llama al creador de cartas y activamos el script de swipeCard y destruimos este
    //Además activamos el animator que es el giro de las cartas
    void cardMovedFront()
    {
        transform.parent.GetComponent<CardGenerator>().InstantiateCard();
        SwipeCard swipeCard = gameObject.GetComponent<SwipeCard>();
        swipeCard.enabled = true;
        swipeCard.frontSprite = frontSprite;
        GetComponent<Animator>().enabled = true;
        Destroy(this);
    }
}
