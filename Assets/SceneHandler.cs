using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private Scene currentScene;
    private string sceneName;

    public Animator transition;

    private void Awake()
    {
        transition = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;
    }

    public void ChangeScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }
}
