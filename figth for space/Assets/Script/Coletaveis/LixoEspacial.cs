using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LixoEspacial : MonoBehaviour
{
    
    //public int totalLixoColetado;   
    
    public int LixoValue;// Armazena a quantidade de lixo coletado

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.ColetarLixo(LixoValue);
            Destroy(gameObject, 0.1f);
        }
    }
}