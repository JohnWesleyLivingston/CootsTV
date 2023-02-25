using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlappyCootsManager : MonoBehaviour
{
    public bool isVertcial;

    private GameRunner gameRunner;
    public Rigidbody2D myRigidbody;

    public float flapStrenght;

    public int playerScore;

    public TextMeshProUGUI scoreText;

    public PipeSpawnScript pipesSpawnScript;
    private bool endingZoom;
    public GameObject mainCam;
    public Transform camPosEnding;
    public Transform endingBoxPos;

    public GameObject endingCoots;
    private bool gameEnd;
    private Transform endingBox;

    public LineRenderer lr;
    public Transform[] points;
    public GameObject balloon;
    public Transform balloonTarget;
    public GameObject flappyCanvas;

    public SpriteRenderer cootsSprite;

    public Transform[] horizontalPos;
    public bool goRight;

    void OnEnable()
    {
        VoiceRecognitionManager.OnMeow += Meow;

    }
    void OnDisable()
    {
        VoiceRecognitionManager.OnMeow -= Meow;

    }

    public void SetUpLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }


    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();
        endingCoots.SetActive(false);

        SetUpLine(points);
    }

    private void Update()
    {
        if (endingZoom)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPosEnding.position, 2f * Time.deltaTime);
            gameObject.transform.position = Vector3.Lerp(transform.position, endingBoxPos.position, 2f * Time.deltaTime);

        }

        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }

        balloon.transform.position = Vector3.Lerp(balloon.transform.position, balloonTarget.position, 2f * Time.deltaTime);


        if(isVertcial)
        {

            if (goRight)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, horizontalPos[1].transform.position, 3 * Time.deltaTime);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, horizontalPos[0].transform.position, 3 * Time.deltaTime);
            }

        }
    }



    void Meow()
    {
        if (!gameEnd)
        {
            myRigidbody.velocity = Vector2.up * flapStrenght;
        }
    }




    public void addScore(int scoretoAdd)
    {
        playerScore = playerScore + scoretoAdd;
        scoreText.text = playerScore.ToString();
        FindObjectOfType<AudioManager>().Play("RightAnswer");

        if(playerScore == 10)
        {
            pipesSpawnScript.SpawnEndPipe();
            endingBox = GameObject.FindGameObjectWithTag("Top").transform;

        }
    }



    private IEnumerator EndSequencer()
    {

        gameEnd = true;
        endingZoom = true;
        flappyCanvas.SetActive(false);
        yield return new WaitForSeconds(1f);
        {
            endingCoots.transform.parent = endingBox.transform;
            FindObjectOfType<AudioManager>().Play("Meow");
            endingCoots.SetActive(true);
            cootsSprite.enabled = false;
        }
        yield return new WaitForSeconds(1.5f);
        {
            FindObjectOfType<AudioManager>().Play("Meow");
        }

        yield return new WaitForSeconds(4f);
        {

            pipesSpawnScript.DestroyAllBoxes();
            gameRunner.GameComplete();

        }

    }

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.tag == "VespaLeft")
        {
            if (!isVertcial)
            {
                // addVelocity = true;
                transform.position += new Vector3(0.1f, 0, 0);
            }
            else
            {
                goRight = false;
            }
        }
        if (target.tag == "VespaRight")
        {
            if (!isVertcial)
            {
                // addVelocity = true;
                transform.position += new Vector3(-0.1f, 0, 0);
            }
            else
            {
                goRight = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Blocker")
        {
            addScore(1);
            target.GetComponent<Collider2D>().gameObject.SetActive(false);
        }

        if (target.tag == "VespaMid")
        {
            print("Hit tower");
            int r = Random.Range(0, 3);
            FindObjectOfType<AudioManager>().Play("Cardboard_" + r);

        }

        if (target.tag == "Finish")
        {
            StartCoroutine(EndSequencer());

        }

    }

}
