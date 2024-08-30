using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AtaqueRuby : MonoBehaviour
{
        public float tempoAntesDeExplodir = 3f; // Tempo em segundos até a bomba explodir
        public float raioExplosao = 5f; // Raio da explosão

        private void Start()
        {
            Invoke("Explodir", tempoAntesDeExplodir);
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
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioExplosao);
        }
}
