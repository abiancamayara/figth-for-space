using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int pontuacaoAtual;

    public string nextSceneName;
    public Transform instaciarboss;
    public static GameManager instance;
    public GameObject painelDeGameOver;
    public GameObject pauseObj;
    
    public GameObject Zarak;
    public GameObject Glaucius;
    public GameObject Dracon;

    public bool gameOver;
    public GameObject bossPrefab; // Prefab do Boss
    public int pontuacaoParaInvocarBoss; // Muda esta variável para definir a quantidade de pontos necessária
        
    private bool bossInstanciado = false;
    private bool isPaused;
    
    
    public TextMeshProUGUI quantidadeCartasText;
    public TextMeshProUGUI contadorLixoText;
    public TextMeshProUGUI pontuacaoAtualText;

    public int Lvalor;

    public VidaDosPlayers vidaDosPlayers;
    
    void Awake()
    {
        instance = this;
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        pontuacaoAtual = 0;
        pontuacaoAtualText.text = "Pontuação: " + pontuacaoAtual;
        
        DontDestroyOnLoad(Dracon);  // Garante que o Dracon não seja destruído ao trocar de cena
        DontDestroyOnLoad(Glaucius); // Faz o mesmo para outros inimigos
        DontDestroyOnLoad(Zarak);
        
    }
    
    void Update()
    {
        PauseGame();
         if (pontuacaoAtual >= pontuacaoParaInvocarBoss && !bossInstanciado)
         {
             InstanciarBoss();
             
         }
    }
    

    public void AumentarPontuacao(int pontosParaGanhar)
    {
        pontuacaoAtual += pontosParaGanhar;
        pontuacaoAtualText.text = "Pontuação: " + pontuacaoAtual;
    }
    
    public void InstanciarBoss()
    {
        // Instancia o Boss
        //Instantiate(bossPrefab, instaciarboss.position, Quaternion.identity); // Ajuste a posição conforme necessário
        //bossInstanciado = true;
        
        
        if (Dracon != null)
        {
            Dracon.SetActive(true); // Ativa o GameObject do inimigo
        }

        // Para os outros inimigos, a lógica seria a mesma
        if (Glaucius != null)
        {
            Glaucius.SetActive(true); // Ativa o segundo inimigo
        }

        if (Zarak != null)
        {
            Zarak.SetActive(true); // Ativa o terceiro inimigo
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
     
        contadorLixoText.text ="Lixo : " + Lvalor.ToString();
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
    }        
    
     public void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            pauseObj.SetActive(isPaused);
        }

        if(isPaused) 
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    } 
}   
