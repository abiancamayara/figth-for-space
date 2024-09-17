using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int pontuacaoAtual;

    public Transform instaciarboss;
    public static GameManager instance;
    public GameObject painelDeGameOver;
    
    public bool gameOver;
    public GameObject bossPrefab; // Prefab do Boss
    public int pontuacaoParaInvocarBoss; // Muda esta variável para definir a quantidade de pontos necessária
    
    private bool bossInstanciado = false;
    
    public TextMeshProUGUI quantidadeCartasText;
    public TextMeshProUGUI contadorLixoText;
    public TextMeshProUGUI pontuacaoAtualText;

    public int Lvalor;

    public VidaDosPlayers vidaDosPlayers;

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        if (pontuacaoAtual >= pontuacaoParaInvocarBoss && !bossInstanciado)
        {
            InstanciarBoss();
        }
    }

    public void AumentarPontuaao(int pontosParaGanhar)
    {
        pontuacaoAtual += pontosParaGanhar;
        pontuacaoAtualText.text = "Pontuação: " + pontuacaoAtual;
    }
    
    public void InstanciarBoss()
    {
        // Instancia o Boss
        Instantiate(bossPrefab, instaciarboss.position, Quaternion.identity); // Ajuste a posição conforme necessário
        bossInstanciado = true;
        
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
        contadorLixoText.text ="Lixo: " + Lvalor.ToString();
    }
    public void GameOver()
    {
        AudioObserver.OnStopMusicEvent();
        gameOver = true;
        painelDeGameOver.SetActive(true);
    }
}
