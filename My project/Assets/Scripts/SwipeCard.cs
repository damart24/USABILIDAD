using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCard : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 iniPos_;
    private float distancedMoved_;
    private bool swipeLeft_;
    void Start()
    {
        iniPos_ = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
