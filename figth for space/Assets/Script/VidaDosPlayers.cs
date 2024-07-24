using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaDosPlayers : MonoBehaviour
{
    public int vidaMaximaDoJogador;
    public int vidaAtualDojogador;

    public bool temEscudo; 

    // Start is called before the first frame update
    void Start()
    {
        vidaAtualDojogador = vidaMaximaDoJogador;
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

            if(vidaAtualDojogador <= 0)
            {
                Debug.Log("Game Over");
            }
        }
    }
}
