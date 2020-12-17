using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private Animator titleAnimator;
    private Animator startButtonAnimator;

    public GameObject bannerTitle;
    public GameObject bannerStart;

    private SpriteRenderer titleSR;
    private SpriteRenderer startSR;

    private SoundManager soundManager;

    private void Awake()
    {
        titleSR = bannerTitle.GetComponent<SpriteRenderer>();
        startSR = bannerStart.GetComponent<SpriteRenderer>();

        titleAnimator = bannerTitle.GetComponent<Animator>();
        startButtonAnimator = bannerStart.GetComponent<Animator>();

        soundManager = FindObjectOfType<SoundManager>();
    }

    public void LoadSequence()
    {
        gameObject.GetComponent<PlayerInput>().enabled = false;
        soundManager.Stop("Main Menu");
        titleAnimator.SetTrigger("Fade");
        startButtonAnimator.SetTrigger("Flicker");
        soundManager.Play("Evil Laugh");
        //SceneManager.LoadScene("Room Scene");
    }
}
