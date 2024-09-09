using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensColetaveis : MonoBehaviour
{
    public bool itemDoEscudo; 
    public bool itemDaBomba; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(itemDoEscudo == true)
            {
                other.gameObject.GetComponent<VidaDosPlayers>().AtivarEscudo();
            }

            Destroy(this.gameObject);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
