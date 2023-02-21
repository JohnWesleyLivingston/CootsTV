using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CootsScreenManager : MonoBehaviour
{
    private PauseGame pauseMenu;

    public Transform cootsOnTV;
    public Transform cootsInFrontTV;

    public GameObject cootsObject;

    public Animator cootsAnim;
    private bool gameStartedToggle = false;

    private int cootsInteruptDelay;
    public Animator cootsInterupt;

    public PostProcessVolume postProcessVolume;
    private float depthOfFieldValue;
    public bool lowerDoF;
    public bool raiseDoF;
    public float lowerDoFValue = 1.25f;
    public float raiseDoFValue = 2.85f;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseGame>();
        cootsObject.transform.position = cootsOnTV.transform.position;
        raiseDoF = true;

    }


    void Update()
    {
        if (pauseMenu.gameStarted)
        {
            if (!gameStartedToggle)
            {
                StartCoroutine(CootsMoveDelay());
                gameStartedToggle = true;
            }
        }
        else
        {
            cootsObject.transform.position = cootsOnTV.transform.position;
            cootsAnim.SetBool("CootsMeow", false);
        }


        if (raiseDoF)
        {
            RaiseDoF();
        }

        if (lowerDoF)
        {
            LowerDoF();
        }

    }

    private IEnumerator CootsMoveDelay()
    {

        yield return new WaitForSeconds(2f);

        {
            cootsObject.transform.position = cootsInFrontTV.transform.position;
            cootsAnim.SetBool("CootsMeow", true);
            CootsInterupt();
        }
    }

    void CootsInterupt()
    {
        if (pauseMenu.gameStarted)
        {
            cootsInteruptDelay = Random.Range(25, 120);
            StartCoroutine(CootsInteruptDelay());
        }

    }

    private IEnumerator CootsInteruptDelay()
    {
        yield return new WaitForSeconds(cootsInteruptDelay);
        {
            cootsInterupt.SetBool("RaiseCoots", true);
            lowerDoF = true;

            yield return new WaitForSeconds(5f);
            {
                cootsInterupt.SetBool("RaiseCoots", false);
                CootsInterupt();
                raiseDoF = true;
            }
        }
    }

    public void RaiseDoF()
    {
        if (depthOfFieldValue <= raiseDoFValue)
        {
            depthOfFieldValue = depthOfFieldValue + 0.05f;

        }

        if (postProcessVolume)
        {
            DepthOfField pr;
            if (postProcessVolume.sharedProfile.TryGetSettings<DepthOfField>(out pr))
            {
                //Debug.Log(pr.focusDistance.value);
                pr.focusDistance.value = depthOfFieldValue;
            }
        }

        if (depthOfFieldValue >= raiseDoFValue - 0.05f)
        {
            raiseDoF = false;
        }
    }

    public void LowerDoF()
    {
        if (depthOfFieldValue >= lowerDoFValue)
        {
            depthOfFieldValue = depthOfFieldValue - 0.05f;
        }

        if (postProcessVolume)
        {
            DepthOfField pr;
            if (postProcessVolume.sharedProfile.TryGetSettings<DepthOfField>(out pr))
            {
                pr.focusDistance.value = depthOfFieldValue;
            }
        }


        if (depthOfFieldValue <= lowerDoFValue + 0.05f)
        {
            lowerDoF = false;
        }
    }

}
