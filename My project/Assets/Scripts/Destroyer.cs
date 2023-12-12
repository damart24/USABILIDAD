using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 3f);
    }
}
