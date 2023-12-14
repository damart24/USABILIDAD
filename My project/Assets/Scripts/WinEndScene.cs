using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEndScene : MonoBehaviour
{
    // Start is called before the first frame update
    string gameOver = "Has sido destituido de tu cargo, no has gestionado los recursos. Espabila para la proxima vez.";
    string winOver = "Has cumplido exitosamente con tus responsabilidades. ¡Bien hecho!";
    TextAnimator animator;

    [SerializeField] GameObject winSound, loseSound;

    void Start()
    {
        animator = GetComponent<TextAnimator>();
        if (GameManager.Instance.win)
        {
            animator.text = winOver;
            Instantiate(winSound);
        }
        else
        {
            animator.text = gameOver;
            Instantiate(loseSound);
        }

    }
}
