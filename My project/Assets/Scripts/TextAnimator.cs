using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    public float delayBetweenLetters = 0.05f;  // Tiempo de espera entre letras
    private TextMeshProUGUI textMeshPro;
    public string escenas = "GameScene";
    public string text;
    string gameOver = "Has sido destetuido de tu cargo ,no has gestionado los recursos.Espabila para la proxima vez.";
    string winOver = "Has cumplido exitosamente con tus responsabilidades. ¡Bien hecho!";
    

    void Start()
    {
        if (MySceneManager.Instance.getActiveSceneName() == "EndScene")
        {
            if (GameManager.Instance.win) text = winOver;
            else text = gameOver;
        }
        
        textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimateText(text));
    }

    IEnumerator AnimateText(string text)
    {
        textMeshPro.text = "";
        foreach (char c in text)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(delayBetweenLetters);
        }
        yield return new WaitForSeconds(3f); // Puedes ajustar el tiempo de espera antes de cambiar de escena
        MySceneManager.Instance.LoadScene(escenas);
    }
}
