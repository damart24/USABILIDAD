using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject text, canvas;
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
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    [HideInInspector]
    public bool cardAvalaible = false;

    //Guarda la posInicial
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>())
                text = transform.GetChild(i).gameObject;
            else
                canvas = transform.GetChild(i).gameObject;
        }
        iniPos_ = transform.position;

        canvas.SetActive(true);
        text.SetActive(true);
        Color canvasColor = canvas.GetComponent<Image>().color;
        canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, 0);
        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 0);
    }
    //Movemos la posición y calculamos la diferencia entre la posX actual y la original
    //Dependiendo de esa diferencia rotará más o menos y dependiendo el lado, a la izquierda o a la derecha
    public void OnDrag(PointerEventData eventData)
    {
        if (cardAvalaible)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);

            if (transform.localPosition.x - iniPos_.x > 0)
            {
                transform.localEulerAngles = new Vector3(0, 0,
                    Mathf.LerpAngle(0, -30, (iniPos_.x + transform.localPosition.x) / (Screen.width / 2)));
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0,
                   Mathf.LerpAngle(0, +30, (iniPos_.x - transform.localPosition.x) / (Screen.width / 2)));
            }

            distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
            GameManager gameManager = GameManager.Instance;

            if (distancedMoved_ < distanceDragged * Screen.width)
            {
                gameManager.hideAllIcons();
                if (canvas.GetComponent<Image>().color.a >= 0.1f)
                {
                    // Detener la Coroutine existente antes de iniciar la nueva.
                    if (fadeOutCoroutine != null)
                        StopCoroutine(fadeOutCoroutine);
               
                        fadeOutCoroutine = StartCoroutine(FadeOut());
                }
            }
            else
            {
                Carta carta = GetComponent<Carta>();
                int dinero, gente, flora, fauna, aireYAgua;
                if (canvas.GetComponent<Image>().color.a <= 0.9f)
                {
                    // Detener la Coroutine existente antes de iniciar la nueva.
                    if(fadeInCoroutine != null)
                        StopCoroutine(fadeInCoroutine);
                
                        fadeInCoroutine = StartCoroutine(FadeIn());

                    if (transform.localPosition.x > iniPos_.x)
                        text.GetComponent<TMPro.TextMeshProUGUI>().text = GetComponent<Carta>().SobrescribirSi;
                    else
                        text.GetComponent<TMPro.TextMeshProUGUI>().text = GetComponent<Carta>().SobrescribeNo;
                }

                if (transform.localPosition.x - iniPos_.x > 0)
                {
                    dinero = carta.SiDinero;
                    gente = carta.SiGente;
                    flora = carta.SiFlora;
                    fauna = carta.SiFauna;
                    aireYAgua = carta.SiAire;
                }
                else
                {
                    dinero = carta.NoDinero;
                    gente = carta.NoGente;
                    flora = carta.NoFlora;
                    fauna = carta.NoFauna;
                    aireYAgua = carta.NoAire;
                }

                if (dinero != 0)
                    gameManager.showIcon(0, dinero);
                if (gente != 0)
                    gameManager.showIcon(1, gente);
                if (flora != 0)
                    gameManager.showIcon(2, flora);
                if (fauna != 0)
                    gameManager.showIcon(3, fauna);
                if (aireYAgua != 0)
                    gameManager.showIcon(4, aireYAgua);
            }
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
        float time = 0;

        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
        if (distancedMoved_ < distanceDragged * Screen.width)
        {
            transform.localPosition = iniPos_;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            Carta carta = GetComponent<Carta>();
            GameManager gameManager = GameManager.Instance;
            int dinero, gente, flora, fauna, aireYAgua;
            if (transform.localPosition.x > iniPos_.x)
            {
                dinero = carta.SiDinero;
                gente = carta.SiGente;
                flora = carta.SiFlora;
                fauna = carta.SiFauna;
                aireYAgua = carta.SiAire;
                swipeLeft_ = false;
            }
            else
            {
                dinero = carta.NoDinero;
                gente = carta.NoGente;
                flora = carta.NoFlora;
                fauna = carta.NoFauna;
                aireYAgua = carta.NoAire;
                swipeLeft_ = true;
            }
            gameManager.hideAllIcons();

            if(dinero != 0)
                gameManager.AddResource(0, dinero);
            if (gente != 0)
                gameManager.AddResource(1, gente);
            if (flora != 0)
                gameManager.AddResource(2, flora);
            if (fauna != 0)
                gameManager.AddResource(3, fauna);
            if (aireYAgua != 0)
                gameManager.AddResource(4, aireYAgua);

            StartCoroutine(gameManager.changeResources());
            cardMoved?.Invoke();
            StartCoroutine(MovedCard());
        }
    }
    //Al haber movido la carta, hace scroll hacia un lateral y un fadeout, al terminar
    private IEnumerator MovedCard()
    {
        float time = 0;

        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

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
            text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, Mathf.SmoothStep(1, 0, 4 * time));
            canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null;
        }
        Destroy(gameObject);
    }
    //Cambia el sprite por el frontal desde un evento en el animator
    public void changeSprite()
    {
        GetComponent<Image>().sprite = frontSprite;
    }
    //Desactiva el animator desde un evento en el animator
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
        GameManager.Instance.questionText.text = GetComponent<Carta>().Pregunta;
        cardAvalaible = true;
    }
    private IEnumerator FadeOut()
    {
        StopCoroutine(FadeIn());
        float duration = 0.1f;

        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        while (canvas.GetComponent<Image>().color.a > 0 ||
            text.GetComponent<TMPro.TextMeshProUGUI>().color.a > 0)
        {
            canvasColor.a -= Time.deltaTime / duration;
            canvas.GetComponent<Image>().color =
                new Color(canvasColor.r, canvasColor.g, canvasColor.b, canvasColor.a);

            textColor.a -= Time.deltaTime / duration;
            text.GetComponent<TMPro.TextMeshProUGUI>().color =
                new Color(textColor.r, textColor.g, textColor.b, textColor.a);
            yield return null;
        }

        canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, 0);
        text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 0);
    }
    private IEnumerator FadeIn()
    {
        StopCoroutine(FadeOut());
        float duration = 0.1f;

        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        while (canvas.GetComponent<Image>().color.a < 1 ||
            text.GetComponent<TMPro.TextMeshProUGUI>().color.a < 1)
        {
            canvasColor.a += Time.deltaTime / duration;
            canvas.GetComponent<Image>().color =
                new Color(canvasColor.r, canvasColor.g, canvasColor.b, canvasColor.a);

            textColor.a += Time.deltaTime / duration;
            text.GetComponent<TMPro.TextMeshProUGUI>().color =
                new Color(textColor.r, textColor.g, textColor.b, textColor.a);

            yield return null;
        }
        canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, 1);
        text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 1);
    }
}