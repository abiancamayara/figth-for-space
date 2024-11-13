using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuby : MonoBehaviour
{
    public Rigidbody2D rig;

    public GameObject laserDoJogador;
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
    
    public GameObject bombaPrefab; // O prefab da bomba
    public Transform localDoLancamento; // O local onde a bomba será lançada

    private bool lançandoBomba = false; // Flag para verificar se a bomba está sendo lançada
    public bool lancou;
    public float variacaoangulo = 3;
    
    private Animator animator;
    private Transition currentTransition;

    public enum Transition
    {
        Parado = 0,
        Voando = 1,
        Hit = 2
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
        // Se o dash não está sendo usado, podemos movimentar o jogador
        if (!this.dash.Usado)
        {
            MovimentarJogador();
            LimiteDoJogador();
        }
        
        AplicarDash();  
        
        // Lógica para disparar a bomba
        if (Input.GetKeyDown(KeyCode.X) && !lancou)
        {
            LançarBomba();
            lançandoBomba = true; // Ativa a flag enquanto está lançando a bomba
            lancou = true;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            lançandoBomba = false;
        }
        
        // Se não estiver lançando bomba, atira 
        if (!lançandoBomba)
        {
            AtirarLaser();
        }
        else
        {
            // Se a bomba foi lançada, desativa a flag após um pequeno delay
            // Para evitar disparar lasers imediatamente após lançar a bomba
           Invoke("ResetarLancamento", 0.1f);
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
        if(cooldownTiro < 0)
        {
            if(temLaserDuplo == false)
            {
                Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            }
            else
            {
                Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
                
            }
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward*Random.Range(-variacaoangulo,variacaoangulo));
            
            cooldownTiro += 1 / balasPorSegundo;
            AudioObserver.OnplaySFXEvent("tiro", Random.value);
        }
    }
    
    private void LançarBomba()
    {
        GameObject bomba = Instantiate(bombaPrefab, localDoLancamento.position, localDoLancamento.rotation);
        bomba.GetComponent<AtaqueRuby>().player = this;
    }

    private void ResetarLancamento()
    {
        lançandoBomba = false; // Reinicia a flag para permitir disparo novamente
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

    private void SetTransition(Transition newTransition)
    {
        currentTransition = newTransition;
        animator.SetInteger("Transition", (int)currentTransition);
    }
}
