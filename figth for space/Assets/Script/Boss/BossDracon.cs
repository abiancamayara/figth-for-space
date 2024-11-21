using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDracon : MonoBehaviour
{
    public GameObject laserDoJogador;
    public GameObject itemParaDropar;
    public GameObject destruidorObj;
    
    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;
    public Transform localDoDisparoParaCima;
    public Transform localDoDisparoParaBaixo;

    public Transform alvo;

    public float vidaMaximaDracon;
    public float vidaAtualDracon;
    private float cooldownTiro;
    public float balasPorSegundo = 2f;
    public float variacaoangulo;
    
    private Animator animator;
    private Transition currentTransition;
    
    public enum Transition
    {
        Parado = 0,
        Morrendo = 1
    }
    
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
        vidaAtualDracon -= danoParaReceber;

        if(vidaAtualDracon <= 0)
        {
            animator.SetTrigger("morrendo");  
            int numeroAleatorio = Random.Range(0, 100);

            if (numeroAleatorio <= chanceParaDropar)
            {
                Instantiate(itemParaDropar, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            Destroy(this.gameObject);
            destruidorObj.SetActive(true);
        }
    }
}

