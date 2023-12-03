using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explicacion : MonoBehaviour
{
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
