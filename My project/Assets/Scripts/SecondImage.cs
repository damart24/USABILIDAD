using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondImage : MonoBehaviour
{
    private SwipeCard swipeCard_;
    private GameObject firstCard_;
    private float finalScale_, beginScale_;
    // Start is called before the first frame update
    void Start()
    {
        finalScale_ = transform.localScale.x;
        beginScale_ = 2.0f;
        swipeCard_ = transform.parent.GetComponentInChildren<SwipeCard>();
        firstCard_ = swipeCard_.gameObject;
        transform.localScale = new Vector3(beginScale_, beginScale_, beginScale_);
        swipeCard_.cardMoved += cardMovedFront;
    }

    // Update is called once per frame
    void Update()
    {
        float distancedMoved = firstCard_.transform.localPosition.x;

        if(Mathf.Abs(distancedMoved) > 0)
        {
            float step = Mathf.SmoothStep(beginScale_, finalScale_, Mathf.Abs(distancedMoved) / (Screen.width / 2));
            transform.localScale = new Vector3(step, step, step);
        }
    }

    void cardMovedFront()
    {
        transform.parent.GetComponent<CardGenerator>().InstantiateCard();
        gameObject.AddComponent<SwipeCard>();
        Destroy(this);
    }
}
