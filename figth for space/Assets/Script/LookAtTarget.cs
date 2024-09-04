using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{

    public Transform alvo;
   

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(alvo);
        transform.Rotate(Vector3.up * 90);
    }
}
