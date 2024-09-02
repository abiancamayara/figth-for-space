using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bomba : MonoBehaviour
{
    //public bool itemDaBomba; 
    public void AtivarBomba()
    {
            //instantitate explosao
            
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigo");

            foreach(var item in inimigos)
            {
                Destroy(item);
            }

            GameObject[] laserdoinimigo = GameObject.FindGameObjectsWithTag("Laser");

            foreach(var item in laserdoinimigo)
            {
                Destroy(item);
            }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
           AtivarBomba();

           Destroy(gameObject);

        }
    }
}
