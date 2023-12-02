using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : MonoBehaviour
{
    void Start()
    {
        GetComponent<SwipeCard>().cardAvalaible = true;
    }
}
