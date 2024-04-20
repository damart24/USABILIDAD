using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        main();
    }

    private static void main()
    {
        MyTracker.CardStateChangeEvent gameStart = new MyTracker.CardStateChangeEvent();
        gameStart.CardState = MyTracker.CardStateChangeEvent.CardStateEnum.dropped;
    }
}
