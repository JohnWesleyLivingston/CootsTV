using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchOutPlayer : MonoBehaviour
{
    [Header ("Game Settings")]
    private GameRunner gameRunner;

    [Header("Player")]
    public GameObject player;
    public Animator playerAnim;
    public Collider playerCol;

    [Header("Transforms")]
    public Transform leftPos;
    public Transform midPos;
    public Transform rightPos;
    public Transform punchPos;

    private Transform currentPos;

    [Header("Bools")]
    public bool gameStart;
    public bool gameEnd;

    public bool dodgeLeft;
    public bool dodgeRight;
    public bool punch;

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
        gameRunner = FindObjectOfType<GameRunner>();
        StartCoroutine(StartSequencer());
        currentPos = midPos;
       // StartCoroutine(EndSequencer());

    }

    void Update()
    {
        if(gameStart && !gameEnd)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, currentPos.position, 3f * Time.deltaTime);

        }
    }

    void Meow()
    {
        if (!dodgeLeft && !dodgeRight && !punch)
        {
            StartCoroutine(Punch());
        }
    }
    void Hiss()
    {
        if (!dodgeLeft && !dodgeRight && !punch)
        {
            StartCoroutine(DodgeLeft());
        }
    }
    void Purr()
    {
        if (!dodgeLeft && !dodgeRight && !punch)
        {
            StartCoroutine(DodgeRight());
        }
    }

    private IEnumerator Punch()
    {
        punch = true;
        currentPos = punchPos;
        playerAnim.SetBool("Punch", true);
        yield return new WaitForSeconds(1f);
        {
            currentPos = midPos;
            playerAnim.SetBool("Punch", false);
            punch = false;
        }
    }

    private IEnumerator DodgeLeft()
    {
        dodgeLeft = true;
        currentPos = leftPos;
        playerAnim.SetBool("DodgeLeft", true);
        playerCol.enabled = false;

        yield return new WaitForSeconds(3f);
        {
            currentPos = midPos;
            playerAnim.SetBool("DodgeLeft", false);
            playerCol.enabled = true;
            dodgeLeft = false;
        }
    }

    private IEnumerator DodgeRight()
    {
        dodgeRight = true;
        currentPos = rightPos;
        playerAnim.SetBool("DodgeRight", true);
        playerCol.enabled = false;

        yield return new WaitForSeconds(3f);
        {
            currentPos = midPos;
            playerAnim.SetBool("DodgeRight", false);
            playerCol.enabled = true;
            dodgeRight = false;

        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Blocker")
        {
            print("Take Damage");
        }


    }





    private IEnumerator StartSequencer()
    {
        //Intro cutscene

        yield return new WaitForSeconds(1f);
        {
            gameStart = true;
        }

    }


    private IEnumerator EndSequencer()
    {
        gameEnd = true;

        yield return new WaitForSeconds(1f);
        {
            gameRunner.GameComplete();
        }

    }



}
