using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaDosPlayers : MonoBehaviour
{
    public Slider barraDeVidaDoJogador;
    /// public GameObject escudoDoJogador; 
   


    public int vidaMaximaDoJogador;
    public int vidaAtualDojogador;
    //public int vidaMaximaDoEscudo; 
    //public int vidaAtualDoEscudo; 

    public bool temEscudo; 

    // Start is called before the first frame update
    void Start()
    {
        vidaAtualDojogador = vidaMaximaDoJogador;
        barraDeVidaDoJogador.maxValue = vidaMaximaDoJogador;
        barraDeVidaDoJogador.value = vidaAtualDojogador;

        //escudoDoJogador.SetActive(false);
        //temEscudo = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void AtivarEscudo()
    {
        //vidaAtualDoEscudo = vidaMaximaDoEscudo;

        //escudoDoJogador.SetActive(true);
        temEscudo = true;
    }*/

    public void MachucarJogador(int danoParaReceber)
    {
        if(temEscudo == false)
        {
            vidaAtualDojogador -= danoParaReceber;
            barraDeVidaDoJogador.value = vidaAtualDojogador;

            if(vidaAtualDojogador <= 0)
            {
                Debug.Log("Game Over");
            }
        }
        /*else
        {
            vidaAtualDoEscudo -= danoParaReceber;

            if(vidaAtualDoEscudo <= 0)
            {
                //escudoDoJogador.SetActive(false);
                temEscudo = false;
            }
        }*/
    }
}
