using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaSonora : MonoBehaviour
{
    public float tempoDeVida = 2f;  // Tempo de vida da onda sonora
    public float taxaExpansao = 2f; // Taxa de expansão da onda sonora
    public float raioMaximo = 5f;  // Raio máximo da onda sonora
    public float velocidadeDoLaser;
    public int dano = 10;          // Dano causado pela onda sonora

    private float tempoAtual = 0f; // Tempo atual de vida da onda sonora
    private CircleCollider2D colisor; // Colisor da onda sonora

    public PlayerLuca player;

    void Start()
    {
        colisor = GetComponent<CircleCollider2D>();
        colisor.radius = 0f; // Inicialmente, o raio da onda sonora é 0
    }

    void Update()
    {
        // Expande a onda sonora enquanto ela está viva
        if (tempoAtual < tempoDeVida)
        {
            tempoAtual += Time.deltaTime;
            colisor.radius = Mathf.Lerp(0f, raioMaximo, tempoAtual / tempoDeVida);
        }
        else
        {
            Destroy(gameObject); // Destrói a onda sonora quando o tempo de vida acabar
        }

        // Detecta inimigos dentro da área da onda sonora
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(transform.position, colisor.radius);
        foreach (Collider2D inimigo in inimigos)
        {
            if (inimigo.CompareTag("Inimigo"))
            {
                inimigo.GetComponent<Inimigos>().MachucarInimigo(dano);
            }
        }
    }

    public void IniciarOnda()
    {
        // A onda sonora começa a se expandir assim que é criada
        tempoAtual = 0f;
    }
    
    private void MovimentarLaser()
    {
        // Movimento para frente, com base na rotação do jogador
        Vector3 direction = player.transform.right;  // Agora, a direção é para a frente, considerando a rotação do jogador
        transform.Translate(direction * velocidadeDoLaser * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, raioMaximo);  // Visualiza o raio da onda sonora no editor
    }
}
