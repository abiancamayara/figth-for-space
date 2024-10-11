using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGlaucius : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    public GameObject escudoPrefab; // Um prefab para o escudo que será ativado

    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;

    public float vidaMaximaGlaucius;
    public float vidaAtualGlaucius;
    private float cooldownTiro;
    public float balasPorSegundo = 2f;
    public float variacaoangulo;
    
    private bool estadoAcelerado = false; // Se o inimigo está no estado acelerado
    
    public int chanceParaDropar;

    private enum EstadoInimigo
    {
        EstadoUm,
        EstadoDois
    }

    private EstadoInimigo estadoAtual;
    private GameObject escudoAtual; // Para guardar a referência ao escudo ativado

    void Start()
    {
        vidaAtualGlaucius = vidaMaximaGlaucius;
        estadoAtual = EstadoInimigo.EstadoUm; // Começa no estado um
    }

    void Update()
    {
        VerificarEstado();
        Atirar();
    }

    private void VerificarEstado()
    {
        // Verifica se a vida do inimigo está abaixo de 50% da vida máxima
        if (vidaAtualGlaucius <= vidaMaximaGlaucius * 0.5f && estadoAtual != EstadoInimigo.EstadoDois)
        {
            estadoAtual = EstadoInimigo.EstadoDois; // Muda para estado dois
            AtivarEscudo(); // Chamamos o método para ativar o escudo
        }
    }

    private void Atirar()
    {
        switch (estadoAtual)
        {
            case EstadoInimigo.EstadoUm:
                AtirarLaser();
                break;
            case EstadoInimigo.EstadoDois:
                // Não precisa atirar, o escudo é ativado
                break;
        }
    }

    private void AtivarEscudo()
    {
        if (escudoAtual == null)
        {
            escudoAtual = Instantiate(escudoPrefab, transform.position, Quaternion.identity);
            // Ajustar a posição do escudo, se necessário
            escudoAtual.transform.SetParent(transform);
        }
    }

    private void AtirarLaser()
    {
        cooldownTiro -= Time.deltaTime;
        if (cooldownTiro < 0)
        {
            // Tiro duplo
            Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    public void MachucarBoss(int danoParaReceber)
    {
        vidaAtualGlaucius -= danoParaReceber;

        if (vidaAtualGlaucius <= 0)
        {
            int numeroAleatorio = Random.Range(0, 100);

            if (numeroAleatorio <= chanceParaDropar)
            {
                Instantiate(itemParaDropar, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            Destroy(this.gameObject);
        }
    }
}

