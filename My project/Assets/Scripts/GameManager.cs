using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance_; // Singleton pattern
    Carta[] cartasPorPartida;

    [HideInInspector]
    public RectTransform[] resourcesBars = new RectTransform[5];

    [HideInInspector]
    public Image[] imageArray = new Image[5];

    [HideInInspector]
    public Sprite[] circleSprites = new Sprite[2];
    [HideInInspector]
    public TMPro.TextMeshProUGUI questionText =  new TMPro.TextMeshProUGUI();

    private int[] resources = { 50, 50, 50, 50, 50 };
    int uniqueUpdate = 0;
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
        // Implementing a Singleton pattern to ensure only one instance of GameManager exists
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
            if(uniqueUpdate <= 0)
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    resourcesBars[i].transform.localScale = new Vector3(1, (float)resources[i] / 100, 1);
                }
                uniqueUpdate++;
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
        float duration = 0.3f;

        while (MySceneManager.Instance.getActiveSceneName() == "GameScene")
        {
            for (int i = 0; i < resources.Length; i++)
            {
                float targetScaleY = (float)resources[i] / 100;
                float currentScaleY = resourcesBars[i].transform.localScale.y;
                float interpolatedScaleY = Mathf.Lerp(currentScaleY, targetScaleY, Time.deltaTime / duration);

                resourcesBars[i].transform.localScale = new Vector3(1, interpolatedScaleY, 1);
            }

            Debug.Log("Aqui estoy");

            yield return null;
        }

        // Asegúrate de que los valores finales estén configurados según sea necesario
    }

}
