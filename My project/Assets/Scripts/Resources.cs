using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public RectTransform[] resourcesBars = new RectTransform[5];
    public Image[] imageArray = new Image[5];
    public Sprite[] circleSprite = new Sprite[2];
    private void Start()
    {
        for (int i = 0; i < resourcesBars.Length; i++)
        {
            GameManager.instance.resourcesBars[i] = resourcesBars[i];
        }
        for (int i = 0; i < resourcesBars.Length; i++)
        {
            GameManager.instance.imageArray[i] = imageArray[i];
        }
        for (int i = 0; i < circleSprite.Length; i++)
        {
            GameManager.instance.circleSprites[i] = circleSprite[i];
        }
    }
}
