using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLuna : MonoBehaviour
{
    public float velocidadeDoLaser; 


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
        transform.Translate(Vector3.up * velocidadeDoLaser * Time.deltaTime);
    }
}
