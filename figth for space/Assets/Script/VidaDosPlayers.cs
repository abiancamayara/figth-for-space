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

        //escudoDoJogador.SetActive(false);
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
        if(vidaAtualDojogador + vidaParaReceber <= vidaMaximaDoJogador)
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
        if(temEscudo == false)
        {
            vidaAtualDojogador -= danoParaReceber;
            barraDeVidaDoJogador.value = vidaAtualDojogador;

            if(vidaAtualDojogador <= 0)
            {
                Destroy(gameObject);
                GameManager.instance.GameOver();
                Debug.Log("Game Over");
            }
        }
        else
        {
            vidaAtualDoEscudo -= danoParaReceber;
            barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;

            if(vidaAtualDoEscudo <= 0)
            {
                DesativarEscudo();
                barraDeEnergiaDoEscudo.gameObject.SetActive(false);
            }
        }
    }
}
