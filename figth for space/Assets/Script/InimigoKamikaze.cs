using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoKamikaze : MonoBehaviour
{
    public GameObject laserDoInimigo;
    public Transform localDoDisparo;

    public float velocidadeDoInimigo;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;

    public float tempoMaximoEntreOsLasers;
    public float tempoAtualDosLasers;

    public bool inimigoAtivado;

    public string estado = "parado";
    public Transform player;  // Referência ao jogador
    public float distanciaParaSeguir = 10f;  // Distância para começar a seguir o jogador
    public float distanciaParaColidir = 1f;  // Distância em que o inimigo colide e morre
    public float tempoParaSeguir = 2f;  // Tempo que o inimigo fica seguindo o jogador antes de ir em linha reta

    private float tempoDeSeguir = 0f;

    void Start()
    {
        inimigoAtivado = false;
        vidaAtualDoInimigo = vidaMaximaDoInimigo;
        estado = "parado";
    }

    void Update()
    {



        if (!inimigoAtivado) return;

        switch (estado)
        {
            case "parado":
                if (Vector3.Distance(transform.position, player.position) < distanciaParaSeguir)
                {
                    estado = "seguindo";
                }
                break;

            case "seguindo":
                SeguirJogador();
                break;

            case "reto":
                MovimentarReto();
                break;

            case "morrendo":
                // Adicione aqui lógica para morte se necessário (efeitos, animações, etc.)
                Destroy(this.gameObject, 0.5f); // Tempo para destruir o objeto após a morte
                break;
        }

        // Verifica se o inimigo deve morrer ao colidir com o jogador
        if (Vector3.Distance(transform.position, player.position) < distanciaParaColidir)
        {
            estado = "morrendo";
        }
    }

    public void AtivarInimigo()
    {
        inimigoAtivado = true;
        estado = "parado"; // Começa parado
    }

    private void SeguirJogador()
    {
        // Movimento em direção ao jogador
        transform.position = Vector3.MoveTowards(transform.position, player.position, velocidadeDoInimigo * Time.deltaTime);
        tempoDeSeguir += Time.deltaTime;

        // Se o inimigo seguiu por um tempo suficiente ou chegou muito perto, muda para o estado reto
        if (tempoDeSeguir >= tempoParaSeguir || Vector3.Distance(transform.position, player.position) < distanciaParaColidir)
        {
            estado = "reto";
        }
    }

    private void MovimentarReto()
    {
        // Movimento em linha reta em direção ao jogador
        transform.position = Vector3.MoveTowards(transform.position, player.position, velocidadeDoInimigo * Time.deltaTime);
        
        // O inimigo morre se atingir o jogador
        if (Vector3.Distance(transform.position, player.position) < distanciaParaColidir)
        {
            estado = "morrendo";
        }
    }

    public void MachucarInimigo(int danoParaReceber)
    {
        vidaAtualDoInimigo -= danoParaReceber;

        if (vidaAtualDoInimigo <= 0)
        {
            estado = "morrendo";  // Mudar para estado de morte
        }
    }
}


