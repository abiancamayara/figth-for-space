using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZag : MonoBehaviour
{
   public GameObject laserDoInimigo;
   
   public Transform localDoDisparo;
   public Transform localDoDisparoDaEsquerda;
   public Transform localDoDisparoDaDireita;
   
   public int multiplicador;

   public float velocidadeDoInimigo;
   public int vidaMaximaDoInimigo;
   public int vidaAtualDoInimigo;
   
   private Transform Target;

   public float angulo = 0;
   private float cooldownTiro;
   public float balasPorSegundo;

   public float alturaOriginal;
   public int amplitude;
   public int velocidadeDaOscilacao = 10;
   
   public bool inimigoAtirador; 
   public bool inimigoAtivado;
   
   
   // Start is called before the first frame update
   void Start()
   {
      inimigoAtivado = false;
      
      Target = GameObject.FindGameObjectWithTag("Player").transform;
      alturaOriginal = transform.position.y;

      vidaAtualDoInimigo = vidaMaximaDoInimigo;
   }

   // Update is called once per frame
  

   void Update()
   {
      MovimentarInimigo();
      MovimentoSeno();
      if(inimigoAtirador == true && inimigoAtivado == true)
      {
         AtirarLaser(); 
      }
   }
   
   public void AtivarInimigo()
   {
      inimigoAtivado = true;
   }
   
   private void MovimentarInimigo()
   {
       Vector3 movimentar = new Vector3(transform.position.x, Target.position.y);
       transform!.position = Vector3.Lerp(transform.position, movimentar, multiplicador * Time.deltaTime);
       transform!.Translate(Vector3.down * velocidadeDoInimigo * Time.deltaTime);
   }
   
   private void AtirarLaser()
   {
      cooldownTiro -= Time.deltaTime;
      if (cooldownTiro < 0)
      {
         // Tiro duplo
         Instantiate(laserDoInimigo, localDoDisparoDaEsquerda.position, localDoDisparoDaEsquerda.rotation);
         Instantiate(laserDoInimigo, localDoDisparoDaDireita.position, localDoDisparoDaDireita.rotation);
         cooldownTiro += 1 / balasPorSegundo;
      }
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
   
   public void MachucarInimigo(int danoParaReceber)
   {
      vidaAtualDoInimigo -= danoParaReceber;

      if(vidaAtualDoInimigo <= 0)
      {
         Destroy(this.gameObject);
      }
   }
   
}



