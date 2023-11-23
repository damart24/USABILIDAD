using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //Distancia que se mueve la carta para que desaparezca
    private const float distanceDragged = 0.15f;
    //Guarda la posición inicial
    private Vector3 iniPos_;
    //Guarda la distancia movida desde la iniPos_ hasta la distancia movida con el click en el OnDrag
    private float distancedMoved_;
    //Booleano para saber si es hacia un lado u  otro, es decir izquierda o derecha
    private bool swipeLeft_;
    //Sprite público al que se le pasa la referencia desde secondImage
    [HideInInspector]
    public Sprite frontSprite;
    //Evento que sucederá al hacer el volteo de la carta y hará llamar a los métodos suscritos a él
    public event Action cardMoved;
    //Guarda la posInicial
    void Start()
    {
        iniPos_ = transform.position;
    }
    //Movemos la posición y calculamos la diferencia entre la posX actual y la original
    //Dependiendo de esa diferencia rotará más o menos y dependiendo el lado, a la izquierda o a la derecha
    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
    
        if(transform.localPosition.x - iniPos_.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0,
                Mathf.LerpAngle(0, -30, (iniPos_.x + transform.localPosition.x) / (Screen.width / 2)));
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0,
               Mathf.LerpAngle(0, +30, (iniPos_.x - transform.localPosition.x) / (Screen.width / 2)));
        }
    }
    //Cuando empieza el drag, es decir el click sobre la imagen y movimiento guardamos la posOriginal
    public void OnBeginDrag(PointerEventData eventData)
    {
        iniPos_ = transform.localPosition;
    }
    //Al terminar el drag si se ha movido más de la mitad de la pantalla * 0.3
    //Llama al invoke de cardMoved, sino vueve a la posición inicial y con la rotación actual
    public void OnEndDrag(PointerEventData eventData)
    {
        distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
        if(distancedMoved_ < distanceDragged * Screen.width)
        {
            transform.localPosition = iniPos_;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            if (transform.localPosition.x > iniPos_.x)
                swipeLeft_ = false;
            else
                swipeLeft_ = true;

            cardMoved?.Invoke();
            StartCoroutine(MovedCard());
        }
    }
    //Al haber movido la carta, hace scroll hacia un lateral y un fadeout, al terminar
    private IEnumerator MovedCard()
    {
        float time = 0;
        
        while (GetComponent<Image>().color != new Color(1, 1, 1, 0))
        {
            time += Time.deltaTime;
            if (swipeLeft_)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x, 
                    transform.localPosition.x - 10, time), transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + 10, time), transform.localPosition.y, 0);
            }
            GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null;
        }
        Destroy(gameObject);
    }
    //Cambia el sprite por el frontal desde un evento en el animator
    public void changeSprite() {
        GetComponent<Image>().sprite = frontSprite;
    }
    //Desactiva el animator desde un evento en el animator
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}