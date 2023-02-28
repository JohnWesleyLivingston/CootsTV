using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroRunner : MonoBehaviour
{
    public Animator[] creadedBy;
    public Animator[] microphone;
    public Animator pluggedIn;
    public Animator[] yes;
    public Animator loading;
    public Animator microphoneIcon;

    public bool waitForYes;
    public AudioSource micFeedback;
    public AudioSource micInputFeedback;
    public AudioSource micInputFeedback2;

    public Animator camShake;


    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Purr;
        VoiceRecognitionManager.OnCoots += Coots;
        VoiceRecognitionManager.OnCoots += Pause;

    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Purr;
        VoiceRecognitionManager.OnCoots -= Coots;
        VoiceRecognitionManager.OnCoots -= Pause;

    }



    void Start()
    {
        AudioListener.volume = 0.34f;
        StartCoroutine(Intro());
    }

    void Meow()
    { }
    void Purr()
    { }
    void Hiss()
    { }
    void Pause()
    { }

    void Coots()
    {
        if(waitForYes)
        {
            microphoneIcon.SetTrigger("MicInput");

            StartCoroutine(LoadScene());
            camShake.SetTrigger("Shake");
            micInputFeedback.Play();
            micInputFeedback2.Play();
            waitForYes = false;
            //load next scene
        }
    }

    private IEnumerator Intro()
    {
        for (int i = 0; i < creadedBy.Length; i++)
        {
            creadedBy[i].SetBool("TextFadeIn", true);
        }
        yield return new WaitForSeconds(4f);
        {
            for (int i = 0; i < creadedBy.Length; i++)
            {
                creadedBy[i].SetBool("TextFadeIn", false);
            }
        }
        yield return new WaitForSeconds(2f);
        {
            for (int i = 0; i < microphone.Length; i++)
            {
                microphone[i].SetBool("TextFadeIn", true);
            }
            microphoneIcon.SetTrigger("MicRaise");
            micFeedback.Play();
        }
        yield return new WaitForSeconds(3f);
        {
            pluggedIn.SetBool("TextFadeIn", true);
        }
        yield return new WaitForSeconds(3f);
        {
            for (int i = 0; i < yes.Length; i++)
            {
                yes[i].SetBool("TextFadeIn", true);
            }
            waitForYes = true;
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        {
            loading.SetBool("TextFadeIn", true);

            for (int i = 0; i < microphone.Length; i++)
            {
                microphone[i].SetBool("TextFadeIn", false);
            }
            pluggedIn.SetBool("TextFadeIn", false);
            for (int i = 0; i < yes.Length; i++)
            {
                yes[i].SetBool("TextFadeIn", false);
            }
        }


        yield return new WaitForSeconds(1f);
        {
            SceneManager.LoadScene("MainScene");
        }

    }
}
