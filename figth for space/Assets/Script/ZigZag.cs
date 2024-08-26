using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZag : MonoBehaviour
{
   public int multiplicador;

   public float velocidadeDoInimigo;
   
   private Transform Target;

   public float angulo = 0;

   public float alturaOriginal;
   public int amplitude;
   public int velocidadeDaOscilacao = 10;
   
   
   
   
   // Start is called before the first frame update
   void Start()
   {
      Target = GameObject.FindGameObjectWithTag("Player").transform;
      alturaOriginal = transform.position.y;
   }

   // Update is called once per frame
  

   void Update()
   {
      MovimentarInimigo();
      MovimentoSeno();
   }
   
   private void MovimentarInimigo()
   {
       Vector3 movimentar = new Vector3(transform.position.x, Target.position.y);
       transform.position = Vector3.Lerp(transform.position, movimentar, multiplicador * Time.deltaTime);
       transform.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
   }


   void MovimentoSeno()
   {
      angulo += Time.deltaTime * velocidadeDaOscilacao;
      if (angulo >= 360)
      {
         angulo -= 360;
      }


      transform.position = new Vector3(transform.position.x, alturaOriginal + Mathf.Sin(angulo) * amplitude);
   }

   
}



