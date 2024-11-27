using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource, sfxSource;
    public AudioClip clipTiro, clipColetavel;
    public AudioClip[] musicClips; // Array para armazenar músicas por cena

    private void Awake()
    {
        // Implementação do Singleton para garantir que haja apenas uma instância do AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Faz com que o AudioManager não seja destruído ao trocar de cena
        }
        else
        {
            Destroy(gameObject); // Destroi a instância duplicada
        }
    }

    private void OnEnable()
    {
        // Inscreve-se nos eventos do AudioObserver para tocar música e efeitos sonoros
        AudioObserver.PlayMusicEvent += TocarMusica;
        AudioObserver.PlayMusicEvent += PararMusica;
        AudioObserver.PlaySFXEvent += TocarEfeitoSonoro;

        // Adiciona um ouvinte para o evento de carregamento de cena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Desinscreve-se dos eventos ao desativar o script
        AudioObserver.PlayMusicEvent -= TocarMusica;
        AudioObserver.PlayMusicEvent -= PararMusica;
        AudioObserver.PlaySFXEvent -= TocarEfeitoSonoro;

        // Remove o ouvinte para o evento de carregamento de cena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Inicialmente, não faz nada, pois a música será carregada na troca de cena
    }

    // Método chamado quando uma cena é carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ao carregar a nova cena, mude a música
        TrocarMusicaPorCena(scene.buildIndex);
    }

    // Método para trocar a música dependendo da cena
    void TrocarMusicaPorCena(int sceneIndex)
    {
        // Verifica se há uma música para a cena atual no array
        if (sceneIndex < musicClips.Length && musicClips[sceneIndex] != null)
        {
            musicSource.clip = musicClips[sceneIndex]; // Atribui a música da cena
            musicSource.Play(); // Começa a tocar
        }
        else
        {
            Debug.LogWarning("Nenhuma música atribuída para esta cena ou índice inválido.");
        }
    }

    void TocarEfeitoSonoro(string nomeDoClip, float volume)
    {
        // Toca o efeito sonoro baseado no nome do clip
        switch (nomeDoClip)
        {
            case "tiro":
                if (clipTiro != null)
                {
                    sfxSource.PlayOneShot(clipTiro, volume);
                }
                else
                {
                    Debug.LogWarning("Clip de tiro não atribuído!");
                }
                break;

            case "coletavel":
                if (clipColetavel != null)
                {
                    sfxSource.PlayOneShot(clipColetavel, volume);
                }
                else
                {
                    Debug.LogWarning("Clip de coletável não atribuído!");
                }
                break;

            default:
                Debug.LogError($"Efeito sonoro {nomeDoClip} não encontrado.");
                break;
        }
    }

    void TocarMusica()
    {
        if (musicSource.clip != null)
        {
            musicSource.Play(); // Toca a música, se o clip estiver atribuído
        }
        else
        {
            Debug.LogWarning("Nenhuma música atribuída para tocar.");
        }
    }

    void PararMusica()
    {
        musicSource.Stop(); // Para a música
    }
}
