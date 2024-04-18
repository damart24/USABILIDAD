using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTracker 
{
    public class GameStartEvent : TrackerEvent
    {
        public GameStartEvent() 
        {
            eventType = EventType.gameStart;
        }
    }

    public class GameEndEvent : TrackerEvent
    {
        private bool win;
        public bool Win { get => win; set => win = value; }

        public GameEndEvent()
        {
            eventType = EventType.gameEnd;
        }
    }

    public class SessionStartEvent : TrackerEvent
    {
        public SessionStartEvent()
        {
            eventType = EventType.sessionStart;
        }
    }

    public class SessionEndEvent : TrackerEvent
    {
        public SessionEndEvent()
        {
            eventType = EventType.sessionEnd;
        }
    }

    public class RoundStartEvent : TrackerEvent
    {
        public RoundStartEvent()
        {
            eventType = EventType.roundStart;
        }
    }

    public class CardStateChangeEvent : TrackerEvent
    {
        public enum CardStateEnum 
        {
            left, right, center, dropped
        }

        private CardStateEnum cardState;

        public CardStateEnum CardState { get => cardState; set => cardState = value; }

        public CardStateChangeEvent() 
        {
            eventType = EventType.cardStateChange;
        }
    }
}
