using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public RectTransform[] resourcesBars = new RectTransform[5];
    public Image[] imageArray = new Image[5];
    public Sprite[] circleSprite = new Sprite[2];
    public TMPro.TextMeshProUGUI questionText = new TMPro.TextMeshProUGUI();
    public GameObject[] explanationGameObjects = new GameObject[5];
    public RectTransform explanationCanvas;
    public string[] tokensNames = new string[3];
    public GameObject[] tokens = new GameObject[3];
    private void Awake()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.resetGame();
        for (int i = 0; i < resourcesBars.Length; i++)
        {
            gameManager.resourcesBars[i] = resourcesBars[i];
        }
        for (int i = 0; i < resourcesBars.Length; i++)
        {
            gameManager.imageArray[i] = imageArray[i];
        }
        for (int i = 0; i < circleSprite.Length; i++)
        {
            gameManager.circleSprites[i] = circleSprite[i];
        }
        gameManager.questionText = questionText;
        for (int i = 0; i < explanationGameObjects.Length; i++)
        {
            gameManager.explanationGameObjects[i] = explanationGameObjects[i];
        }
        gameManager.explanationCanvas =  explanationCanvas;
        for (int i = 0; i < tokensNames.Length; i++)
        {
            gameManager.tokens.Add(tokensNames[i], tokens[i]);
        }
    }
}
