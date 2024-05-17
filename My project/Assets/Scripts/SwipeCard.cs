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
    bool swipedLeft, swipedRight, swipedCenter = false;
    TextMeshProUGUI textMeshProUGUI;
    string textSaved;
    //Guarda la posInicial
    void Start()
    {
        cardWhiteAnimator = transform.GetChild(0).gameObject;
        anotherImage = transform.GetChild(1).gameObject;
        canvas = transform.GetChild(2).gameObject;
        text = transform.GetChild(3).gameObject;
        GameObject gameTextSaved = new GameObject();
        if (transform.childCount > 4)
            gameTextSaved = transform.GetChild(4).gameObject;
        else gameTextSaved = null;

        if (gameTextSaved != null)
        {
            TextMeshProUGUI[] textComponents = gameTextSaved.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI textComponent in textComponents)
            {
                if (textComponent.gameObject.activeSelf){
                    textMeshProUGUI = textComponent; 
                    break; 
                }
            }
        }
        

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
            Carta carta = GetComponent<Carta>();
            int dinero, gente, flora, fauna, aireYAgua;

            if (transform.localPosition != iniPos_ && cardWhiteAnimator.GetComponent<Animation>())
                cardWhiteAnimator.SetActive(false);
            else if (cardWhiteAnimator.GetComponent<Animation>())
                cardWhiteAnimator.SetActive(true);

            if (transform.localPosition.x - iniPos_.x > 0)
            {
                transform.localEulerAngles = new Vector3(0, 0,
                    Mathf.LerpAngle(0, -30, (iniPos_.x + transform.localPosition.x) / (Screen.width / 2)));
                text.GetComponent<TMPro.TextMeshProUGUI>().text = GetComponent<Carta>().SobrescribirSi;
                dinero = carta.SiDinero;
                gente = carta.SiGente;
                flora = carta.SiFlora;
                fauna = carta.SiFauna;
                aireYAgua = carta.SiAire;
            }
            else 
            {
                transform.localEulerAngles = new Vector3(0, 0,
                   Mathf.LerpAngle(0, +30, (iniPos_.x - transform.localPosition.x) / (Screen.width / 2)));
                text.GetComponent<TMPro.TextMeshProUGUI>().text = GetComponent<Carta>().SobrescribeNo;
                dinero = carta.NoDinero;
                gente = carta.NoGente;
                flora = carta.NoFlora;
                fauna = carta.NoFauna;
                aireYAgua = carta.NoAire;
            }

            distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
            GameManager gameManager = GameManager.Instance;


            MyTracker.CardStateChangeEvent trackerEvent = null;

            if (!swipedRight && distancedMoved_ > distanceDragged * Screen.width && transform.localPosition.x - iniPos_.x > 0)
            {
                swipedRight = true;
                swipedCenter = false;
                //Tracker
                trackerEvent = MyTracker.Tracker.Instance.CreateCardStateChangeEvent();
                trackerEvent.CardState = MyTracker.CardStateChangeEvent.CardStateEnum.right;
                MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
            }
            else if (!swipedLeft && distancedMoved_ > distanceDragged * Screen.width && transform.localPosition.x + iniPos_.x < 0)
            {
                swipedLeft = true;
                swipedCenter = false;
                //Tracker
                trackerEvent = MyTracker.Tracker.Instance.CreateCardStateChangeEvent();
                trackerEvent.CardState = MyTracker.CardStateChangeEvent.CardStateEnum.left;
                MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
            }
            else if(!swipedCenter && distancedMoved_ < distanceDragged * Screen.width)
            {
                swipedLeft = false;
                swipedRight = false;
                swipedCenter = true;
                //Tracker
                trackerEvent = MyTracker.Tracker.Instance.CreateCardStateChangeEvent();
                trackerEvent.CardState = MyTracker.CardStateChangeEvent.CardStateEnum.center;
                MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
            }

            if(trackerEvent != null)
                Debug.Log(trackerEvent.EventType + " " + trackerEvent.CardState + " " + trackerEvent.TimeStamp);



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
                
                if (canvas.GetComponent<Image>().color.a <= 0.9f)
                {
                    // Detener la Coroutine existente antes de iniciar la nueva.
                    if(fadeInCoroutine != null)
                        StopCoroutine(fadeInCoroutine);
                
                        fadeInCoroutine = StartCoroutine(FadeIn());
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
            //Tracker
            MyTracker.CardStateChangeEvent trackerEvent = MyTracker.Tracker.Instance.CreateCardStateChangeEvent();
            trackerEvent.CardState = MyTracker.CardStateChangeEvent.CardStateEnum.dropped;
            Debug.Log(trackerEvent.EventType + " " + trackerEvent.CardState + " " + trackerEvent.TimeStamp);
            MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
            transform.localPosition = iniPos_;
            transform.eulerAngles = Vector3.zero;
            if (cardWhiteAnimator.GetComponent<Animation>())
                cardWhiteAnimator.SetActive(true);
        }
        else
        {
            MyTracker.TrackerEvent trackerEvent = MyTracker.Tracker.Instance.CreateRoundStartEvent();
            Debug.Log(trackerEvent.EventType + " " + trackerEvent.TimeStamp);
            MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
            cardMoved?.Invoke();
            Carta carta = GetComponent<Carta>();
            GameManager gameManager = GameManager.Instance;
            EventVariableMixer eventVariable = EventVariableMixer.Instance;
            int dinero, gente, flora, fauna, aireYAgua;
            string extras = "ficha";
            Answers answer;
            answer.question = carta.Pregunta;

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
                answer.answer = carta.SobrescribirSi;
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
                answer.answer = carta.SobrescribeNo;
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

            if(gameManager.cardsCount > 2)
                gameManager.gameAnswers.Add(answer);

            if(dinero != 0)
            {
                gameManager.AddResource(0, dinero);
                StartCoroutine(gameManager.changeMoney());
            }
            if (gente != 0)
            {
                gameManager.AddResource(1, gente);
                StartCoroutine(gameManager.changePeople());
            }
            if (flora != 0)
            {
                gameManager.AddResource(2, flora);
                StartCoroutine(gameManager. changeFlora());
            }
            if (fauna != 0)
            {
                gameManager.AddResource(3, fauna);
                StartCoroutine(gameManager.changeFauna());
            }
            if (aireYAgua != 0)
            {
                gameManager.AddResource(4, aireYAgua);
                StartCoroutine(gameManager.changeAirAndWater());
            }

            gameManager.updateResources();

            EventVariableMixer eventVariableMixer = EventVariableMixer.Instance;

            eventVariableMixer.setMusicParameter("Dinero", GameManager.Instance.resources[0]);
            eventVariableMixer.setMusicParameter("Gente", GameManager.Instance.resources[1]);
            eventVariableMixer.setMusicParameter("Fauna", GameManager.Instance.resources[2]);
            eventVariableMixer.setMusicParameter("Flora", GameManager.Instance.resources[3]);
            eventVariableMixer.setMusicParameter("Aire", GameManager.Instance.resources[4]);

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
        if(textMeshProUGUI)
            textMeshProUGUI.enabled = true;

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