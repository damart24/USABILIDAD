using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesEndScene : MonoBehaviour
{
    [SerializeField]
    GameObject texto, contenedor;
    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        for (int i = 0; i < gameManager.cardsCount - 2; i++)
        {
            GameObject container = Instantiate(contenedor, transform);

            GameObject questionNumber = Instantiate(texto, container.transform);
            TMPro.TextMeshProUGUI questionNumberText = questionNumber.GetComponent<TMPro.TextMeshProUGUI>();

            GameObject questionInstantiated = Instantiate(texto, container.transform);
            TMPro.TextMeshProUGUI questionText = questionInstantiated.GetComponent<TMPro.TextMeshProUGUI>();

            GameObject answerInstantiated = Instantiate(texto, container.transform);
            TMPro.TextMeshProUGUI answerText = answerInstantiated.GetComponent<TMPro.TextMeshProUGUI>();

            int number = i + 1;
            questionNumberText.text = (i + 1).ToString();
            questionText.text = gameManager.gameAnswers[i].question;
            answerText.text = gameManager.gameAnswers[i].answer;

            questionNumberText.enableWordWrapping = false;
            if (answerText.text == "Si" || answerText.text == "No")
                answerText.enableWordWrapping = false;
        }
    }
}
