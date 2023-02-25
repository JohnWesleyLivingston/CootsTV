using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlappyCootsManager : MonoBehaviour
{
    public bool isVertcial;
    private bool gameStarted;

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
    public Transform camPos;

    public GameObject endingCoots;
    private bool gameEnd;
    private Transform endingBox;

    public LineRenderer lr;
    public Transform[] points;

    public LineRenderer lr2;
    public Transform[] points2;

    public GameObject balloon;
    public Transform balloonTarget;
    public GameObject flappyCanvas;
    public GameObject balloon2;
    public Transform balloonTarget2;
    public GameObject balloon3;
    public Transform balloonTarget3;
    public GameObject balloon4;
    public Transform balloonTarget4;
    public SpriteRenderer cootsSprite;

    public Transform[] horizontalPos;
    public bool goRight;
    public GameObject helmet;
    public GameObject spaceHelmet;
    public Transform camPosIntro;

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

    public void SetUpLine2(Transform[] points)
    {
        lr2.positionCount = points.Length;
        this.points2 = points;
    }

    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();
        endingCoots.SetActive(false);

        SetUpLine(points);
        SetUpLine2(points2);

        StartCoroutine(StartSequencer());


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



        if(isVertcial && gameStarted)
        {

            for (int i = 0; i < points2.Length; i++)
            {
                lr2.SetPosition(i, points2[i].position);
            }

            balloon2.transform.position = Vector3.Lerp(balloon2.transform.position, balloonTarget2.position, 2f * Time.deltaTime);
            balloon3.transform.position = Vector3.Lerp(balloon3.transform.position, balloonTarget3.position, 2f * Time.deltaTime);
            balloon4.transform.position = Vector3.Lerp(balloon4.transform.position, balloonTarget4.position, 2f * Time.deltaTime);

            if (goRight)
            {
                // gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, horizontalPos[1].transform.position, 8 * Time.deltaTime);
                gameObject.transform.position = transform.position + (Vector3.right * 6) * Time.deltaTime;

               // cootsSprite.transform.Rotate(new Vector3(0, 180, 0));

            }
            else
            {
                //  gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, horizontalPos[0].transform.position, 8 * Time.deltaTime);
                gameObject.transform.position = transform.position + (Vector3.left * 6) * Time.deltaTime;

                //  cootsSprite.transform.Rotate(new Vector3(0, 0, 0));

            }

            if (!endingZoom)
            {
                camPos.position = new Vector3(mainCam.transform.position.x, transform.position.y, camPos.position.z);

                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos.position, 1f * Time.deltaTime);
            }


        }

        if (isVertcial && !gameStarted && !gameEnd)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPosIntro.position, 2f * Time.deltaTime);

        }
    }



    void Meow()
    {
        if (!gameEnd && gameStarted)
        {
            myRigidbody.velocity = Vector2.up * flapStrenght;
        }
    }




    public void addScore(int scoretoAdd)
    {
        playerScore = playerScore + scoretoAdd;
        scoreText.text = playerScore.ToString();
        FindObjectOfType<AudioManager>().Play("RightAnswer");

        if(playerScore == 8)
        {
            pipesSpawnScript.SpawnEndPipe();
            endingBox = GameObject.FindGameObjectWithTag("Top").transform;

        }
    }

    private IEnumerator StartSequencer()
    {
        if (isVertcial)
        {
            FindObjectOfType<AudioManager>().Play("Thrusters");

            yield return new WaitForSeconds(5.5f);
            {
                gameStarted = true;
                myRigidbody.velocity = Vector2.up * flapStrenght * 1.2f;
                FindObjectOfType<AudioManager>().Play("Meow");
                FindObjectOfType<AudioManager>().Stop("Thrusters");
                flappyCanvas.SetActive(true);

            }
        }
        else
        {
            myRigidbody.velocity = Vector2.up * flapStrenght;
            FindObjectOfType<AudioManager>().Play("Meow");
            gameStarted = true;
            flappyCanvas.SetActive(true);
        }

    }


    private IEnumerator EndSequencer()
    {
        gameEnd = true;
        endingZoom = true;
        flappyCanvas.SetActive(false);
        playerScore = 0;
        if (!isVertcial)
        {

            yield return new WaitForSeconds(1f);
            {
                endingCoots.transform.parent = endingBox.transform;
                FindObjectOfType<AudioManager>().Play("Meow");
                endingCoots.SetActive(true);
                cootsSprite.enabled = false;
            }

            yield return new WaitForSeconds(4.5f);
            {
                pipesSpawnScript.DestroyAllBoxes();
                gameRunner.GameComplete();
            }
        }
        if (isVertcial)
        {
            yield return new WaitForSeconds(1f);
            {
                FindObjectOfType<AudioManager>().Play("Meow");
                endingCoots.SetActive(true);
                cootsSprite.enabled = false;
            }
            yield return new WaitForSeconds(4.5f);
            {
                gameRunner.GameComplete();
            }
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
                goRight = true;
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
                goRight = false;
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

        if (target.tag == "Space")
        {
            spaceHelmet.SetActive(true);
            helmet.SetActive(false);
        }

    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Space")
        {
            spaceHelmet.SetActive(false);
            helmet.SetActive(true);
        }


    }
}
