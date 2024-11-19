using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZarak : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    public GameObject escudoPrefab; // Prefab para o escudo que será ativado

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
    public int vidaMaximaDoEscudo; // Vida máxima do escudo
    public int vidaAtualDoEscudo; // Para armazenar a vida atual do escudo

    // Adicionando uma variável para controlar o tempo entre os disparos no EstadoDois
    public float tempoEntreTirosFeixeContinui = 0.5f; // Tempo entre os tiros contínuos no EstadoDois
    private float cooldownFeixeContinui; // Variável para controlar o tempo de cooldown do feixe contínuo
    
    private Animator animator;

    private enum EstadoInimigo
    {
        EstadoUm,  // Atira lasers normais
        EstadoDois, // Atira feixe contínuo
        EstadoTres // Ativa o escudo
    }

    private EstadoInimigo estadoAtual;
    private GameObject escudoAtual; // Para guardar a referência ao escudo ativado

    void Start()
    {
        vidaAtualZarak = vidaMaximaZarak;
        estadoAtual = EstadoInimigo.EstadoUm; // Começa no EstadoUm
        vidaAtualDoEscudo = vidaMaximaDoEscudo; // Inicializa a vida do escudo
        cooldownFeixeContinui = tempoEntreTirosFeixeContinui; // Inicializa o cooldown do feixe contínuo
        
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        VerificarEstado(); // Checa se deve mudar de estado
        Atirar(); // Responsável por atirar de acordo com o estado atual
        VerificarVidaDoEscudo(); // Verifica se o escudo foi destruído
    }

    private void VerificarEstado()
    {
        // Verifica a mudança de estado com base na vida do Boss
        if (vidaAtualZarak <= vidaMaximaZarak * 0.2f && estadoAtual != EstadoInimigo.EstadoTres)
        {
            estadoAtual = EstadoInimigo.EstadoTres; // Muda para EstadoTres
            AtivarEscudo(); // Ativa o escudo
        }
        else if (vidaAtualZarak <= vidaMaximaZarak * 0.5f && estadoAtual != EstadoInimigo.EstadoDois)
        {
            estadoAtual = EstadoInimigo.EstadoDois; // Muda para EstadoDois
        }
    }

    private void Atirar()
    {
        // Comportamento de ataque de acordo com o estado
        switch (estadoAtual)
        {
            case EstadoInimigo.EstadoUm:
                AtirarLaser(); // Laser normal
                break;
            case EstadoInimigo.EstadoDois:
                AtirarFeixeContinui(); // Feixe contínuo
                break;
            case EstadoInimigo.EstadoTres:
                // Não atira no EstadoTres, apenas ativa o escudo
                break;
        }
    }

    private void AtivarEscudo()
    {
        if (escudoAtual == null)
        {
            escudoAtual = Instantiate(escudoPrefab, transform.position, Quaternion.identity);
            escudoAtual.transform.SetParent(transform); // Faz o escudo seguir o Boss
            vidaAtualDoEscudo = vidaMaximaDoEscudo; // Reseta a vida do escudo
            escudoAtual.SetActive(true); // Ativa o escudo
            temEscudo = true; // Boss agora tem escudo
        }
    }

    private void VerificarVidaDoEscudo()
    {
        // Verifica se o escudo foi destruído
        if (temEscudo && vidaAtualDoEscudo <= 0)
        {
            Destroy(escudoAtual); // Destrói o escudo
            escudoAtual = null; // Reseta a referência ao escudo
            temEscudo = false; // O Boss não tem mais escudo
        }
    }

    private void AtirarLaser()
    {
        cooldownTiro -= Time.deltaTime;
        if (cooldownTiro < 0)
        {
            // Atira lasers normais da esquerda e da direita com variação no ângulo
            Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    private void AtirarFeixeContinui()
    {
        // A lógica do feixe contínuo respeita o tempo entre os disparos
        cooldownFeixeContinui -= Time.deltaTime;

        if (cooldownFeixeContinui <= 0)
        {
            // Instancia o laser contínuo no local definido
            Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));

            // Reseta o cooldown para o próximo disparo
            cooldownFeixeContinui = tempoEntreTirosFeixeContinui;
        }
    }

    public void LimiteDoJogador()
    {
        // Garante que o Boss fique dentro dos limites definidos
        if (transform.position.x < limiteSuperiorEsquerdo.position.x)
        {
            transform.position = new Vector2(limiteSuperiorEsquerdo.position.x, transform.position.y);
        }

        if (transform.position.y > limiteSuperiorEsquerdo.position.y)
        {
            transform.position = new Vector2(transform.position.x, limiteSuperiorEsquerdo.position.y);
        }

        if (transform.position.x > limiteInferiorDireito.position.x)
        {
            transform.position = new Vector2(limiteInferiorDireito.position.x, transform.position.y);
        }

        if (transform.position.y < limiteInferiorDireito.position.y)
        {
            transform.position = new Vector2(transform.position.x, limiteInferiorDireito.position.y);
        }
    }

    public void MachucarBoss(int danoParaReceber)
    {
        if (temEscudo)
        {
            // Se o Boss tem um escudo, reduz a vida do escudo
            vidaAtualDoEscudo -= danoParaReceber;

            if (vidaAtualDoEscudo <= 0)
            {
                Destroy(escudoAtual); // Destrói o escudo quando sua vida chega a zero
                escudoAtual = null; // Reseta a referência
                temEscudo = false; // O Boss não tem mais escudo
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
                Destroy(this.gameObject); // Destrói o Boss
            }
        }
    }
}

