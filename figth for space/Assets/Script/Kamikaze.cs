using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    
    public float velocidadeDoInimigo;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    
    private Transform Target;
    private Rigidbody2D inimigoRb;
    private GameObject jogador;

    public bool inimigoAtivado;

    /*void Start()
    {
        inimigoAtivado = false;
        vidaAtualDoInimigo = vidaMaximaDoInimigo;

        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    void Update()
    {
        MovimentarInimigo();
        transform.position = Vector2.MoveTowards(transform.position, Target.position, velocidadeDoInimigo * Time.deltaTime);
    }
    public void AtivarInimigo()
    {
        inimigoAtivado = true;
    }

    private void MovimentarInimigo()
    {
        transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
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
    }*/

  
}
