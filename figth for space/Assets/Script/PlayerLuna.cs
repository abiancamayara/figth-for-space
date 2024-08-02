using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLuna : MonoBehaviour
{
    public Rigidbody2D rig;
    public GameObject laserDoJogador;
    public Transform localDoDisparoUnico; 
    public Transform localDoDisparoDaEsquerda;
    public Transform localDoDisparoDaDireita;

    public float velocidadeDaNave;
    public bool temLaserDuplo;
    public float balasPorSegundo = 5;
    private float cooldownTiro = 0;
    private Vector2 teclasApertadas;

    // Start is called before the first frame update
    void Start()
    {
        temLaserDuplo = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovimentarJogador();
        AtirarLaser();
    }


    private void MovimentarJogador()
    {
        teclasApertadas = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rig.velocity = teclasApertadas.normalized * velocidadeDaNave;
    }

    private void AtirarLaser()
    {
        if(Input.GetKey(KeyCode.X))
        {
            cooldownTiro -= Time.deltaTime;
            if(cooldownTiro <0)
            {
                if(temLaserDuplo == false)
                {
                    Instantiate(laserDoJogador, localDoDisparoUnico.position, localDoDisparoUnico.rotation);
                }
                else
                {
                    Instantiate(laserDoJogador, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation);
                }   Instantiate(laserDoJogador, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation);
                cooldownTiro += 1/balasPorSegundo;
            }

        }

    }
}
