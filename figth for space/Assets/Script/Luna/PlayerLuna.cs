using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLuna : MonoBehaviour
{
    public Rigidbody2D rig;
    public GameObject laserDoJogador;
    public Transform localDoDisparoUnico;
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;
    public Transform localDoDisparoParaCima;
    public Transform localDoDisparoParaBaixo;

    public float velocidadeDaNave;
    public bool temLaserDuplo;
    public float balasPorSegundo = 5;

    [SerializeField] private Dash dash;
    private float cooldownTiro = 0;
    private Vector2 teclasApertadas;
    public Transform limiteSuperiorEsquerdo, limiteInferiorDireito;

    private Animator animator;
    private Transition currentTransition;

    public enum Transition
    {
        Parado = 0,
        Voando = 1,
        Hit = 2,
        Morrendo = 3

    }

    // Start is called before the first frame update
    void Start()
    {
        temLaserDuplo = true;
        animator = GetComponent<Animator>();
        SetTransition(Transition.Parado);
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTiro -= Time.deltaTime;

        if (!this.dash.Usado)
        {
            MovimentarJogador();
            LimiteDoJogador();
            AplicarDash();
        }

        if (Input.GetKey(KeyCode.X))
        {
            AtirarLaserQuadruplo();
        }
        else
        {
            AtirarLaser();
        }

        // Atualiza a transição de animação com base no movimento do jogador, mas não muda durante o hit
        if (currentTransition != Transition.Hit)
        {
            if (teclasApertadas.magnitude > 0)
            {
                SetTransition(Transition.Voando);
            }
            else
            {
                SetTransition(Transition.Parado);
            }
        }
    }

    private void MovimentarJogador()
    {
        teclasApertadas = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rig.velocity = teclasApertadas.normalized * velocidadeDaNave;
    }

    private void AtirarLaser()
    {
        if (cooldownTiro < 0)
        {
            if (!temLaserDuplo)
            {
                Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation);
            }
            else
            {
                Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation);
                Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation);
            }
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    private void AtirarLaserQuadruplo()
    {
        if (cooldownTiro < 0)
        {
            Instantiate(laserDoJogador, localDoDisparoParaCima.position, localDoDisparoParaCima.rotation);
            Instantiate(laserDoJogador, localDoDisparoParaBaixo.position, localDoDisparoParaBaixo.rotation);
            cooldownTiro += 1 / balasPorSegundo;
        }
    }

    public void LimiteDoJogador()
    {
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

    private void AplicarDash()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.dash.Aplicar();
            ParticleObserver.OnParticleSpawnEvent(transform.position);
        }
    }

    public void ReceiveDamage()
    {
        // Ativa a animação de hit
        SetTransition(Transition.Hit);
        StartCoroutine(HandleHitTransition());
    }
    public void Morreu(){
        
        //SetTransition(Transition.Death);
        StartCoroutine(HandleDeathTransition());
    }

    private IEnumerator HandleHitTransition()
    {
        // Aguarda o tempo da animação de hit antes de retornar ao estado normal
        yield return new WaitForSeconds(0.5f); // Tempo de duração do hit, pode ajustar conforme necessário

        // Após o tempo do hit, voltamos à animação de "Parado" ou "Voando"
        if (teclasApertadas.magnitude > 0)
        {
            SetTransition(Transition.Voando);
        }
        else
        {
            SetTransition(Transition.Parado);
        }
    }


    private IEnumerator HandleDeathTransition()
    {
        // Aguarda o tempo da animação de morte antes de destruir o jogador
        yield return new WaitForSeconds(0.7f); // Tempo de duração da animação de morte, pode ajustar conforme necessário
        Debug.Log("Chamoou o destroy");
        
        // Após o tempo da animação de morte, destruímos o jogador
        Destroy(gameObject);
        //yield return new WaitForSeconds(0.5f); // Tempo de duração da animação de morte, pode ajustar conforme necessário
        GameManager.instance.GameOver();
    }

    private void SetTransition(Transition newTransition)
    {
        
        currentTransition = newTransition;
        animator.SetInteger("Transition", (int)currentTransition);
    }
}
