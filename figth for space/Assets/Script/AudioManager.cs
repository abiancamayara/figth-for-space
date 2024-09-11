using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource, sfxSource;
    public AudioClip clipTiro, clipColetavel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        AudioObserver.PlayMusicEvent += TocarMusica;
        AudioObserver.PlayMusicEvent += PararMusica;
        AudioObserver.PlaySFXEvent += TocarEfeitoSonoro;

    }

    private void OnDisable()
    {
        AudioObserver.PlayMusicEvent -= TocarMusica;
        AudioObserver.PlayMusicEvent -= PararMusica;
        AudioObserver.PlaySFXEvent -= TocarEfeitoSonoro;
    }

    void TocarEfeitoSonoro(string nomeDoClip, float volume)
    {
        switch (nomeDoClip)
        {
            case "tiro" :
                sfxSource.PlayOneShot(clipTiro, volume);
                break;
            case "coletavel":
                sfxSource.PlayOneShot(clipColetavel, volume);            
                break;
            default:
                Debug.LogError($"efeito sonoro {nomeDoClip} não encontrado");
                break;
        }
    }

    void TocarMusica()
    {
        musicSource.Play();
    }
    void PararMusica()
    {
        musicSource.Stop();
    }
}