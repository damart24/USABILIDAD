using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private string filePath;
        private string fileName;
        public FilePersistence()
        {
            serializer = new JsonSerializer();

            filePath = Path.Combine(Application.persistentDataPath, fileName);

            fileName = "ArchivoGuardado";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            pendingEvents = new List<TrackerEvent>();
        }
        public override void Flush()
        {
            List<string> jsonEvents = new List<string>();

            foreach (TrackerEvent trackerEvent in pendingEvents)
            {
                jsonEvents.Add(serializer.serialize(trackerEvent));
            }

            File.WriteAllLines(filePath, jsonEvents.ToArray());      
        }

        public override void Send(TrackerEvent trackerEvent)
        {
            pendingEvents.Add(trackerEvent);
        }
    }
}
