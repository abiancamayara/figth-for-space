using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carta : MonoBehaviour
{
    public int vidaAtualDracon;
    public int danoPorHit;
    
    public GameObject cartaPrefab; // Prefab da carta que você quer coletar
    public TextMeshProUGUI quantidadeCartasText; // Text UI para mostrar a quantidade de cartas

    private int quantidadeCartas = 0;

    public void MachucarBoss(int danoParaReceber)
    {
        vidaAtualDracon -= danoParaReceber;

        if (vidaAtualDracon <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        // Aqui você pode instanciar a carta
        InstanciarCarta();
        // Atualiza a UI para mostrar a quantidade de cartas
        AtualizarQuantidadeCartasUI();
        Destroy(this.gameObject);
    }

    private void InstanciarCarta()
    {
        // Instantiate a carta na posição do boss ou em um lugar específico
        Instantiate(cartaPrefab, transform.position, Quaternion.identity);

        // Aumenta a quantidade de cartas coletadas
        quantidadeCartas++;
    }

    private void AtualizarQuantidadeCartasUI()
    {
        quantidadeCartasText.text = "Carta: " + quantidadeCartas.ToString();
    }
}
