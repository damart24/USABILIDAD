using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEndScene : MonoBehaviour
{
    // Start is called before the first frame update
    string gameOver = "Has sido destetuido de tu cargo ,no has gestionado los recursos.Espabila para la proxima vez.";
    string winOver = "Has cumplido exitosamente con tus responsabilidades. ¡Bien hecho!";
    TextAnimator animator;

    void Start()
    {
        animator = GetComponent<TextAnimator>();
        if (GameManager.Instance.gameWon) animator.text = winOver;
        else animator.text = gameOver;


    }
}
