using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LaserDoInimitoRapidos : MonoBehaviour
{
    public float velocidadeDoLaser;
    public int danoParaDar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovimentarLaser();
    }

    

    private void MovimentarLaser()
    { 
    
        transform.Translate(Vector3.down * velocidadeDoLaser * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<VidaDosPlayers>().MachucarJogador(danoParaDar);
            Destroy(this.gameObject);
        }
    }
}
