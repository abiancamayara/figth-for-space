using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    
    public float velocidadeDoInimigo;
    public int danoParaDar;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    
    private Transform Target;
    private Rigidbody2D inimigoRb;
    private GameObject jogador;
    

    void Start()
    {
        vidaAtualDoInimigo = vidaMaximaDoInimigo;

        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    void Update()
    {
        MovimentarInimigo();
        transform.position = Vector2.MoveTowards(transform.position, Target.position, velocidadeDoInimigo * Time.deltaTime);
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
            Destroy(this.gameObject);
        }
    }

  
}
