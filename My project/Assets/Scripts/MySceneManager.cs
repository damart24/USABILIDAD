using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    // Singleton instance
    private static MySceneManager _instance;
    public static MySceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new GameObject("MySceneManagerSingleton");
                _instance = singleton.AddComponent<MySceneManager>();
                DontDestroyOnLoad(singleton);
            }
            return _instance;
        }
    }

    // M�todo para cargar una escena por su nombre
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // M�todo para salir de la aplicaci�n (solo funciona en builds, no en el Editor)
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Este m�todo asegura que solo haya una instancia en ejecuci�n
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
