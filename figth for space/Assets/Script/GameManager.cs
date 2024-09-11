using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject painelDeGameOver;
    public bool gameOver;
    public TextMeshProUGUI quantidadeCartasText;
    public TextMeshProUGUI contadorLixoText;

    public int Lvalor;

    public VidaDosPlayers vidaDosPlayers;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
