using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Los comentarios está para que compile y no pete.

public interface ISerializer

    public string serialize(/*TrackerEvent event*/);
}


public class JsonSerializer : ISerializer
{
    public string serialize(/*TrackerEvent event*/)
    {
        /*return event.ToJson();*/
    }
}
