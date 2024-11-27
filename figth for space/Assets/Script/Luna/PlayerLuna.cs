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
    public float balasPorSegundo;
    public float variacaoangulo;

    [SerializeField] private Dash dash;
    private float cooldownTiro = 0;
    private Vector2 teclasApertadas;
    public Transform limiteSuperiorEsquerdo, limiteInferiorDireito;

    private Animator animator;
    private Transition currentTransition;

    private AudioSource audioSource; // Adiciona um campo para o AudioSource
    public AudioClip somExplosao; // Adiciona um campo para o som de explosão

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
        audioSource = GetComponent<AudioSource>(); // Obtém o AudioSource anexado ao GameObject

        // Verificação adicional para garantir que o AudioSource e o AudioClip não estão nulos
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado no GameObject do jogador.");
        }

        if (somExplosao == null)
        {
            Debug.LogError("AudioClip do som de explosão não atribuído no inspetor.");
        }

        SetTransition(Transition.Parado);
    }

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
                Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
                Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            }
            cooldownTiro += 1 / balasPorSegundo;
            AudioObserver.OnplaySFXEvent("tiro", Random.value);
        }
    }

    private void AtirarLaserQuadruplo()
    {
        if (cooldownTiro < 0)
        {
            Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoParaCima.position, localDoDisparoParaCima.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            Instantiate(laserDoJogador, localDoDisparoParaBaixo.position, localDoDisparoParaBaixo.rotation).transform.Rotate(Vector3.forward * Random.Range(-variacaoangulo, variacaoangulo));
            cooldownTiro += 1 / balasPorSegundo;
            AudioObserver.OnplaySFXEvent("tiro", Random.value);
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

    public void Morreu()
    {
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
        // Reproduz o som de explosão
        if (audioSource != null && somExplosao != null)
        {
            audioSource.PlayOneShot(somExplosao);
        }

        // Aguarda o tempo da animação de morte antes de destruir o jogador
        yield return new WaitForSeconds(0.7f); // Tempo de duração da animação de morte, pode ajustar conforme necessário
        Debug.Log("Chamou o destroy");

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
