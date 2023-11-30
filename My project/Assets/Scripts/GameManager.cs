using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton pattern
    Carta[] cartasPorPartida;

    [HideInInspector]
    public RectTransform[] resourcesBars = new RectTransform[5];

    [HideInInspector]
    public Image[] imageArray = new Image[5];

    [HideInInspector]
    public Sprite[] circleSprites = new Sprite[2];

    private int[] resources = { 50, 50, 50, 50, 50 };

    private void Awake()
    {
        // Implementing a Singleton pattern to ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
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
            for (int i = 0; i < resources.Length; i++)
            {
                resourcesBars[i].transform.localScale = new Vector3(1, (float)resources[i]/100, 1);
            }
        }
    }

    public void AddResource(int resource, int value)
    {
        resources[resource] += value;
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
}
