using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLuca : MonoBehaviour
{
    public Rigidbody2D rig;

    public GameObject laserDoJogador;
    public GameObject ondaSonora; // Adicionado para o ataque secundário
    public Transform localDoDisparoUnico; 
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;

    public float velocidadeDaNave;
    public bool temLaserDuplo;
    public float balasPorSegundo = 5;



    [SerializeField]
    private Dash dash;


    private float cooldownTiro = 0;
    private Vector2 teclasApertadas;
    public Transform limiteSuperiorEsquerdo, limiteInferiorDireito;
    private float cooldownOndaSonoraAtual; // Controle do cooldown da onda sonora
    
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
        
        if(!this.dash.Usado)
        {
            MovimentarJogador();
            LimiteDoJogador();
            AplicarDash();  
        }
        if(Input.GetKey(KeyCode.X))
        {
            AtirarOndaSonora();
        }
        else
        {
            AtirarLaser();
        }
        
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
            cooldownTiro -= Time.deltaTime;
            if(cooldownTiro <0)
            {
                if(temLaserDuplo == false)
                {
                    Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation);
                }
                else
                {
                    Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation);
                }   Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation);
                cooldownTiro += 1/balasPorSegundo;
            }
    }
    
    private void AtirarOndaSonora()
    {
        Instantiate(ondaSonora, localDoDisparoUnico.position, localDoDisparoUnico.rotation);
    }


    public void LimiteDoJogador()
    {
        if(transform.position.x < limiteSuperiorEsquerdo.position.x)
        {
            transform.position = new Vector2 (limiteSuperiorEsquerdo.position.x, transform.position.y);
        }

        if(transform.position.y > limiteSuperiorEsquerdo.position.y)
        {
            transform.position = new Vector2 (transform.position.x, limiteSuperiorEsquerdo.position.y);
        }

        if(transform.position.x > limiteInferiorDireito.position.x)
        {
            transform.position = new Vector2 (limiteInferiorDireito.position.x, transform.position.y);
        }

        if(transform.position.y < limiteInferiorDireito.position.y)
        {
            transform.position = new Vector2 (transform.position.x, limiteInferiorDireito.position.y);
        }

    }

    private void AplicarDash()
    {
        if(Input.GetKeyDown(KeyCode.Q))
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
        Debug.Log("CHamou o morreu");
        // Ativa a animação de morte

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
        // Destrói o jogador após a animação de morte
        Destroy(gameObject);
        GameManager.instance.GameOver();
    }

    private void SetTransition(Transition newTransition)
    {
        
        currentTransition = newTransition;
        animator.SetInteger("Transition", (int)currentTransition);
    }
}
