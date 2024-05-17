using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextAnimator))]
public class WinEndScene : MonoBehaviour
{
    // Start is called before the first frame updatSeri
    [SerializeField]
    string gameOver;
    [SerializeField]
    string winOver;
    TextAnimator animator;

    [SerializeField] GameObject winSound, loseSound;

    void Awake()
    {
        MyTracker.GameEndEvent trackerEvent = MyTracker.Tracker.Instance.CreateGameEndEvent();
        animator = GetComponent<TextAnimator>();
        if (GameManager.Instance.win)
        {
            trackerEvent.Win = true;
            animator.Text = winOver;
            Instantiate(winSound);
        }
        else
        {
            trackerEvent.Win = false; 
            animator.Text = gameOver;
            Instantiate(loseSound);
        }
        MyTracker.Tracker.Instance.TrackEvent(trackerEvent);
        Debug.Log(trackerEvent.EventType + " " + trackerEvent.Win + " " + trackerEvent.TimeStamp);
    }
}
