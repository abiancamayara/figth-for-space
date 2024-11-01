using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    private Transform Target;
    private Rigidbody2D inimigoRb;
    private GameObject jogador;
    public GameObject itemParaDropar;
    
    public float velocidadeDoInimigo;
    
    public int danoParaDar;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    public int chanceParaDropar;
    public int pontosParaDar;
    
    
    

    void Start()
    {
        vidaAtualDoInimigo = vidaMaximaDoInimigo;

        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    void Update()
    {
        //MovimentarInimigo();
        if (Target.position.x <= transform.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.position, velocidadeDoInimigo * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime, Space.Self);
        }
    }

    private void MovimentarInimigo()
    {
        transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<VidaDosPlayers>().MachucarJogador(danoParaDar);
            Destroy(this.gameObject);
        }
    }

    public void MachucarInimigo(int danoParaReceber)
    {
        vidaAtualDoInimigo -= danoParaReceber;

        if(vidaAtualDoInimigo <= 0)
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
