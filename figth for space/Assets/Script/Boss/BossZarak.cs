using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZarak : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    public GameObject escudoPrefab; // Um prefab para o escudo que será ativado

    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;
    public Transform limiteSuperiorEsquerdo, limiteInferiorDireito;

    public float vidaMaximaZarak = 100;
    public float vidaAtualZarak;
    private float cooldownTiro;
    public float balasPorSegundo = 2f;
    public float variacaoangulo;

    public bool temEscudo;

    public int chanceParaDropar;
    public int vidaMaximaDoEscudo; // Definindo a vida máxima do escudo
    public int vidaAtualDoEscudo; // Para armazenar a vida atual do escudo

    private enum EstadoInimigo
    {
        EstadoUm,
        EstadoDois,
        EstadoTres // Adicionado o estado três
    }

    private EstadoInimigo estadoAtual;
    private GameObject escudoAtual; // Para guardar a referência ao escudo ativado

    void Start()
    {
        vidaAtualZarak = vidaMaximaZarak;
        estadoAtual = EstadoInimigo.EstadoUm; // Começa no estado um
        vidaAtualDoEscudo = vidaMaximaDoEscudo; // Inicializa a vida do escudo
    }

    void Update()
    {
        VerificarEstado();
        Atirar();
        VerificarVidaDoEscudo(); // Verifica a vida do escudo a cada frame
    }

    private void VerificarEstado()
    {
        if (vidaAtualZarak <= vidaMaximaZarak * 0.2f && estadoAtual != EstadoInimigo.EstadoTres)
        {
            estadoAtual = EstadoInimigo.EstadoTres; // Muda para estado três
            AtivarEscudo();
        }
        else if (vidaAtualZarak <= vidaMaximaZarak * 0.5f && estadoAtual != EstadoInimigo.EstadoDois)
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
                AtirarFeixeContinui(); // Método para atirar o feixe contínuo
                break;
            case EstadoInimigo.EstadoTres:
                // Não atira, apenas utiliza o escudo
                break;
        }
    }

    private void AtivarEscudo()
    {
        if (escudoAtual == null)
        {
            escudoAtual = Instantiate(escudoPrefab, transform.position, Quaternion.identity);
            escudoAtual.transform.SetParent(transform);
            vidaAtualDoEscudo = vidaMaximaDoEscudo; // Reseta a vida do escudo ao ativá-lo
            escudoPrefab.SetActive(true);
            temEscudo = true; // O boss agora tem escudo
        }
    }

    private void VerificarVidaDoEscudo()
    {
        if (temEscudo && vidaAtualDoEscudo <= 0)
        {
            Destroy(escudoAtual); // Destrói o escudo se a vida chegar a 0
            escudoAtual = null; // Reseta a referência ao escudo
            escudoPrefab.SetActive(false);
            temEscudo = false; // O boss não tem mais escudo
        }
    }

    private void AtirarLaser()
    {
        cooldownTiro -= Time.deltaTime;
        if (cooldownTiro < 0)
        {
            Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    private void AtirarFeixeContinui()
    {
        // Método para atirar um feixe contínuo
        Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
    }

    public void LimiteDoJogador()
    {
        if(transform.position.x < limiteSuperiorEsquerdo.position.x)
        {
            transform.position = new Vector2(limiteSuperiorEsquerdo.position.x, transform.position.y);
        }

        if(transform.position.y > limiteSuperiorEsquerdo.position.y)
        {
            transform.position = new Vector2(transform.position.x, limiteSuperiorEsquerdo.position.y);
        }

        if(transform.position.x > limiteInferiorDireito.position.x)
        {
            transform.position = new Vector2(limiteInferiorDireito.position.x, transform.position.y);
        }

        if(transform.position.y < limiteInferiorDireito.position.y)
        {
            transform.position = new Vector2(transform.position.x, limiteInferiorDireito.position.y);
        }
    }

    public void MachucarBoss(int danoParaReceber)
    {
        if (temEscudo)
        {
            // Se o boss tem um escudo, reduz a vida do escudo em vez da vida do boss
            vidaAtualDoEscudo -= danoParaReceber;

            if (vidaAtualDoEscudo <= 0)
            {
                Destroy(escudoAtual); // Destrói o escudo se a vida chegar a 0
                escudoAtual = null; // Reseta a referência ao escudo
                temEscudo = false; // O boss não tem mais escudo
            }
        }
        else
        {
            vidaAtualZarak -= danoParaReceber;

            if (vidaAtualZarak <= 0)
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
}
