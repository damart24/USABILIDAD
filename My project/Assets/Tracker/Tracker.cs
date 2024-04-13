using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tracker
{
    // Comentado para evitar errores de compilacion
    //private List<ITrackerAsset> activeTrackers;
    //private IPersistence persistenceObject;

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
        
    }

    public void TrackEvent() 
    {

    }
}
