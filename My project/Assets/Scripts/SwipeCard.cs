using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private Vector3 iniPos_;
    private float distancedMoved_;
    private bool swipeLeft_;
    [HideInInspector]
    public Sprite frontSprite;

    public event Action cardMoved;
    void Start()
    {
        iniPos_ = this.transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
    
        if(transform.localPosition.x - iniPos_.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0,
                Mathf.LerpAngle(0, -30, (iniPos_.x + transform.localPosition.x) / (Screen.width / 2)));
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0,
               Mathf.LerpAngle(0, +30, (iniPos_.x - transform.localPosition.x) / (Screen.width / 2)));
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        iniPos_ = transform.localPosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        distancedMoved_ = Mathf.Abs(transform.localPosition.x - iniPos_.x);
        if(distancedMoved_ < 0.3 * Screen.width)
        {
            transform.localPosition = iniPos_;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            if (transform.localPosition.x > iniPos_.x)
                swipeLeft_ = false;
            else
                swipeLeft_ = true;

            cardMoved?.Invoke();
            StartCoroutine(MovedCard());
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0;
        
        while (GetComponent<Image>().color != new Color(1, 1, 1, 1))
        {
            time += Time.deltaTime;
            if (swipeLeft_)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x, 
                    transform.localPosition.x - Screen.width, 40 * time), transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + Screen.width, 40 * time), transform.localPosition.y, 0);
            }
            GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 40 * time));
            yield return null;
        }
        Destroy(gameObject);
    }
    public void changeSprite() {
        GetComponent<Image>().sprite = frontSprite;
    }
    public void disableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}