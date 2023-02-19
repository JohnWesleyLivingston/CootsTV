using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    private float pitchAdjust;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;

            s.source.enabled = false;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("!!No Audio Found!!");
            return;
        }
        s.source.enabled = true;
        s.source.Play();
    }


    public void PitchedPlay(string name)
    {
        pitchAdjust = UnityEngine.Random.Range(0.9f, 1.1f);
        //print(pitchAdjust);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("!!No Audio Found!!");
            return;
        }
        s.source.enabled = true;
        s.source.pitch = pitchAdjust;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("!!No Audio Found!!");
            return;
        }
        s.source.Stop();
        s.source.enabled = false;
    }
}

