using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorDisable : MonoBehaviour
{
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
