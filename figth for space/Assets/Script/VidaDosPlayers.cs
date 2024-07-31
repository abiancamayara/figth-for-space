using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaDosPlayers : MonoBehaviour
{
    public Slider barraDeVidaDoJogador;
    public int vidaMaximaDoJogador;
    public int vidaAtualDojogador;

    public bool temEscudo; 

    // Start is called before the first frame update
    void Start()
    {
        vidaAtualDojogador = vidaMaximaDoJogador;
        barraDeVidaDoJogador.maxValue = vidaMaximaDoJogador;
        barraDeVidaDoJogador.value = vidaAtualDojogador;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }
}
