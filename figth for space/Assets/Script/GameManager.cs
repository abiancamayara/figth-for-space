using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float pontuacaoAtual;
    public string nextSceneName;
    public Transform instaciarboss;
    public static GameManager instance;
    public GameObject painelDeGameOver;
    public GameObject pauseObj;

    public GameObject Zarak;
    public GameObject Glaucius;
    public GameObject Dracon;
    public GameObject Carta;

    public bool gameOver;
    public int pontuacaoParaInvocarDracon; // Pontuação para invocar Dracon
    public int pontuacaoParaInvocarGlaucius; // Pontuação para invocar Glaucius
    public int pontuacaoParaInvocarZarak; // Pontuação para invocar Zarak
    public int pontuacaoParaInvocarCarta;

    private bool bossInstanciadoDracon = false;
    private bool bossInstanciadoGlaucius = false;
    private bool bossInstanciadoZarak = false;
    private bool cartaInstanciado = false;
    private bool isPaused;

    public TextMeshProUGUI quantidadeCartasText;
    public TextMeshProUGUI contadorLixoText;
    public TextMeshProUGUI pontuacaoAtualText;

    public GameObject lixoEspacial;
    public GameObject carta;

    public int Lvalor;

    public VidaDosPlayers vidaDosPlayers;

    void Awake()
    {
        instance = this;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        pontuacaoAtual = 0;
        pontuacaoAtualText.text = "Pontuação: " + pontuacaoAtual; // Atualiza a UI no início
        Debug.Log("Pontuação Inicial: " + pontuacaoAtual); // Debug para verificar o valor inicial
    }

    void Update()
    {
        // Debug para verificar a pontuação durante a execução
        Debug.Log("Pontuação Atual: " + pontuacaoAtual);

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        // Verifica se a pontuação atingiu o valor para invocar os bosses, mas garante que não sejam ativados mais de uma vez.
        if (pontuacaoAtual >= pontuacaoParaInvocarDracon && !bossInstanciadoDracon)
        {
            AtivarDracon();
        }

        if (pontuacaoAtual >= pontuacaoParaInvocarGlaucius && !bossInstanciadoGlaucius)
        {
            AtivarGlaucius();
        }

        if (pontuacaoAtual >= pontuacaoParaInvocarZarak && !bossInstanciadoZarak)
        {
            AtivarZarak();
        }
        
        if (pontuacaoAtual >= pontuacaoParaInvocarCarta && !cartaInstanciado)
        {
            AtivarCarta();
        }
    }

    public void AumentarPontuacao(int pontosParaGanhar)
    {
        pontuacaoAtual += pontosParaGanhar; // Aumenta a pontuação
        Debug.Log("Pontuação após aumento: " + pontuacaoAtual); // Debug para verificar o aumento
        pontuacaoAtualText.text = "Pontuação: " + pontuacaoAtual; // Atualiza a UI
    }

    public void AtivarDracon()
    {
        // Ativa o Boss Dracon
        if (Dracon != null)
        {
            Dracon.SetActive(true); // Ativa o GameObject do inimigo Dracon
            bossInstanciadoDracon = true;
        }
    }

    public void AtivarGlaucius()
    {
        // Ativa o Boss Glaucius
        if (Glaucius != null)
        {
            Glaucius.SetActive(true); // Ativa o GameObject do inimigo Glaucius
            bossInstanciadoGlaucius = true;
        }
    }

    public void AtivarZarak()
    {
        // Ativa o Boss Zarak
        if (Zarak != null)
        {
            Zarak.SetActive(true); // Ativa o GameObject do inimigo Zarak
            bossInstanciadoZarak = true;
        }
    }
    
    public void AtivarCarta()
    {
        // Ativa o Boss Zarak
        if (Carta != null)
        {
            Carta.SetActive(true); // Ativa o GameObject do inimigo Zarak
            cartaInstanciado = true;
        }
    }
    

    public void AtualizarQuantidadeCartasUI(int value)
    {
        quantidadeCartasText.text = "Carta: " + value.ToString();
    }

    public void ColetarLixo(int LixoV)
    {
        Lvalor += LixoV;
        // Verifica se coletou 5 lixos
        if (Lvalor >= 5)
        {
            int vidaParaReceber = Mathf.CeilToInt(vidaDosPlayers.vidaMaximaDoJogador * 0.2f); // 20% da vida máxima
            vidaDosPlayers.GanharVida(vidaParaReceber);
            Lvalor = 0; // Reinicia o contador após o aumento de vida
        }

        contadorLixoText.text = "Lixo : " + Lvalor.ToString();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Carta"))
        {
            PassarDeFase();
        }
    }

    public void PassarDeFase()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void GameOver()
    {
        AudioObserver.OnStopMusicEvent();
        gameOver = true;
        painelDeGameOver.SetActive(true);

        // Desligando textos e imagens da tela para o game over
        quantidadeCartasText.gameObject.SetActive(false);
        contadorLixoText.gameObject.SetActive(false);
        carta.SetActive(false);
        lixoEspacial.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            pauseObj.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseObj.SetActive(false);
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
