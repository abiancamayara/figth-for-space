using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoros : MonoBehaviour
{
    public int danoParaDar;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<VidaDosPlayers>().MachucarJogador(danoParaDar);
            Destroy(this.gameObject);
        }
    }
}
