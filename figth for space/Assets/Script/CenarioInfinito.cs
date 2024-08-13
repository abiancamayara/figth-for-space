using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenarioInfinito : MonoBehaviour
{
    public float velocidadeDoCenario;

    public float d;

   
    // Update is called once per frame
    void Update()
    {
        MovimentarCenario();
    }

    private void MovimentarCenario()
    {
        
        d += Time.deltaTime * velocidadeDoCenario;
        if(d >= 0.5f) d -= 0.5f;
      Vector2 deslocamentoDoCenario = new Vector2(d, 0f);
        GetComponent<Renderer>().material.mainTextureOffset = deslocamentoDoCenario;
    }

}
