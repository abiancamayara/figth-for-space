using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LixoEspacial : MonoBehaviour
{
    public VidaDosPlayers vidaDosPlayers;  // Referência à classe VidaDosPlayers// Texto que mostrará a quantidade de lixo coletado
    public int totalLixoColetado;   
    
    public int LixoValue;// Armazena a quantidade de lixo coletado

    // Este método deve ser chamado quando o jogador colhe um lixo espacial
    public void ColetarLixo()
    {
        // Verifica se coletou 5 lixos
        if (totalLixoColetado >= 5)
        {
            AumentarVida();
            totalLixoColetado = 0; // Reinicia o contador após o aumento de vida
        }
    }
    
    private void AumentarVida()
    {
        int vidaParaReceber = Mathf.CeilToInt(vidaDosPlayers.vidaMaximaDoJogador * 0.2f); // 20% da vida máxima
        vidaDosPlayers.GanharVida(vidaParaReceber);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.AtualizarContadorTexto(LixoValue);
            Destroy(gameObject, 0.1f);
        }
    }
}