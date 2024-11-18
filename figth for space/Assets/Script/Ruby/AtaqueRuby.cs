using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AtaqueRuby : MonoBehaviour
{
    public GameObject bombaPrefab;  // Referência ao prefab da bomba
    public float tempoAntesDeExplodir = 3f;  // Tempo até a bomba explodir
    public float raioExplosao = 5f;  // Raio de explosão
    public PlayerRuby player;  // Referência ao jogador
    public float velocidadeDoLaser;
    public int danoParaDar;
    public float cooldown = 2f;  // Tempo de cooldown entre lançamentos

    private bool podeAtacar = true;  // Controle de cooldown

    private void Start() { }

    private void Update()
    {
        // Verifica se o jogador apertou a tecla 'X' e se o cooldown permitiu
        if (Input.GetKeyDown(KeyCode.X) && podeAtacar)
        {
            LançarBomba();  // Lança a bomba
        }
        
        // Movimento da bomba (somente se a bomba foi lançada)
        MovimentarLaser();
    }

    private void MovimentarLaser()
    {
        // Supondo que o jogador tenha uma direção ou seja uma transformação a ser seguida.
        // Movimento para frente, na direção do jogador
        Vector3 direction = player.transform.up;  // Assume que "up" é a direção para frente do jogador
        transform.Translate(direction * velocidadeDoLaser * Time.deltaTime);
    }


    private void LançarBomba()
    {
        // Usando a posição da frente do jogador (ajustando com base na direção do jogador)
        Vector3 spawnPosition = player.transform.position + player.transform.up * 1.5f;  // 1.5f é um valor arbitrário, ajusta a distância da bomba
        //GameObject bomba = Instantiate(bombaPrefab, spawnPosition, Quaternion.identity);

        // Configura a bomba (caso precise de ajustes na bomba instanciada)
        AtaqueRuby bombaScript = bombaPrefab.GetComponent<AtaqueRuby>();
        if (bombaScript != null)
        {
            bombaScript.player = player;  // Passa referência do jogador para a bomba
        }

        // Inicia a explosão da bomba após o tempo especificado
        Invoke("Explodir", tempoAntesDeExplodir);

        // Registra o tempo do lançamento e inicia o cooldown
        Invoke("RecarregarCooldown", cooldown);
    }


    private void RecarregarCooldown()
    {
        podeAtacar = true;  // Permite o lançamento de uma nova bomba após o cooldown
    }

    private void Explodir()
    {
        // Lógica de explosão
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, raioExplosao);
        foreach (Collider2D collider in colliders)
        {
            AplicarDanoInimigos(collider);
        }

        Destroy(gameObject);  // Destrói a bomba após a explosão
    }

    private void AplicarDanoInimigos(Collider2D other)
    {
        Inimigos inimigo = other.GetComponent<Inimigos>();
        Kamikaze kamikaze = other.GetComponent<Kamikaze>();
        BossDracon bossDracon = other.GetComponent<BossDracon>();
        BossGlaucius glaucius = other.GetComponent<BossGlaucius>();
        BossZarak zarak = other.GetComponent<BossZarak>();
        ZigZag zigzag = other.GetComponent<ZigZag>();

        if (inimigo != null) inimigo.MachucarInimigo(danoParaDar);
        else if (kamikaze != null) kamikaze.MachucarInimigo(danoParaDar);
        else if (bossDracon != null) bossDracon.MachucarBoss(danoParaDar);
        else if (glaucius != null) glaucius.MachucarBoss(danoParaDar);
        else if (zarak != null) zarak.MachucarBoss(danoParaDar);
        else if (zigzag != null) zigzag.MachucarInimigo(danoParaDar);

        Destroy(this.gameObject);  // Destrói a bomba após a colisão
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioExplosao);  // Desenha o raio de explosão no editor
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Inimigo"))
        {
            other.gameObject.GetComponent<Inimigos>().MachucarInimigo(danoParaDar);
            Destroy(this.gameObject);
        }
        if(other.gameObject.CompareTag("Kamikaze"))
        {
            other.gameObject.GetComponent<Kamikaze>().MachucarInimigo(danoParaDar);
            Destroy(this.gameObject);
        }
        if(other.gameObject.CompareTag("zigzag"))
        {
            other.gameObject.GetComponent<ZigZag>().MachucarInimigo(danoParaDar);
            Destroy(this.gameObject);
        }
        if(other.gameObject.CompareTag("Dracon"))
        {
            other.gameObject.GetComponent<BossDracon>().MachucarBoss(danoParaDar);
            Destroy(this.gameObject);
        }
        if(other.gameObject.CompareTag("Glaucius"))
        {
            other.gameObject.GetComponent<BossGlaucius>().MachucarBoss(danoParaDar);
            Destroy(this.gameObject);
        }
        if(other.gameObject.CompareTag("Zarak"))
        {
            other.gameObject.GetComponent<BossZarak>().MachucarBoss(danoParaDar);
            Destroy(this.gameObject);
        }
        
    }
}
