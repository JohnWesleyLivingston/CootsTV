using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuitarHeroManager2 : MonoBehaviour
{
    [Header("Guitar Pick")]
    public GameObject guitarPick;
    public Animator guitarPickAnim;
    public Animator guitarPickAnimStrum;
    public ParticleSystem fireEffect;

    [Header("Move Transforms")]
    public Transform leftPos;
    public Transform midPos;
    public Transform rightPos;

    public bool isLeft;
    public bool isMiddle = true;
    public bool isRight;
    private bool isMoving;
    private bool wasRight;
    private bool wasLeft;

    [Header("Camera")]
    public Camera mainCam;
    public Transform camPos;
    private bool ending;

    [Header("Music")]
    public AudioSource backTrack;

    [Header("Note Anims")]
    public Animator redAnimLeft;
    public Animator greenAnimMid;
    public Animator blueAnimRight;

    [Header("Other")]
    public GameObject guitarCanvas;
    private GameRunner gameRunner;
    public int missedNotes;
    public Animator cootsSlash1;
    public Animator cootsSlash2;
    public GameObject endingFire;
    private GameObject[] guitarTiles;
    public TextMeshProUGUI scoreText;
    public int score;
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


    void Update()
    {
        if (isLeft)
        {
            MoveLeft();
        }
        if (isMiddle)
        {
            MoveMiddle();
        }
        if (isRight)
        {
            MoveRight();
        }

        scoreText.text = score.ToString();
    }
    void Hiss()
    {
        if (!ending)
        {
            isLeft = true;
            isMiddle = false;
            isRight = false;

            isMoving = true;

            wasRight = true;
            wasLeft = false;
        }


    }
    void Meow()
    {
        if (!ending)
        {
            isLeft = false;
            isMiddle = true;
            isRight = false;

            isMoving = true;
        }
    }

    void Purr()
    {
        if (!ending)
        {
            isLeft = false;
            isMiddle = false;
            isRight = true;

            isMoving = true;

            wasRight = false;
            wasLeft = true;
        }
    }


    void MoveLeft()
    {
        guitarPick.transform.position = Vector3.Lerp(guitarPick.transform.position, leftPos.position, 8f * Time.deltaTime);

        if (isMoving)
        {
            guitarPickAnim.SetBool("Left", true);
        }


    }
    void MoveMiddle()
    {
        guitarPick.transform.position = Vector3.Lerp(guitarPick.transform.position, midPos.position, 8f * Time.deltaTime);

        if (isMoving && wasRight)
        {
            guitarPickAnim.SetBool("Right", true);
        }
        if (isMoving && wasLeft)
        {
            guitarPickAnim.SetBool("Left", true);
        }
    }
    void MoveRight()
    {
        guitarPick.transform.position = Vector3.Lerp(guitarPick.transform.position, rightPos.position, 8f * Time.deltaTime);

        if (isMoving)
        {
            guitarPickAnim.SetBool("Right", true);
        }

    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "VespaLeft")
        {
            guitarPickAnim.SetBool("Left", false);
            isMoving = false;
        }
        if (target.tag == "VespaMid")
        {
            guitarPickAnim.SetBool("Left", false);
            guitarPickAnim.SetBool("Right", false);
            isMoving = false;
        }
        if (target.tag == "VespaRight")
        {
            guitarPickAnim.SetBool("Right", false);
            isMoving = false;
        }

        if (target.tag == "Blocker")
        {
            // string soundToPlay = target.GetComponent<GuitarNote>().musicalNote;
            // FindObjectOfType<AudioManager>().Play(soundToPlay);
            target.GetComponent<GuitarNote>().KillMeImmediate();
            HitNote();
        }

        if (target.tag == "Ending")
        {
            StopAllCoroutines();
            StartCoroutine(EndingSequencer());
            guitarCanvas.SetActive(false);
        }
    }


    void HitNote()
    {
        guitarPickAnimStrum.SetTrigger("HitNote");
        FindObjectOfType<AudioManager>().PitchedPlay("HitNote");
        fireEffect.Play();

        missedNotes = 0;

        if (isLeft) { redAnimLeft.SetTrigger("NoteHit"); }
        if (isMiddle) { greenAnimMid.SetTrigger("NoteHit"); }
        if (isRight) { blueAnimRight.SetTrigger("NoteHit"); }


        int adlibToPlay = Random.Range(0, 9);
        FindObjectOfType<AudioManager>().Play("RapAdlib_" + adlibToPlay);

        score = score + 50;
    }

    public void MissNote()
    {
        guitarPickAnimStrum.SetTrigger("MissNote");


        FindObjectOfType<AudioManager>().PitchedPlay("MissNoteRap" );

        missedNotes++;

        if (missedNotes >= 2)
        {
            FindObjectOfType<AudioManager>().PitchedPlay("Boo");
        }
    }





    private IEnumerator StartSequencer()
    {
        yield return new WaitForSeconds(9f);
        {
            backTrack.Play();
            guitarCanvas.SetActive(true);
            cootsSlash1.SetBool("PlayMusic", true);
            cootsSlash2.SetBool("PlayMusic", true);
            print("Music ON");
        }
    }






    private IEnumerator EndingSequencer()
    {
        endingFire.SetActive(true);
        cootsSlash1.SetTrigger("Ending");
        cootsSlash2.SetTrigger("Ending");
        backTrack.Stop();

        yield return new WaitForSeconds(6f);
        {
            guitarTiles = GameObject.FindGameObjectsWithTag("GuitarNeck");
            for (int i = 0; i < guitarTiles.Length; i++)
            {
                Destroy(guitarTiles[i]);
            }

            gameRunner.GameComplete();
        }
    }

}