using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    public GameObject thanksForPlaying;
    public GameObject quitOverlay;

    private void Start()
    {
        quitOverlay.SetActive(false);
        thanksForPlaying.SetActive(false);

    }


    public void Quit()
    {
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
        StartCoroutine(QuitGameDelay());
    }

    private IEnumerator QuitGameDelay()
    {
        FindObjectOfType<AudioManager>().Play("PauseGame");
        thanksForPlaying.SetActive(true);

        quitOverlay.SetActive(true);

        yield return new WaitForSeconds(2f);

        {
            FindObjectOfType<AudioManager>().Play("Meow");

            yield return new WaitForSeconds(2f);

            {
                Application.Quit();
                print("Quit Game");

            }

        }

    }
}
