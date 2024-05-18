using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    public float delayBetweenLetters = 0.05f;  // Tiempo de espera entre letras
    private TextMeshProUGUI textMeshPro;
    public string escenas = "GameScene";
    [SerializeField]
    private string text;
    public string Text { get => text; set => text = value; }
    private string shownString;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimateText(text));
    }

    IEnumerator AnimateText(string text)
    {
        shownString = "";
        foreach (char c in text)
        {
            shownString += c;
            textMeshPro.text = shownString;
            yield return new WaitForSeconds(delayBetweenLetters);
        }
        yield return new WaitForSeconds(3f); // Puedes ajustar el tiempo de espera antes de cambiar de escena
        if(MySceneManager.Instance.getActiveSceneName() != "EndScene")
            MySceneManager.Instance.LoadScene(escenas);
    }
}
