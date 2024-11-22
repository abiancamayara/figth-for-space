using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGlaucius : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    public GameObject escudoPrefab; // Um prefab para o escudo que será ativado
    public GameObject destruidorObj;

    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;
    
    public Transform alvo;

    public float vidaMaximaGlaucius;
    public float vidaAtualGlaucius;
    private float cooldownTiro;
    public float balasPorSegundo;
    public float variacaoangulo;
    
    //private bool estadoAcelerado = false; // Se o inimigo está no estado acelerado
    public bool temEscudo;
    
    public int chanceParaDropar;
    public int vidaMaximaDoEscudo; // Vida máxima do escudo
    public int vidaAtualDoEscudo;
    
    private Animator animator;
    private Transition currentTransition;
    
    public enum Transition
    {
        Parado = 0,
        Morrendo = 1
    }

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
        alvo = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        SetTransition(Transition.Parado);
    }

    void Update()
    {
        if(alvo == null) return;
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
                AtirarLaser();
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
    
    public void Morreu(){
        Debug.Log("CHamou o morreu");
        // Ativa a animação de morte

        StartCoroutine(HandleDeathTransition());
    }
    
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
                vidaAtualGlaucius -= Mathf.Abs(vidaAtualDoEscudo); // Aplica o dano restante que "passou" pelo escudo
            }
        }
        else
        {
            // Se não tiver escudo, o dano vai diretamente para a vida do boss
            vidaAtualGlaucius -= danoParaReceber;
        }
        // Se a vida do Boss for 0 ou menos, chama a função de morte
        if (vidaAtualGlaucius <= 0)
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

