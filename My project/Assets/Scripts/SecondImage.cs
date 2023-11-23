using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondImage : MonoBehaviour
{
    //El sprite de la parte de atr�s que se pasar� por el editor
    [SerializeField]
    private Sprite backSprite;
    //Aqu� se guardar� el sprite de frente que se pasar� al script SwipeCard
    //Se pasa por c�digo y cuando se gire 180� se cambiar�
    private Sprite frontSprite;
    //Instancia del swipeCard para acceder a la otra carta, ya que ser� la �nica carta con este script 
    //Adem�s de para pasarle el otro sprite
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

        //Guardamos el gameobject y a�adimos el m�todo al evento de swipeCard
        firstCard_ = swipeCard_.gameObject;
        swipeCard_.cardMoved += cardMovedFront;
    }

    //Llama al creador de cartas y activamos el script de swipeCard y destruimos este
    //Adem�s activamos el animator que es el giro de las cartas
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
