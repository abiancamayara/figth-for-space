using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDracon : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    
    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;
    public Transform localDoDisparoParaCima;
    public Transform localDoDisparoParaBaixo;

    public float vidaMaximaDracon;
    public float vidaAtualDracon;
    private float cooldownTiro;
    public float balasPorSegundo = 2f;
    public float variacaoangulo;
    
    //private bool estadoAcelerado = false; // Se o inimigo está no estado acelerado
    
    public int chanceParaDropar;

    private enum EstadoInimigo
    {
        EstadoUm,
        EstadoDois
    }

    private EstadoInimigo estadoAtual;

    void Start()
    {
        vidaAtualDracon = vidaMaximaDracon;
        estadoAtual = EstadoInimigo.EstadoUm; // Começa no estado um
    }

    void Update()
    {
        VerificarEstado();
        Atirar();
    }

    private void VerificarEstado()
    {
        // Verifica se a vida do inimigo está abaixo de 70% da vida máxima
        if (vidaAtualDracon <= vidaMaximaDracon * 0.5f && estadoAtual != EstadoInimigo.EstadoDois)
        {
            estadoAtual = EstadoInimigo.EstadoDois; // Muda para estado dois
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
                AtirarLaserQuadruplo();
                break;
        }
    }

    private void AtirarLaser()
    {
        cooldownTiro -= Time.deltaTime;
        if (cooldownTiro < 0)
        {
            // Tiro duplo
            Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    private void AtirarLaserQuadruplo()
    {
        cooldownTiro -= Time.deltaTime;
        if (cooldownTiro < 0)
        {
            // Tiro quadruplo
            Instantiate(laserDoJogador, localDoDisparoParaCima.position, localDoDisparoParaCima.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoParaBaixo.position, localDoDisparoParaBaixo.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    public void MachucarBoss(int danoParaReceber)
    {
        vidaAtualDracon -= danoParaReceber;

        if(vidaAtualDracon <= 0)
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

