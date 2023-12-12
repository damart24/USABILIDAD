using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject text, canvas, anotherImage, cardWhiteAnimator;
    //Distancia que se mueve la carta para que desaparezca
    private const float distanceDragged = 0.15f;
    //Guarda la posici�n inicial
    private Vector3 iniPos_;
    //Guarda la distancia movida desde la iniPos_ hasta la distancia movida con el click en el OnDrag
    private float distancedMoved_;
    //Booleano para saber si es hacia un lado u  otro, es decir izquierda o derecha
    private bool swipeLeft_;
    //Sprite p�blico al que se le pasa la referencia desde secondImage
    [HideInInspector]
    public Sprite frontSprite;
    //Evento que suceder� al hacer el volteo de la carta y har� llamar a los m�todos suscritos a �l
    public event Action cardMoved;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    [HideInInspector]
    public bool cardAvalaible = false;

    //Guarda la posInicial
    void Start()
    {
        cardWhiteAnimator = transform.GetChild(0).gameObject;
        anotherImage = transform.GetChild(1).gameObject;
        canvas = transform.GetChild(2).gameObject;
        text = transform.GetChild(3).gameObject;
       
        iniPos_ = transform.position;

        canvas.SetActive(true);
        text.SetActive(true);
        Color canvasColor = canvas.GetComponent<Image>().color;
        canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, 0);
        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 0);
    }
    //Movemos la posici�n y calculamos la diferencia entre la posX actual y la original
    //Dependiendo de esa diferencia rotar� m�s o menos y dependiendo el lado, a la izquierda o a la derecha
    public void OnDrag(PointerEventData eventData)
    {
        if (cardAvalaible)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);

            if(transform.localPosition != iniPos_ && cardWhiteAnimator.GetComponent<Animation>())
                cardWhiteAnimator.SetActive(false);
            else if (cardWhiteAnimator.GetComponent<Animation>())
                cardWhiteAnimator.SetActive(true);

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
    //Al terminar el drag si se ha movido m�s de la mitad de la pantalla * 0.3
    //Llama al invoke de cardMoved, sino vueve a la posici�n inicial y con la rotaci�n actual
    public void OnEndDrag(PointerEventData eventData)
    {
        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
        if (distancedMoved_ < distanceDragged * Screen.width)
        {
            transform.localPosition = iniPos_;
            transform.eulerAngles = Vector3.zero;
            if (cardWhiteAnimator.GetComponent<Animation>())
                cardWhiteAnimator.SetActive(true);
        }
        else
        {
            cardMoved?.Invoke();
            Carta carta = GetComponent<Carta>();
            GameManager gameManager = GameManager.Instance;
            EventVariableMixer eventVariable = EventVariableMixer.Instance;
            int dinero, gente, flora, fauna, aireYAgua;
            string extras = "ficha";

            if (transform.localPosition.x > iniPos_.x)
            {
                if (gameManager.cardsCount - 3 >= 0 && gameManager.cartasPorPartida[gameManager.cardsCount - 3].ExtrasSi != "")
                {
                    gameManager.conditions.Add(gameManager.cartasPorPartida[gameManager.cardsCount - 3].ExtrasSi);
                    extras = gameManager.cartasPorPartida[gameManager.cardsCount - 3].ExtrasSi;
                }

                Instantiate(eventVariable.yesSound);
                dinero = carta.SiDinero;
                gente = carta.SiGente;
                flora = carta.SiFlora;
                fauna = carta.SiFauna;
                aireYAgua = carta.SiAire;
                swipeLeft_ = false;
            }
            else
            {
                if (gameManager.cardsCount - 3 >= 0 && gameManager.cartasPorPartida[gameManager.cardsCount - 3].ExtrasNo != "")
                {
                    gameManager.conditions.Add(gameManager.cartasPorPartida[gameManager.cardsCount - 3].ExtrasNo);
                    extras = gameManager.cartasPorPartida[gameManager.cardsCount - 3].ExtrasNo;
                }
                Instantiate(eventVariable.noSound);
                dinero = carta.NoDinero;
                gente = carta.NoGente;
                flora = carta.NoFlora;
                fauna = carta.NoFauna;
                aireYAgua = carta.NoAire;
                swipeLeft_ = true;
            }

            // Verificar si el personaje est� en el diccionario
            if (gameManager.tokens.TryGetValue(extras, out GameObject tokenGameObject))
            {
                Instantiate(eventVariable.tokenSound);
                tokenGameObject.GetComponent<Animator>().enabled = true;
                tokenGameObject.GetComponent<Image>().enabled = true;
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

            EventVariableMixer eventVariableMixer = EventVariableMixer.Instance;

            eventVariableMixer.setMusicParameter("Dinero", (int)(GameManager.Instance.resources[0]));
            eventVariableMixer.setMusicParameter("Gente", (int)(GameManager.Instance.resources[1]));
            eventVariableMixer.setMusicParameter("Fauna", (int)(GameManager.Instance.resources[2]));
            eventVariableMixer.setMusicParameter("Flora", (int)(GameManager.Instance.resources[3]));
            eventVariableMixer.setMusicParameter("Aire", (int)(GameManager.Instance.resources[4]));

            StartCoroutine(gameManager.changeResources());
            String textoExplicativo = GetComponent<Carta>().TextoExplicativo;

            textoExplicativo = textoExplicativo.Replace(";", "");

            if (textoExplicativo != "")
            {
                Instantiate(eventVariable.paperSound);
                int explanationNumber =  gameManager.lastExplanationUsed;
                while (explanationNumber == gameManager.lastExplanationUsed)
                    explanationNumber = UnityEngine.Random.Range(0, 5);

                GameObject explanation = gameManager.explanationGameObjects[explanationNumber];
                GameObject explanationObjectInstance = Instantiate(explanation, gameManager.explanationCanvas);
                explanationObjectInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = textoExplicativo;
            }
           
            StartCoroutine(MovedCard());
        }
    }
    //Al haber movido la carta, hace scroll hacia un lateral y un fadeout, al terminar
    private IEnumerator MovedCard()
    {
        float time = 0;

        Color textColor = text.GetComponent<TMPro.TextMeshProUGUI>().color;
        Color canvasColor = canvas.GetComponent<Image>().color;

        while (anotherImage.GetComponent<Image>().color != new Color(1, 1, 1, 0))
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
            anotherImage.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
            text.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, Mathf.SmoothStep(1, 0, 4 * time));
            canvas.GetComponent<Image>().color = new Color(canvasColor.r, canvasColor.g, canvasColor.b, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null;
        }
        Destroy(gameObject);
    }
    //Cambia el sprite por el frontal desde un evento en el animator
    public void changeSprite()
    {
        anotherImage.GetComponent<Image>().sprite = frontSprite;
    }
    //Desactiva el animator desde un evento en el animator
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
        GameManager.Instance.questionText.text = GetComponent<Carta>().Pregunta;
        cardWhiteAnimator.SetActive(true);
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