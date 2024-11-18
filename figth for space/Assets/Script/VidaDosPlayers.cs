using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaDosPlayers : MonoBehaviour
{
    public Slider barraDeVidaDoJogador;
    public Slider barraDeEnergiaDoEscudo;
    public GameObject escudoDoJogador;

    public int vidaMaximaDoJogador;
    public int vidaAtualDojogador;
    public int vidaMaximaDoEscudo;
    public int vidaAtualDoEscudo;

    public bool temEscudo;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.vidaDosPlayers = this;
        vidaAtualDojogador = vidaMaximaDoJogador;
        vidaAtualDoEscudo = vidaMaximaDoEscudo;

        barraDeVidaDoJogador.maxValue = vidaMaximaDoJogador;
        barraDeVidaDoJogador.value = vidaAtualDojogador;

        barraDeEnergiaDoEscudo.maxValue = vidaMaximaDoEscudo;
        barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;

        barraDeEnergiaDoEscudo.gameObject.SetActive(false);
        temEscudo = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AtivarEscudo()
    {
        barraDeEnergiaDoEscudo.gameObject.SetActive(true);
        vidaAtualDoEscudo = vidaMaximaDoEscudo;
        barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;
        escudoDoJogador.SetActive(true);
        temEscudo = true;
    }

    public void DesativarEscudo()
    {
        escudoDoJogador.SetActive(false);
        temEscudo = false;
    }

    public void GanharVida(int vidaParaReceber)
    {
        if (vidaAtualDojogador + vidaParaReceber <= vidaMaximaDoJogador)
        {
            vidaAtualDojogador += vidaParaReceber;
        }
        else
        {
            vidaAtualDojogador = vidaMaximaDoJogador;
        }

        barraDeVidaDoJogador.value = vidaAtualDojogador;
    }
    
    public void MachucarJogador(int danoParaReceber)
    {
        if (temEscudo)
        {
            // Primeiro aplica o dano ao escudo
            vidaAtualDoEscudo -= danoParaReceber;
            barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;

            // Se a vida do escudo for menor ou igual a 0, desativa o escudo
            if (vidaAtualDoEscudo <= 0)
            {
                DesativarEscudo();
                // Se sobrar dano, aplica ao jogador
                int danoRestante = Mathf.Abs(vidaAtualDoEscudo);  // danoRestante é o valor absoluto da vida negativa do escudo
                vidaAtualDojogador -= danoRestante;
                barraDeVidaDoJogador.value = vidaAtualDojogador;
            }
        }
        else
        {
            // Aplica o dano ao jogador se não há escudo
            vidaAtualDojogador -= danoParaReceber;
            barraDeVidaDoJogador.value = vidaAtualDojogador;
        }

        // Verifica se a vida do jogador é menor ou igual a 0
        if (vidaAtualDojogador <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.GameOver();
            Debug.Log("Game Over");
        }
        else
        {
            // Chama o método para iniciar a animação de dano nos três jogadores
            PlayerLuna luna = GetComponent<PlayerLuna>();
            PlayerLuca luca = GetComponent<PlayerLuca>();
            PlayerRuby ruby = GetComponent<PlayerRuby>();

            if (luna != null)
                luna.ReceiveDamage();  // Chama a animação de dano do Player Luna

            if (luca != null)
                luca.ReceiveDamage();  // Chama a animação de dano do Player Luca

            if (ruby != null)
                ruby.ReceiveDamage();  // Chama a animação de dano do Player Ruby
        }
    }
}


