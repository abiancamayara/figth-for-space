using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hordatutorial : MonoBehaviour
{
    public GameObject laserDoInimigo;
    public GameObject itemParaDropar;  // Item de drop principal
    public GameObject outroItemParaDropar; // Item de drop secundário
    public Transform localDoDisparo;
    
    public float velocidadeDoInimigo;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    public int chanceParaDropar;
    public int pontosParaDar;
    
    public float tempoMaximoEntreOsLasers; 
    public float tempoAtualDosLasers; 
    
    public bool inimigoAtirador; 
    public bool inimigoAtivado;

    // Start is called before the first frame update
    void Start()
    {
        inimigoAtivado = false;
        vidaAtualDoInimigo = vidaMaximaDoInimigo;
    }

    // Update is called once per frame
    void Update()
    {
        MovimentarInimigo();
        if(inimigoAtirador == true && inimigoAtivado == true)
        {
            AtirarLaser(); 
        }
    }

    public void AtivarInimigo()
    {
        inimigoAtivado = true;
    }

    private void MovimentarInimigo()
    {
        transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
    }

    private void AtirarLaser()
    {
        tempoAtualDosLasers -= Time.deltaTime; 

        if(tempoAtualDosLasers <= 0)
        {
            Instantiate(laserDoInimigo, localDoDisparo.position, Quaternion.Euler(0f, 0f, 90f));
            tempoAtualDosLasers = tempoMaximoEntreOsLasers;
        }
    }

    public void MachucarInimigo(int danoParaReceber)
    {
        vidaAtualDoInimigo -= danoParaReceber;

        if(vidaAtualDoInimigo <= 0)
        {
            // Aumenta a pontuação do jogador
            GameManager.instance.AumentarPontuacao(pontosParaDar);

            // Determina se o inimigo vai dropar um item
            int numeroAleatorio = Random.Range(0, 100);

            // Verifica se o inimigo vai dropar algo
            if (numeroAleatorio <= chanceParaDropar)
            {
                // Dropa o primeiro item
                Instantiate(itemParaDropar, transform.position, Quaternion.Euler(0f, 0f, 0f));

                // Dropa o segundo item
                Instantiate(outroItemParaDropar, transform.position + new Vector3(1, 0, 0), Quaternion.Euler(0f, 0f, 0f)); // Ajuste a posição conforme necessário
            }

            // Destroi o inimigo
            Destroy(this.gameObject);
        }
    }
}

