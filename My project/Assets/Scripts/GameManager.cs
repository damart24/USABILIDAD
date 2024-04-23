using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct Answers
{
    public string question;
    public string answer;
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance_;
    [HideInInspector]
    public List<Carta> cartasPorPartida;
    [HideInInspector]
    public RectTransform[] resourcesBars = new RectTransform[5];
    [HideInInspector]
    public Image[] imageArray = new Image[5];
    [HideInInspector]
    public Sprite[] circleSprites = new Sprite[2];
    [HideInInspector]
    public TMPro.TextMeshProUGUI questionText =  new TMPro.TextMeshProUGUI();
    [HideInInspector]
    public GameObject[] explanationGameObjects = new GameObject[5];
    [HideInInspector]
    public RectTransform explanationCanvas;
    [HideInInspector]
    public Dictionary<string, GameObject> tokens = new Dictionary<string, GameObject>();
    public int[] resources = { 50, 50, 50, 50, 50 };
    [HideInInspector]
    public int cardsCount = 0;
    [HideInInspector]
    public bool win = false;
    [HideInInspector]
    public bool gameWon = false;
    [HideInInspector]
    public int lastExplanationUsed = -1;
    [HideInInspector]
    public List<string> conditions;
    [HideInInspector]
    public float duration = 0;
    public List<Answers> gameAnswers;
    public void gameStarts()
    {
        MyTracker.TrackerEvent trackerEvent = MyTracker.Tracker.Instance.CreateGameStartEvent();
        MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
        Debug.Log(trackerEvent.EventType + " " + trackerEvent.TimeStamp);
    }
    public static GameManager Instance
    {
        get
        {
            if (instance_ == null)
            {
                GameObject singleton = new GameObject("MySceneManagerSingleton");
                instance_ = singleton.AddComponent<GameManager>();
                DontDestroyOnLoad(singleton);
            }
            return instance_;
        }
    }
    private void OnApplicationQuit()
    {
        MyTracker.TrackerEvent trackerEvent = MyTracker.Tracker.Instance.CreateSessionEndEvent();
        MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
        Debug.Log(trackerEvent.EventType + " " + trackerEvent.TimeStamp);
        MyTracker.Tracker.End();
    }
    private void Awake()
    {
        if (instance_ == null)
        {
            MyTracker.Tracker.Init();
            MyTracker.TrackerEvent trackerEvent = MyTracker.Tracker.Instance.CreateSessionStartEvent();
            MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
            Debug.Log(trackerEvent.EventType + " " + trackerEvent.TimeStamp);
            instance_ = this;
        }
        else
        {
            Destroy(gameObject);
        }

        duration = 0.3f;

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            Perder();
            if (gameWon)
            {
                win = true;
                MySceneManager.Instance.LoadScene("EndScene");
            }
        }
       
    }
    public void Perder()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            if (resources[i] <= 0) {
                win = false;
                resetMusic();
                MySceneManager.Instance.LoadScene("EndScene");
            }
        }
    }
    public void AddResource(int resource, int value)
    {
        resources[resource] += value;

        if (resources[resource] > 200)
            resources[resource] = 200;
        else if (resources[resource] < 0)
            resources[resource] = 0;
    }
    public void showIcon(int resource, int value)
    {
        imageArray[resource].gameObject.SetActive(true);
        if (Mathf.Abs(value) >= 15) imageArray[resource].sprite = circleSprites[1];
        else imageArray[resource].sprite = circleSprites[0];
    }
    public void hideAllIcons()
    {
        for (int i = 0; i < imageArray.Length; i++)
        {
            imageArray[i].gameObject.SetActive(false);
        }
    }
    public IEnumerator changeMoney()
    {
        float threshold = 0.01f; // Umbral de diferencia permitido

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            float targetScaleY = (float)resources[0] / 100;
            float currentScaleY = resourcesBars[0].transform.localScale.y;

            if (Mathf.Abs(targetScaleY - currentScaleY) > threshold)
            {
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsed / duration);
                    float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, t);

                    resourcesBars[0].transform.localScale = new Vector3(1, interpolatedScaleY, 1);
                    
                    yield return null;
                }
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator changePeople()
    {
        float threshold = 0.01f; // Umbral de diferencia permitido

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            float targetScaleY = (float)resources[1] / 100;
            float currentScaleY = resourcesBars[1].transform.localScale.y;

            if (Mathf.Abs(targetScaleY - currentScaleY) > threshold)
            {
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsed / duration);
                    float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, t);

                    resourcesBars[1].transform.localScale = new Vector3(1, interpolatedScaleY, 1);

                    yield return null;
                }
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator changeFlora()
    {
        float threshold = 0.01f; // Umbral de diferencia permitido

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            float targetScaleY = (float)resources[2] / 100;
            float currentScaleY = resourcesBars[2].transform.localScale.y;

            if (Mathf.Abs(targetScaleY - currentScaleY) > threshold)
            {
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsed / duration);
                    float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, t);

                    resourcesBars[2].transform.localScale = new Vector3(1, interpolatedScaleY, 1);

                    yield return null;
                }
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator changeFauna()
    {
        float threshold = 0.01f; // Umbral de diferencia permitido

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            float targetScaleY = (float)resources[3] / 100;
            float currentScaleY = resourcesBars[3].transform.localScale.y;

            if (Mathf.Abs(targetScaleY - currentScaleY) > threshold)
            {
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsed / duration);
                    float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, t);

                    resourcesBars[3].transform.localScale = new Vector3(1, interpolatedScaleY, 1);

                    yield return null;
                }
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator changeAirAndWater()
    {
        float threshold = 0.01f; // Umbral de diferencia permitido

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            float targetScaleY = (float)resources[4] / 100;
            float currentScaleY = resourcesBars[4].transform.localScale.y;

            if (Mathf.Abs(targetScaleY - currentScaleY) > threshold)
            {
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsed / duration);
                    float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, t);

                    resourcesBars[4].transform.localScale = new Vector3(1, interpolatedScaleY, 1);

                    yield return null;
                }
            }
            yield return null;
        }
        yield return null;
    }
    public void updateResources()
    {
        Invoke("FixResources", duration + 0.02f);
    }
    private void FixResources()
    {
        GameManager gameManager = GameManager.Instance;
        for (int i = 0; i < gameManager.resources.Length; i++)
        {
            float targetScaleY = (float)gameManager.resources[i] / 100;
            if(gameManager.resourcesBars[i] != null)
                gameManager.resourcesBars[i].transform.localScale = new Vector3(1, targetScaleY, 1);
        }
    }
    public void resetGame()
    {
        CancelInvoke();
        if (resources != null)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = 50;
            }
        }

        if(gameAnswers !=  null)
            gameAnswers.Clear();
        
        if(cartasPorPartida != null)
            cartasPorPartida.Clear();

        if(tokens != null)
            tokens.Clear();

        if(conditions != null)
            conditions.Clear();

        cardsCount = 0;
        win = false;
        gameWon = false;
        lastExplanationUsed = -1;
    }
    public void resetMusic()
    {
        EventVariableMixer eventVariableMixer = EventVariableMixer.Instance;
        eventVariableMixer.setMusicParameter("Dinero", 50);
        eventVariableMixer.setMusicParameter("Gente", 50);
        eventVariableMixer.setMusicParameter("Fauna", 50);
        eventVariableMixer.setMusicParameter("Flora", 50);
        eventVariableMixer.setMusicParameter("Aire", 50);
    }
}
