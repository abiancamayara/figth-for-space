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
        if (temEscudo == false)
        {
            // Primeiro verifica se há escudo, e aplica o dano ao jogador apenas se não houver escudo
            if (vidaAtualDojogador > 0)
            {
                vidaAtualDojogador -= danoParaReceber;
                barraDeVidaDoJogador.value = vidaAtualDojogador;

                // Se a vida do jogador for 0 ou menor, destruir o objeto e finalizar o jogo
                if (vidaAtualDojogador <= 0)
                {
                    Destroy(gameObject);
                    GameManager.instance.GameOver();
                    Debug.Log("Game Over");
                }
                else
                {
                    // Chama o método para iniciar a animação de dano nos três jogadores
                    // Vamos garantir que cada player tenha seu componente de dano e reagir individualmente

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
    }
}

