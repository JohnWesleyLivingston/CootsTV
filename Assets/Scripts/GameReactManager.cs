using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameReactManager : MonoBehaviour
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

    public Animator rightAnswerEffect;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Meow()
    {
        if(waitingForMeow && gameStep == 0)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForMeow = false;
            gameStep++;

            psychiatristAnim.SetTrigger("Talk");
            FindObjectOfType<AudioManager>().Play("ReactLine3");
            subtitles.text = "Clearly say “Hiss” in response to the following image.";
            waitingForHiss = true;

            tunaImage.SetActive(false);
            dogImage.SetActive(true);
            loveImage.SetActive(false);
        }

        if (waitingForMeow2 && gameStep == 5)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForMeow2 = false;

            sardineImage.SetActive(false);

            StartCoroutine(EndingSequence());
        }
    }

    void Hiss()
    {
        if (waitingForHiss && gameStep == 1)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForHiss = false;
            gameStep++;

            psychiatristAnim.SetTrigger("Talk");
            FindObjectOfType<AudioManager>().Play("ReactLine4");
            subtitles.text = "Clearly say “Purr” in response to the following image.";
            waitingForPurr = true;

            tunaImage.SetActive(false);
            dogImage.SetActive(false);
            loveImage.SetActive(true);
        }

        if (waitingForHiss2 && gameStep == 3)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForHiss2 = false;
            gameStep++;

            waitingForPurr2 = true;

            sardineImage.SetActive(false);
            bathImage.SetActive(false);
            sleepImage.SetActive(true);
        }
    }

    void Purr()
    {
        if (waitingForPurr && gameStep == 2)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForPurr = false;
            gameStep++;

            psychiatristAnim.SetTrigger("Talk");
            FindObjectOfType<AudioManager>().Play("ReactLine5");
            subtitles.text = "I think you’re getting the hang of it. Good luck!";

            tunaImage.SetActive(false);
            dogImage.SetActive(false);
            loveImage.SetActive(false);

            StartCoroutine(ExitSequencer());

        }

        if (waitingForPurr2 && gameStep == 4)
        {
            FindObjectOfType<AudioManager>().Play("RightAnswer");
            rightAnswerEffect.SetTrigger("RightAnswer");

            waitingForPurr2 = false;
            gameStep++;

            waitingForMeow2 = true;

            sardineImage.SetActive(true);
            bathImage.SetActive(false);
            sleepImage.SetActive(false);
        }
    }


    private IEnumerator StartSequencer()
    {
        yield return new WaitForSeconds(1.5f);
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");

            FindObjectOfType<AudioManager>().Play("ReactLine1");
            subtitles.text = "Hello. Let’s learn the basics.";

            yield return new WaitForSeconds(4f);
            {
                //FindObjectOfType<AudioManager>().Play("PauseGame");
                psychiatristAnim.SetTrigger("Talk");

                FindObjectOfType<AudioManager>().Play("ReactLine2");
                subtitles.text = "Clearly say “Meow” in response to the following image.";
                waitingForMeow = true;

                tunaImage.SetActive(true);
                dogImage.SetActive(false);
                loveImage.SetActive(false);

            }
        }
    }

    private IEnumerator ExitSequencer()
    {

        yield return new WaitForSeconds(4f);
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");
            psychiatristAnim.SetTrigger("Exit");
            music.Pause();
            yield return new WaitForSeconds(3f);
            {
                music.pitch = 1.2f;
                music.Play();
                subtitles.text = "KEEP GOING!";

                waitingForHiss2 = true;
                sardineImage.SetActive(false);
                bathImage.SetActive(true);
                sleepImage.SetActive(false);
            }
        }
    }

    private IEnumerator EndingSequence()
    {
        psychiatristAnim.SetTrigger("Enter");

        yield return new WaitForSeconds(1.5f);
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");
            //FindObjectOfType<AudioManager>().Play("Line1");

            yield return new WaitForSeconds(2f);
            {
                FindObjectOfType<AudioManager>().Play("ReactLine6");
                subtitles.text = "You did it! You're a genius!";
                psychiatristAnim.SetTrigger("Talk");

                yield return new WaitForSeconds(3.5f);
                {
                    FindObjectOfType<AudioManager>().Play("ReactLine7");
                    subtitles.text = "Remember to say each word clearly, and not too quickly.";
                    psychiatristAnim.SetTrigger("Talk");

                    yield return new WaitForSeconds(5f);
                    {
                        FindObjectOfType<AudioManager>().Play("ReactLine8");
                        subtitles.text = "Don't say Meow, Meow, Meow! Say Meow, PAUSE, meow, PAUSE, meow";
                        psychiatristAnim.SetTrigger("Talk");

                        yield return new WaitForSeconds(6f);
                        {
                            FindObjectOfType<AudioManager>().Play("ReactLine9");
                            subtitles.text = "Good luck! ;)";
                            psychiatristAnim.SetTrigger("Talk");

                            yield return new WaitForSeconds(3f);
                            {
                                gameRunner.GameComplete();

                            }
                        }
                    }
                }
            }
        }
    }
}
