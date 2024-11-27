using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cartatutorial : MonoBehaviour
{
    public int vidaAtualDracon;
    public int danoPorHit;
    
    public GameObject cartaPrefab; // Prefab da carta que você quer coletar

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
        Destroy(this.gameObject);
    }

    private void InstanciarCarta()
    {
        // Instantiate a carta na posição do boss ou em um lugar específico
        Instantiate(cartaPrefab, transform.position, Quaternion.identity);
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            quantidadeCartas++;
            GameManager.instance.AtualizarQuantidadeCartasUI(quantidadeCartas);
            SceneManager.LoadScene("Cena carta 1");
            Destroy(this.gameObject);
        }
    }
}
