using UnityEngine;
using FMOD.Studio;
using System;

public class EventVariableMixer : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference musicEvent;
    private EventInstance musicEventInstance;
    private static EventVariableMixer instance_;
    public GameObject yesSound, noSound, paperSound, tokenSound;
    public static EventVariableMixer Instance
    {
        get
        {
            if (instance_ == null)
            {
                GameObject singleton = new GameObject("VariableMixerSingleton");
                instance_ = singleton.AddComponent<EventVariableMixer>();
                DontDestroyOnLoad(singleton);
            }
            return instance_;
        }
    }
    private void Awake()
    {
        if (instance_ != null && instance_ != this)
        {
            // Ya hay una instancia existente, destruye esta
            Destroy(gameObject);
        }
        else
        {
            // Establece esta instancia como la instancia única
            instance_ = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);

        musicEventInstance.start();
    }

    void OnDestroy()
    {
        // Detén y libera la instancia del evento al destruir el objeto
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        musicEventInstance.release();
    }
    public void setMusicParameter(string parameterName, float number)
    {
        musicEventInstance.setParameterByName(parameterName, number);


    }
    public float getMusicParameter(string parameterName)
    {
        float number = 0;
        musicEventInstance.getParameterByName(parameterName, out number);
        return number;
    }
}