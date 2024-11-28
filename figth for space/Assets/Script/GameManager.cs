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

    public GameObject Coracao;
    public GameObject Escudo;
    public GameObject ButtonPause;
    public GameObject lixoEspacial;
    public GameObject carta;

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
    public TextMeshProUGUI metaDePontuaçãoText;

    public Image retangulo;

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
        Lvalor = 0;
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
        //Debug.Log("Pontuação após aumento: " + pontuacaoAtual); // Debug para verificar o aumento
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
        Debug.Log("Coletando lixo com valor: " + LixoV);  // Verifique se o valor de LixoV está correto

        // Verifique se o valor de LixoV é maior que 0
        if (LixoV > 0)
        {
            Lvalor += LixoV;
            Debug.Log("Lixo somado. Lvalor agora é: " + Lvalor);  // Verifique se o Lvalor está sendo somado corretamente
        }

        // Verifica se coletou 5 ou mais lixos
        if (Lvalor >= 5)
        {
            int vidaParaReceber = Mathf.CeilToInt(vidaDosPlayers.vidaMaximaDoJogador * 0.2f); // 20% da vida máxima
            vidaDosPlayers.GanharVida(vidaParaReceber);  // Atualiza a vida do jogador
            Debug.Log("Vida aumentada em: " + vidaParaReceber);  // Exibe no console a quantidade de vida recebida
            Lvalor = 0; // Reseta o contador de lixo após o aumento de vida
        }

        // Atualiza a UI com o valor atual de lixo
        contadorLixoText.text = "Lixo espacial: " + Lvalor.ToString();  // Atualiza a UI com a quantidade de lixo coletado
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
        // Carregar a próxima cena pelo índice
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Verifica se a próxima cena existe
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Não há mais cenas no Build");
        }
    }


    public void GameOver()
    {
        AudioObserver.OnStopMusicEvent();
        gameOver = true;
        painelDeGameOver.SetActive(true);

        // Desligando textos e imagens da tela para o game over
        quantidadeCartasText.gameObject.SetActive(false);
        contadorLixoText.gameObject.SetActive(false);
        pontuacaoAtualText.gameObject.SetActive(false);
        metaDePontuaçãoText.gameObject.SetActive(false);
        carta.SetActive(false);
        lixoEspacial.SetActive(false);
        Coracao.SetActive(false);
        Escudo.SetActive(false);
        ButtonPause.SetActive(false);
        retangulo.gameObject.SetActive(false);
        
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
            Coracao.SetActive(false);
            retangulo.gameObject.SetActive(false);
            metaDePontuaçãoText.gameObject.SetActive(false);
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
