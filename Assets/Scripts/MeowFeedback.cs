using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowFeedback : MonoBehaviour
{
    public ParticleSystem meowParticles1;
    public ParticleSystem meowParticles2;

    private ParticleSystemRenderer meowRenderer1;
    private ParticleSystemRenderer meowRenderer2;

    public Material[] particles;

    public SpriteRenderer cootsOverlay;
    public Color overlayDark;
    public Color overlayTransparent;


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

    private void Start()
    {
        meowRenderer1 = meowParticles1.GetComponent<ParticleSystemRenderer>();
        meowRenderer2 = meowParticles2.GetComponent<ParticleSystemRenderer>();

        cootsOverlay.color = overlayTransparent;
    }

    private void Update()
    {
        if (cootsOverlay.color != overlayTransparent)
        {
            cootsOverlay.color = Color.Lerp(cootsOverlay.color, overlayTransparent, 1.2f * Time.deltaTime);
        }
    }


    void Meow()
    {
        meowRenderer1.material = particles[0];
        meowRenderer2.material = particles[0];

        PlayParticles();

        FindObjectOfType<AudioManager>().PitchedPlay("Meow");
    }

    void Hiss()
    {
        meowRenderer1.material = particles[1];
        meowRenderer2.material = particles[1];

        PlayParticles();

        FindObjectOfType<AudioManager>().PitchedPlay("Hiss");
    }

    void Purr()
    {
        meowRenderer1.material = particles[2];
        meowRenderer2.material = particles[2];

        PlayParticles();

        FindObjectOfType<AudioManager>().PitchedPlay("Purr");
    }

    void PlayParticles()
    {
        cootsOverlay.color = overlayDark;

        meowParticles1.Clear();
        meowParticles2.Clear();

        meowParticles1.Play();
        meowParticles2.Play();
    }

}
