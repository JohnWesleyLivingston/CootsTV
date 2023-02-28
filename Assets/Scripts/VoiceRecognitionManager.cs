using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognitionManager : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public delegate void MeowAction();
    public static event MeowAction OnMeow;

    public delegate void HissAction();
    public static event HissAction OnHiss;

    public delegate void PurrAction();
    public static event PurrAction OnPurr;

    public delegate void PauseAction();
    public static event PauseAction OnPause;

    public delegate void CootsAction();
    public static event CootsAction OnCoots;

    void Start()
    {
        actions.Add("meow", Meow);
       // actions.Add("mew", Meow);
        actions.Add("meo", Meow);
        actions.Add("mehow", Meow);
        actions.Add("meyow", Meow);

        //actions.Add("now", Meow);
        //actions.Add("meeow", Meow);

        /*
        actions.Add("mow", Meow);
        actions.Add("ow", Meow);
        actions.Add("yow", Meow);
        actions.Add("me", Meow);
        actions.Add("moo", Meow);
        actions.Add("owe", Meow);
        actions.Add("ooo", Meow);
        actions.Add("myow", Meow);
        actions.Add("miow", Meow);
        actions.Add("yowe", Meow);
        actions.Add("meyow", Meow);
        */

        actions.Add("Hiss", Hiss);
       // actions.Add("His", Hiss);
      //  actions.Add("Kiss", Hiss);

        /*
        actions.Add("Eee", Hiss);
        actions.Add("He", Hiss);
        actions.Add("Heee", Hiss);
        actions.Add("Hey", Hiss);
        actions.Add("Is", Hiss);
        actions.Add("Iss", Hiss);
        */

        //actions.Add("Purr", Purr);
       // actions.Add("Pur", Purr);
        //actions.Add("Prr", Purr);
        //actions.Add("Pure", Purr);
        actions.Add("Per", Purr);


        actions.Add("Pause Game", Pause);

        actions.Add("Coots", Coots);

        // actions.Add("Paws", Pause);
        // actions.Add("Pos", Pause);



        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }


    void Meow()
    {
        OnMeow();
    }

    void Hiss()
    {
        OnHiss();
    }

    void Purr()
    {
        OnPurr();
    }

    void Pause()
    {
        OnPause();
    }

    void Coots()
    {
        OnCoots();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Meow();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hiss();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Purr();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Coots();
        }
    }



}
