using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{

    public Transform alvo;
    public int Anguloajuste;
   

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(alvo);
        transform.Rotate(Vector3.up * 90);
        transform.Rotate(Vector3.forward * Anguloajuste);
        
    }
}
