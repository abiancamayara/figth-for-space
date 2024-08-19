using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InimigoKamikaze : MonoBehaviour
{
    public GameObject laserDoInimigo; 
    public Transform localDoDisparo;
    

    public float velocidadeDoInimigo;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    

    public float tempoMaximoEntreOsLasers; 
    public float tempoAtualDosLasers; 

    public bool inimigoAtirador; 
    public bool inimigoAtivado;

    public string estado = "parado";



    // Start is called before the first frame update
    void Start()
    {
        inimigoAtivado = false;

        vidaAtualDoInimigo = vidaMaximaDoInimigo;
    }

    // Update is called once per frame
    /*void Update()
    {

        if(estado == "parado")
        {
                MovimentarInimigo();
                if(inimigoAtirador == true && inimigoAtivado == true)
                {
                    AtirarLaser();
                     estado = "seguindo";
                }
        }
        else if(estado == "seguindo")
        {
            if(inimigoAtirador == true && inimigoAtivado == true)
                {
                    AtirarLaser(); 
                }
        }
        else if (estado == "reto")
        {

        }
        else if(estado == "morrendo")
        {


        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            estado = "seguindo";
        }
       
        
    }*/

    public void AtivarInimigo()
    {
        inimigoAtivado = true;
    }

    private void MovimentarInimigo()
    {
        transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
    }

    private void AtirarLaser()
    {
        tempoAtualDosLasers -= Time.deltaTime; 

        if(tempoAtualDosLasers <= 0)
        {
            Instantiate(laserDoInimigo, localDoDisparo.position, Quaternion.Euler(0f, 0f, 90f));
            tempoAtualDosLasers = tempoMaximoEntreOsLasers;
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
