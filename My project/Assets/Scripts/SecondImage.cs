using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondImage : MonoBehaviour
{
    [SerializeField]
    private Sprite backSprite;
    private Sprite frontSprite;
    private SwipeCard swipeCard_;
    private GameObject firstCard_;
    private float finalScale_, beginScale_;
    // Start is called before the first frame update
    void Start()
    {
        frontSprite = GetComponent<Image>().sprite;
        GetComponent<Image>().sprite = backSprite;
        finalScale_ = transform.localScale.x;
        beginScale_ = 2.0f;

        swipeCard_ = transform.parent.GetChild(1).GetComponent<SwipeCard>();
        //for (int i = 0; i < transform.parent.childCount; i++)
        //{
        //    if (transform.parent.GetChild(i) != this.transform)
        //    {
        //        swipeCard_ = transform.parent.GetComponentInChildren<SwipeCard>();
        //        break;
        //    }
        //}
        
        firstCard_ = swipeCard_.gameObject;
        transform.localScale = new Vector3(beginScale_, beginScale_, beginScale_);
        swipeCard_.cardMoved += cardMovedFront;
    }

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
        SwipeCard swipeCard = gameObject.GetComponent<SwipeCard>();
        swipeCard.enabled = true;
        swipeCard.frontSprite = frontSprite;
        GetComponent<Animator>().enabled = true;
        Destroy(this);
    }
}
