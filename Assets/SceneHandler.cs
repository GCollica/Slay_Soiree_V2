using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private Scene currentScene;
    private string sceneName;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;
    }

    public void ChangeScene()
    {
        if (sceneName == "Room Scene")
        {
            SceneManager.LoadScene("Shop Scene");
        }

        if (sceneName == "Shop Scene")
        {
            SceneManager.LoadScene("Boss Scene");
        }

        if (sceneName == "Boss Scene")
        {
            SceneManager.LoadScene("Room Scene");
        }
    }

}
