using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZarak : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    public GameObject escudoPrefab; // Prefab para o escudo que será ativado
    public GameObject destruidorObj;

    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;
    public Transform limiteSuperiorEsquerdo, limiteInferiorDireito;
    
    public Transform alvo;

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
    private Transition currentTransition;
    
    public enum Transition
    {
        Parado = 0,
        Morrendo = 1
    }

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
        alvo = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        SetTransition(Transition.Parado);
    }

    void Update()
    {
        if(alvo == null) return;
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
            escudoAtual.transform.SetParent(transform);

            // Defina a vida do escudo
            vidaAtualDoEscudo = vidaMaximaDoEscudo;
            temEscudo = true;
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
    
    
        // Agora aguardamos o tempo suficiente para a animação de morte ser concluída antes de destruir o objeto
        
    

    private IEnumerator HandleDeathTransition()
    {
      
        // Espera o tempo necessário para a animação de morte
        // Certifique-se de que o tempo de duração da animação de morte corresponde ao valor abaixo
        yield return new WaitForSeconds(0.7f);  // Ajuste o tempo conforme necessário, dependendo da duração da animação de morte.
        
        Debug.Log("Chamou o destroy");
        Destroy(gameObject);

        // Chama a lógica do GameManager para finalizar o jogo (ou qualquer outro comportamento necessário)
        //GameManager.instance.GameOver();
    }

    
    private void SetTransition(Transition newTransition)
    {
        // isso aqui não está funcionando legal!>
        currentTransition = newTransition;
        animator.SetInteger("Transition", (int)currentTransition);
    }

    public void MachucarBoss(int danoParaReceber)
    {
        if (temEscudo)
        {
            // Se o Boss tem um escudo, reduz a vida do escudo
            vidaAtualDoEscudo -= danoParaReceber;

            // Verifica se a vida do escudo chegou a zero
            if (vidaAtualDoEscudo <= 0)
            {
                // O escudo foi destruído
                Destroy(escudoAtual); // Destrói o escudo
                escudoAtual = null; // Reseta a referência ao escudo
                temEscudo = false; // O Boss não tem mais escudo

                // A vida do boss pode começar a ser afetada agora
                vidaAtualZarak -= Mathf.Abs(vidaAtualDoEscudo); // Aplica o dano restante que "passou" pelo escudo
            }
        }
        else
        {
            // Se não tiver escudo, o dano vai diretamente para a vida do boss
            vidaAtualZarak -= danoParaReceber;
        }

        // Se a vida do Boss for 0 ou menos, chama a função de morte
        if (vidaAtualZarak <= 0)
        {
            animator.SetTrigger("morrendo");
            // deve mover a logica do morrer está duplicado em 2 lugares
            int numeroAleatorio = Random.Range(0, 100);

            if (numeroAleatorio <= chanceParaDropar)
            {
                Instantiate(itemParaDropar, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            destruidorObj.SetActive(true);
            StartCoroutine(HandleDeathTransition());
        }
    }

}

