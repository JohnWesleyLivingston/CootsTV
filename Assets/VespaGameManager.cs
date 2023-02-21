using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VespaGameManager : MonoBehaviour
{
    public GameObject vespa;
    public Animator vespaAnim;

    public Transform leftPos;
    public Transform midPos;
    public Transform rightPos;

    private bool isLeft;
    private bool isMiddle;
    private bool isRight;

    public bool isMoving;

    private bool wasRight;
    private bool wasLeft;

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
    }


    void Update()
    {
        if(isLeft)
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
    }

    void Meow()
    {
        isLeft = true;
        isMiddle = false;
        isRight = false;

        isMoving = true;

        wasRight = true;
        wasLeft = false;

    }
    void Hiss()
    {
        isLeft = false;
        isMiddle = true;
        isRight = false;

        isMoving = true;

    }
    void Purr()
    {
        isLeft = false;
        isMiddle = false;
        isRight = true;

        isMoving = true;

        wasRight = false;
        wasLeft = true;
    }


    void MoveLeft()
    {
        vespa.transform.position = Vector3.Lerp(vespa.transform.position, leftPos.position, 2f * Time.deltaTime);

        if(isMoving)
        {
            vespaAnim.SetBool("Left", true);
        }


    }
    void MoveMiddle()
    {
        vespa.transform.position = Vector3.Lerp(vespa.transform.position, midPos.position, 2f * Time.deltaTime);

        if (isMoving && wasRight)
        {
            vespaAnim.SetBool("Right", true);
        }
        if (isMoving && wasLeft)
        {
            vespaAnim.SetBool("Left", true);
        }
    }
    void MoveRight()
    {
        vespa.transform.position = Vector3.Lerp(vespa.transform.position, rightPos.position, 2f * Time.deltaTime);

        if (isMoving)
        {
            vespaAnim.SetBool("Right", true);
        }

    }


    private IEnumerator StartSequencer()
    {
        yield return new WaitForSeconds(2.5f);
        {
            FindObjectOfType<AudioManager>().Play("VespaMotor");
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "VespaLeft")
        {
            vespaAnim.SetBool("Left", false);
            isMoving = false;
        }
        if (target.tag == "VespaMid")
        {
            vespaAnim.SetBool("Left", false);
            vespaAnim.SetBool("Right", false);
            isMoving = false;
        }
        if (target.tag == "VespaRight")
        {
            vespaAnim.SetBool("Right", false);
            isMoving = false;
        }
    }
}
