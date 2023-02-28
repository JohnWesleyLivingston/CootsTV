using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameReactManagerHard : MonoBehaviour
{
    public Animator psychiatristAnim;
    public TextMeshProUGUI subtitles;

    private bool waitingForMeow;
    private bool waitingForHiss;
    private bool waitingForPurr;
    private bool waitingForMeow2;
    private bool waitingForHiss2;
    private bool waitingForPurr2;

    public int gameStep;

    public GameObject tunaImage;
    public GameObject dogImage;
    public GameObject loveImage;
    public GameObject sardineImage;
    public GameObject bathImage;
    public GameObject sleepImage;

    public GameObject tunaImageBlur;
    public GameObject dogImageBlur;
    public GameObject loveImageBlur;
    public GameObject sardineImageBlur;
    public GameObject bathImageBlur;
    public GameObject sleepImageBlur;

    public GameObject imageBG;

    public GameObject fire;
    public GameObject neutralBG;

    public SpriteRenderer psychSpriteRend;
    public Sprite psychHappy;


    public Animator rightAnswerEffect;
    public Animator wrongAnswerEffect;

    public AudioSource music;

    private GameRunner gameRunner;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;
        VoiceRecognitionManager.OnHiss += Hiss;
        VoiceRecognitionManager.OnPurr += Purr;
    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;
        VoiceRecognitionManager.OnHiss -= Hiss;
        VoiceRecognitionManager.OnPurr -= Purr;
    }

    void Start()
    {
        StartCoroutine(StartSequencer());
        gameRunner = FindObjectOfType<GameRunner>();
        imageBG.SetActive(false);
    }

    void Meow()
    {
        if (gameStep == 1 || gameStep == 3 || gameStep == 4 || gameStep == 5)
        {
            FindObjectOfType<AudioManager>().Play("WrongAnswer");
            wrongAnswerEffect.SetTrigger("RightAnswer");
        }

        if (waitingForMeow && gameStep == 0)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForMeow = false;
            gameStep++;

            waitingForHiss = true;

            tunaImage.SetActive(true);

            tunaImageBlur.SetActive(false);
            dogImageBlur.SetActive(false);
            loveImageBlur.SetActive(false);
            sardineImageBlur.SetActive(false);
            bathImageBlur.SetActive(true);
            sleepImageBlur.SetActive(false);
        }

        if (waitingForMeow2 && gameStep == 2)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForMeow2 = false;
            gameStep++;

            waitingForPurr = true;

            sardineImage.SetActive(true);

            tunaImageBlur.SetActive(false);
            dogImageBlur.SetActive(false);
            loveImageBlur.SetActive(false);
            sardineImageBlur.SetActive(false);
            bathImageBlur.SetActive(false);
            sleepImageBlur.SetActive(true);
        }

    }

    void Hiss()
    {
        if (gameStep == 0 || gameStep == 2 || gameStep == 3 || gameStep == 5)
        {
            FindObjectOfType<AudioManager>().Play("WrongAnswer");
            wrongAnswerEffect.SetTrigger("RightAnswer");
        }

        if (waitingForHiss && gameStep == 1)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForHiss = false;
            gameStep++;

            waitingForMeow2 = true;

            bathImage.SetActive(true);

            tunaImageBlur.SetActive(false);
            dogImageBlur.SetActive(false);
            loveImageBlur.SetActive(false);
            sardineImageBlur.SetActive(true);
            bathImageBlur.SetActive(false);
            sleepImageBlur.SetActive(false);
        }

        if (waitingForHiss2 && gameStep == 4)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForHiss2 = false;
            gameStep++;

            waitingForPurr2 = true;

            dogImage.SetActive(true);

            tunaImageBlur.SetActive(false);
            dogImageBlur.SetActive(false);
            loveImageBlur.SetActive(true);
            sardineImageBlur.SetActive(false);
            bathImageBlur.SetActive(false);
            sleepImageBlur.SetActive(false);

        }
    }

    void Purr()
    {
        if (gameStep == 0 || gameStep == 1 || gameStep == 2 || gameStep == 4)
        {
            FindObjectOfType<AudioManager>().Play("WrongAnswer");
            wrongAnswerEffect.SetTrigger("RightAnswer");
        }

        if (waitingForPurr && gameStep == 3)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForPurr = false;
            gameStep++;

            waitingForHiss2 = true;

            sleepImage.SetActive(true);

            tunaImageBlur.SetActive(false);
            dogImageBlur.SetActive(true);
            loveImageBlur.SetActive(false);
            sardineImageBlur.SetActive(false);
            bathImageBlur.SetActive(false);
            sleepImageBlur.SetActive(false);
        }

        if (waitingForPurr2 && gameStep == 5)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForPurr2 = false;
            gameStep++;

            loveImage.SetActive(true);

            tunaImageBlur.SetActive(false);
            dogImageBlur.SetActive(false);
            loveImageBlur.SetActive(false);
            sardineImageBlur.SetActive(false);
            bathImageBlur.SetActive(false);
            sleepImageBlur.SetActive(false);

            StartCoroutine(EndingSequence());
            imageBG.SetActive(false);

        }
    }


    private IEnumerator StartSequencer()
    {
        psychiatristAnim.SetTrigger("Panic");
        FindObjectOfType<AudioManager>().Play("FootstepsLoop");

        yield return new WaitForSeconds(1.5f);
        {

            FindObjectOfType<AudioManager>().Play("React2Line1");
            subtitles.text = "AAAAAAAAA";

            yield return new WaitForSeconds(2.5f);
            {
                //FindObjectOfType<AudioManager>().Play("PauseGame");
                FindObjectOfType<AudioManager>().Play("React2Line2");
                subtitles.text = "I dropped the images and now they're blurry!";

                yield return new WaitForSeconds(4f);
                {
                    waitingForMeow = true;
                    FindObjectOfType<AudioManager>().Play("React2LineHelp");
                    subtitles.text = "Help!";

                    imageBG.SetActive(true);

                    tunaImageBlur.SetActive(true);
                    dogImageBlur.SetActive(false);
                    loveImageBlur.SetActive(false);
                    sardineImageBlur.SetActive(false);
                    bathImageBlur.SetActive(false);
                    sleepImageBlur.SetActive(false);
                }
            }
        }
    }



    private IEnumerator EndingSequence()
    {

        yield return new WaitForSeconds(1.5f);
        {
            fire.SetActive(false);
            neutralBG.SetActive(true);

            yield return new WaitForSeconds(2f);
            {
                FindObjectOfType<AudioManager>().Play("React2Line3");

                FindObjectOfType<AudioManager>().Stop("FootstepsLoop");
                subtitles.text = "You saved the day! <3";
                psychiatristAnim.SetTrigger("Talk");
                psychSpriteRend.sprite = psychHappy;

                yield return new WaitForSeconds(4f);
                {
                    gameRunner.GameComplete();

                }
            }
        }
    }
}
