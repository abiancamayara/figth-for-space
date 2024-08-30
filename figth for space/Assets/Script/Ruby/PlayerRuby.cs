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
    public float cooldownBomb = 5f; // Tempo entre lançamentos de bomba
    private float cooldownBombaAtual = 0;

    // Start is called before the first frame update
    void Start()
    {
        temLaserDuplo = true;
    }

    // Update is called once per frame
    
    void Update()
    {
        cooldownBombaAtual -= Time.deltaTime;
        
        if(!this.dash.Usado)
        {
            MovimentarJogador();
            
            LimiteDoJogador();
            AplicarDash();  
        }
        
        AtirarLaser();
        
        // Lógica para disparar a bomba
        if (Input.GetKeyDown(KeyCode.X) && cooldownBombaAtual <= 0)
        {
            LançarBomba();
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
    
    private void LançarBomba()
    {
        // Cria a bomba na posição especificada
        Instantiate(bombaPrefab, localDoLancamento.position, localDoLancamento.rotation);
        cooldownBombaAtual = cooldownBomb; // Reinicia o cooldown
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
}


