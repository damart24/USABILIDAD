using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTracker
{
    public abstract class IPersistence
    {
        protected List<TrackerEvent> pendingEvents;

        protected ISerializer serializer;

        //almacena eventos en los pendientes
        public abstract void Send(TrackerEvent trackerEvent);
        //persiste los eventos
        public abstract void Flush();
    }

    public class FilePersistence : IPersistence
    {
        public override void Flush()
        {
            throw new System.NotImplementedException();
        }

        public override void Send(TrackerEvent trackerEvent)
        {
            pendingEvents.Add(trackerEvent);
        }
    }
}
