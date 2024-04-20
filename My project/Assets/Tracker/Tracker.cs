using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyTracker
{
    public class Tracker
    {
        private IPersistence persistenceObject;

        private static Tracker instance;

        public static Tracker Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                return null;
            }
        }

        private Tracker() { }

        public static void Init()
        {
            instance = new Tracker();
        }

        public static void End()
        {
            instance.end();
        }

        private void end() 
        {
            persistenceObject.Flush();
        }

        public void TrackEvent(TrackerEvent trackerEvent)
        {
            persistenceObject.Send(trackerEvent);
        }

        public GameStartEvent CreateGameStartEvent() 
        {
            return new GameStartEvent();
        }

        public GameEndEvent CreateGameEndEvent()
        {
            return new GameEndEvent();
        }

        public SessionStartEvent CreateSessionStartEvent()
        {
            return new SessionStartEvent();
        }

        public SessionEndEvent CreateSessionEndEvent()
        {
            return new SessionEndEvent();
        }

        public RoundStartEvent CreateRoundStartEvent() 
        {
            return new RoundStartEvent();
        }

        public CardStateChangeEvent CreateCardStateChangeEvent() 
        {
            return new CardStateChangeEvent();
        }
    }
}
