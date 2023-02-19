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

    void Start()
    {
        actions.Add("meow", Meow);
        actions.Add("mew", Meow);
        actions.Add("mow", Meow);
        actions.Add("ow", Meow);
        actions.Add("yow", Meow);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
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
}
