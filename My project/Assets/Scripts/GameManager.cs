using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton pattern
    Carta[] cartasPorPartida;

    public RectTransform[] resourcesBars = new RectTransform[5];

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
        for (int i = 0; i < resources.Length; i++)
        {
            resourcesBars[i].transform.localScale = new Vector3(1, (float)resources[i]/100, 1);
        }
    }

    void AddResource(int resource, int value)
    {
        resources[resource] += value;
    }

}
