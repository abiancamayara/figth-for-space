using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigos : MonoBehaviour
{
    public float velocidadeDoInimigo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovimentarInimigo();
    }

    private void MovimentarInimigo()
    {
        transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
    }
}
