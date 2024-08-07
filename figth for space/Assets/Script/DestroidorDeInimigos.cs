using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroidorDeInimigos : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Inimigo"))
        {
            Destroy(other.gameObject);
        }
    }
}
