using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AtaqueRuby : MonoBehaviour
{
        public float tempoAntesDeExplodir = 3f; // Tempo em segundos até a bomba explodir
        public float raioExplosao = 5f; // Raio da explosão
        public PlayerRuby player;
        
        public float velocidadeDoLaser; 
        public int danoParaDar;

        private void Start()
        {
            Invoke("Explodir", tempoAntesDeExplodir);
        }
        
        void Update()
        {
            MovimentarLaser(); 
        }

        private void Explodir()
        {
            // Lógica de explosão
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, raioExplosao);
            foreach (Collider2D collider in colliders)
            {
                // Aqui você pode verificar se o collider é um inimigo e aplicar dano
                // Por exemplo:
                // if(collider.CompareTag("Inimigo"))
                // {
                //     // Aplique dano ao inimigo
                // }
            }

            // Destrói a bomba após a explosão
            player.lancou = false;
            Destroy(gameObject);
        }
        
        private void MovimentarLaser()
        {
            transform.Translate(Vector3.up * velocidadeDoLaser * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Inimigo"))
            {
                other.gameObject.GetComponent<Inimigos>().MachucarInimigo(danoParaDar);
                Destroy(this.gameObject);
            }
            if(other.gameObject.CompareTag("Kamikaze"))
            {
                other.gameObject.GetComponent<Kamikaze>().MachucarInimigo(danoParaDar);
                Destroy(this.gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioExplosao);
        }
}
