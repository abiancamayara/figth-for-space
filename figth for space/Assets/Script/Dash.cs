using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField]
    private float velocidade;

    [SerializeField]
    public Rigidbody2D rigi;

    private bool usando;

    [SerializeField]
    private float duracao;
    public float contTempoDeUso;


    // Start is called before the first frame update
    void Start()
    {
        this.usando = false;
        this.contTempoDeUso = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.usando)
        {
            this.contTempoDeUso += Time.deltaTime;
            if(this.contTempoDeUso >= this.duracao)
            {
                this.contTempoDeUso = 0;
                this.usando = false;
            }
            else
            {
                this.rigi.velocity = new Vector2(this.velocidade, 0);
            }
        }
    }

    public bool Usado
    {
        get
        {
            return this.usando;
        }
    }

    public void Aplicar()
    {
        this.usando = true;
    }
}
