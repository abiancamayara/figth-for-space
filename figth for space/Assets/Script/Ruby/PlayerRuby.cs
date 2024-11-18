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
    [SerializeField] private Dash dash;

    private float cooldownTiro = 0;
    private Vector2 teclasApertadas;
    public Transform limiteSuperiorEsquerdo, limiteInferiorDireito;

    public GameObject bombaPrefab;  // Referência ao prefab da bomba
    public Transform localDoLancamento;  // Local onde a bomba será lançada
    private bool lançandoBomba = false;  // Flag para verificar se a bomba está sendo lançada
    public bool lancou;  // Flag para controle do lançamento
    public float variacaoangulo = 3;

    private float cooldownLancamentoBomba = 0.5f;  // Tempo entre lançamentos de bombas
    private float tempoCooldownBomba = 0f;  // Controla o cooldown do lançamento

    private Animator animator;
    private Transition currentTransition;

    public enum Transition
    {
        Parado = 0,
        Voando = 1,
        Hit = 2,
        Morrendo = 3
    }

    void Start()
    {
        temLaserDuplo = true;
        animator = GetComponent<Animator>();
        SetTransition(Transition.Parado);
    }

    void Update()
    {
        if (!this.dash.Usado)
        {
            MovimentarJogador();
            LimiteDoJogador();
        }

        AplicarDash();

        // Lógica para disparar a bomba com cooldown
        if (Input.GetKey(KeyCode.X) && tempoCooldownBomba <= 0)  // Verifica se a tecla está pressionada e o cooldown expirou
        {
            LançarBomba();
        }

        // Atualiza o cooldown da bomba
        if (tempoCooldownBomba > 0)
        {
            tempoCooldownBomba -= Time.deltaTime;  // Decrementa o tempo do cooldown
        }

        // Se não estiver lançando bomba, atira o laser
        if (!lançandoBomba)
        {
            AtirarLaser();
        }

        // Atualiza a animação conforme a movimentação
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
            if (!temLaserDuplo)
            {
                Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            }
            else
            {
                Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            }
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));

            cooldownTiro += 1 / balasPorSegundo;
            AudioObserver.OnplaySFXEvent("tiro", Random.value);
        }
    }

    private void LançarBomba()
    {
        // Instancia a bomba e configura o local do lançamento
        GameObject bomba = Instantiate(bombaPrefab, localDoLancamento.position, localDoLancamento.rotation);
        AtaqueRuby bombaScript = bomba.GetComponent<AtaqueRuby>();
        if (bombaScript != null)
        {
            bombaScript.player = this;  // Passa referência do jogador para a bomba
        }

        // Reinicia o cooldown de lançamento da bomba
        tempoCooldownBomba = cooldownLancamentoBomba;

        lançandoBomba = true;  // Ativa a flag para indicar que está lançando a bomba
        lancou = true;  // Marca que o jogador lançou a bomba
    }

    private void ResetarLancamento()
    {
        lançandoBomba = false;  // Permite novo lançamento após o cooldown
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
        SetTransition(Transition.Hit);
        StartCoroutine(HandleHitTransition());
    }

    private IEnumerator HandleHitTransition()
    {
        yield return new WaitForSeconds(0.5f);
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
