using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance_;
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
    int uniqueUpdate = 0;
    [HideInInspector]
    public int cardsCount = 0;
    public bool win = false;
    [HideInInspector]
    public bool gameWon = false;
    [HideInInspector]
    public int lastExplanationUsed = -1;
    [HideInInspector]
    public List<string> conditions;
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

    private void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            if (uniqueUpdate <= 0)
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    resourcesBars[i].transform.localScale = new Vector3(1, (float)resources[i] / 100, 1);
                }
                uniqueUpdate++;
            }
            Perder();
            if (gameWon)
            {
                win = true;
                resetGame();
                MySceneManager.Instance.LoadScene("EndScene");
            }
        }
        else if (uniqueUpdate > 0)
            resetGame();
       
    }
    public void Perder()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            if (resources[i] <= 0) {
                win = false;
              
                resetGame();
              
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

    public IEnumerator changeResources()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            print(resources[i] + ", ");
        }

        print("\n");

        float duration = 0.3f;
        float threshold = 0.01f; // Umbral de diferencia permitido
        bool shouldContinue = true;

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            for (int i = 0; i < resources.Length; i++)
            {
                float targetScaleY = (float)resources[i] / 100;
                float currentScaleY = resourcesBars[i].transform.localScale.y;

                if (Mathf.Abs(targetScaleY - currentScaleY) > threshold)
                {
                    float elapsed = 0f;
                    while (shouldContinue)
                    {
                        elapsed += Time.deltaTime;

                        float t = Mathf.Clamp01(elapsed / duration);
                        float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, t);

                        if (elapsed < duration)
                            resourcesBars[i].transform.localScale = new Vector3(1, interpolatedScaleY, 1);
                        else
                        {
                            Debug.Log("Entro concha");
                            resourcesBars[i].transform.localScale = new Vector3(1, targetScaleY, 1);
                            shouldContinue = false;
                        }

                        yield return null;
                    }
                }
                else
                {
                    resourcesBars[i].transform.localScale = new Vector3(1, targetScaleY, 1);
                    yield return null;
                }
            }
        }
    }
    void resetGame()
    {
        uniqueUpdate = 0;
        cardsCount = 0;
        gameWon = false;
        lastExplanationUsed = -1;
    }
}
