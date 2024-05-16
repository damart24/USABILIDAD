using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L10nManager : MonoBehaviour
{
    public enum Language { Spanish, Japanese }
    [SerializeField]
    private Language languageUsed;
    private static L10nManager instance_;
    private void Awake()
    {
        if (instance_ == null){
            instance_ = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
    public static L10nManager Instance
    {
        get => instance_;
    }
    public Language GetLanguage(){
        return languageUsed;
    }
}
