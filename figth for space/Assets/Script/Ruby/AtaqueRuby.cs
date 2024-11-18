using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AtaqueRuby : MonoBehaviour
{
    public GameObject bombaPrefab; // Referência ao prefab da bomba
    public float tempoAntesDeExplodir = 3f; // Tempo em segundos até a bomba explodir
    public float raioExplosao = 5f; // Raio da explosão
    public PlayerRuby player;
    public float velocidadeDoLaser;
    public int danoParaDar;
    public float cooldown = 2f; // Tempo de cooldown entre lançamentos

    private bool podeAtacar = true; // Controle de cooldown

    private void Start()
    {
        // Inicialização não precisa de modificações aqui
    }

    private void Update()
    {
        // Verifica se a tecla 'X' foi pressionada e se o cooldown permitiu o lançamento
        if (Input.GetKeyDown(KeyCode.X) && podeAtacar)
        {
            LançarBomba();
        }
    }

    private void LançarBomba()
    {
        // Desabilita a possibilidade de lançar bombas enquanto o cooldown não passar
        podeAtacar = false;

        // Instancia a bomba a partir do prefab
        GameObject bomba = Instantiate(bombaPrefab, transform.position, Quaternion.identity);

        // Configura a velocidade da bomba - faz com que ela se mova para cima
        AtaqueRuby bombaScript = bomba.GetComponent<AtaqueRuby>();
        if (bombaScript != null)
        {
            bombaScript.velocidadeDoLaser = velocidadeDoLaser; // Atribui a velocidade à bomba instanciada
        }

        // Inicia a explosão da bomba após o tempo especificado
        Invoke("Explodir", tempoAntesDeExplodir);

        // Registra o tempo do lançamento e inicia o cooldown
        Invoke("RecarregarCooldown", cooldown);
    }

    private void RecarregarCooldown()
    {
        podeAtacar = true; // Permite o lançamento de uma nova bomba após o cooldown
    }

    private void Explodir()
    {
        // Lógica de explosão - Encontra todos os colliders dentro do raio de explosão
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, raioExplosao);

        foreach (Collider2D collider in colliders)
        {
            // Verifica o tipo de inimigo e aplica o dano
            Inimigos inimigo = collider.GetComponent<Inimigos>();
            Kamikaze kamikaze = collider.GetComponent<Kamikaze>();
            BossDracon bossDracon = collider.GetComponent<BossDracon>();
            BossGlaucius glaucius = collider.GetComponent<BossGlaucius>();
            BossZarak zarak = collider.GetComponent<BossZarak>();
            ZigZag zigzag = collider.GetComponent<ZigZag>();

            if (inimigo != null)
            {
                inimigo.MachucarInimigo(danoParaDar);
            }
            else if (kamikaze != null)
            {
                kamikaze.MachucarInimigo(danoParaDar);
            }
            else if (bossDracon != null)
            {
                bossDracon.MachucarBoss(danoParaDar); // Aplica dano ao BossDracon
            }
            else if (glaucius != null)
            {
                glaucius.MachucarBoss(danoParaDar); // Aplica dano ao Glaucius
            }
            else if (zarak != null)
            {
                zarak.MachucarBoss(danoParaDar); // Aplica dano ao Zarak
            }
            else if (zigzag != null)
            {
                zigzag.MachucarInimigo(danoParaDar); // Aplica dano ao ZigZag
            }
        }

        // Destrói a bomba após a explosão
        player.lancou = false;
        Destroy(gameObject);
    }

    private void MovimentarLaser()
    {
        transform.Translate(Vector3.up * velocidadeDoLaser * Time.deltaTime); // Move a bomba para cima
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica a colisão da bomba com inimigos e aplica o dano
        Inimigos inimigo = other.GetComponent<Inimigos>();
        Kamikaze kamikaze = other.GetComponent<Kamikaze>();
        BossDracon bossDracon = other.GetComponent<BossDracon>();
        BossGlaucius glaucius = other.GetComponent<BossGlaucius>();
        BossZarak zarak = other.GetComponent<BossZarak>();
        ZigZag zigzag = other.GetComponent<ZigZag>();

        if (inimigo != null)
        {
            inimigo.MachucarInimigo(danoParaDar);
            Destroy(this.gameObject); // Destrói a bomba
        }
        else if (kamikaze != null)
        {
            kamikaze.MachucarInimigo(danoParaDar);
            Destroy(this.gameObject); // Destrói a bomba
        }
        else if (bossDracon != null)
        {
            bossDracon.MachucarBoss(danoParaDar); // Aplica dano ao BossDracon
            Destroy(this.gameObject); // Destrói a bomba
        }
        else if (glaucius != null)
        {
            glaucius.MachucarBoss(danoParaDar); // Aplica dano ao Glaucius
            Destroy(this.gameObject); // Destrói a bomba
        }
        else if (zarak != null)
        {
            zarak.MachucarBoss(danoParaDar); // Aplica dano ao Zarak
            Destroy(this.gameObject); // Destrói a bomba
        }
        else if (zigzag != null)
        {
            zigzag.MachucarInimigo(danoParaDar);
            Destroy(this.gameObject); // Destrói a bomba
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha o gizmo do raio da explosão no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioExplosao); // Raio de explosão
    }
}
