using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZag : MonoBehaviour
{
    public GameObject laserDoInimigo;  // Prefab do laser
    public GameObject itemParaDropar;  // Prefab do item

    public Transform localDoDisparo;  // Local de disparo (não utilizado diretamente aqui, pois temos os locais da esquerda e direita)
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;

    public int multiplicador;
    public float velocidadeDoInimigo;

    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    public int chanceParaDropar;

    private Transform Target;

    public float angulo = 0;
    public float cooldownTiro;
    public float balasPorSegundo;
    public float alturaOriginal;

    public int amplitude;
    public int velocidadeDaOscilacao = 10;
    public int pontosParaDar;

    public bool inimigoAtirador;  // Atirador habilitado?
    public bool inimigoAtivado;  // Inimigo ativado para atirar

    void Start()
    {
        //inimigoAtivado = false;

        // Encontrar o jogador na cena
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        alturaOriginal = transform.position.y;

        // Inicializando a vida do inimigo
        vidaAtualDoInimigo = vidaMaximaDoInimigo;
        cooldownTiro = 1 / balasPorSegundo;  // Inicia o cooldown
    }

    void Update()
    {
        // Verificar se o inimigo pode atirar
        if (inimigoAtirador && inimigoAtivado)
        {
            AtirarLaser();  // Tentar atirar lasers se as condições forem atendidas
        }

        MovimentarInimigo();
        MovimentoSeno();
    }

    public void AtivarInimigo()
    {
        inimigoAtivado = true;
    }

    private void MovimentarInimigo()
    {
        Vector3 movimentar = new Vector3(transform.position.x, Target.position.y);
        transform.position = Vector3.Lerp(transform.position, movimentar, multiplicador * Time.deltaTime);
        transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
    }

    private void AtirarLaser()
    {
        cooldownTiro -= Time.deltaTime;  // Atualiza o cooldown

        if (cooldownTiro <= 0)
        {
            // Verificar se o prefab do laser está atribuído
            if (laserDoInimigo != null)
            {
                Debug.Log("atirou");
                // Dispara os lasers nos locais definidos
                Instantiate(laserDoInimigo, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation);
                Instantiate(laserDoInimigo, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation);

                // Reinicia o cooldown
                cooldownTiro = 1 / balasPorSegundo;
            }
            else
            {
                Debug.LogWarning("Prefab 'laserDoInimigo' não está atribuído!");
            }
        }
    }

    void MovimentoSeno()
    {
        angulo += Time.deltaTime * velocidadeDaOscilacao;
        if (angulo >= 360)
        {
            angulo -= 360;
        }

        transform.position = new Vector3(transform.position.x, alturaOriginal + Mathf.Sin(angulo) * amplitude);
    }

    public void MachucarInimigo(int danoParaReceber)
    {
        vidaAtualDoInimigo -= danoParaReceber;

        if (vidaAtualDoInimigo <= 0)
        {
            GameManager.instance.AumentarPontuacao(pontosParaDar);
            int numeroAleatorio = Random.Range(0, 100);

            if (numeroAleatorio <= chanceParaDropar)
            {
                Instantiate(itemParaDropar, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }

            Destroy(this.gameObject);
        }
    }
}
