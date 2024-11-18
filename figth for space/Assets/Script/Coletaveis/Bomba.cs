using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bomba : MonoBehaviour
{
    // Lista de tags dos inimigos normais
    private List<string> tagsDosInimigos = new List<string> { "Inimigo", "Kamikaze" };
    // Lista de tags dos bosses
    private List<string> tagsDosBosses = new List<string> { "Dracon", "Glaucius", "Zarak" };

    public void AtivarBomba()
    {
        // Destroi todos os objetos com as tags dos inimigos normais
        foreach (var tag in tagsDosInimigos)
        {
            GameObject[] inimigos = GameObject.FindGameObjectsWithTag(tag);

            foreach (var item in inimigos)
            {
                Destroy(item);
            }
        }

        // Reduz a vida dos bosses
        foreach (var tag in tagsDosBosses)
        {
            GameObject[] bosses = GameObject.FindGameObjectsWithTag(tag);

            foreach (var boss in bosses)
            {
                if (tag == "Dracon")
                {
                    BossDracon draconBoss = boss.GetComponent<BossDracon>();
                    if (draconBoss != null)
                    {
                        draconBoss.MachucarBoss(10);  // Reduz a vida em 10 (ajuste conforme necessário)
                    }
                }
                else if (tag == "Glaucius")
                {
                    BossGlaucius glauciusBoss = boss.GetComponent<BossGlaucius>();
                    if (glauciusBoss != null)
                    {
                        glauciusBoss.MachucarBoss(10);  // Reduz a vida em 10 (ajuste conforme necessário)
                    }
                }
                else if (tag == "Zarak")
                {
                    BossZarak zarakBoss = boss.GetComponent<BossZarak>();
                    if (zarakBoss != null)
                    {
                        zarakBoss.MachucarBoss(10);  // Reduz a vida em 10 (ajuste conforme necessário)
                    }
                }
            }
        }

        // Destroi todos os objetos com a tag "Laser"
        GameObject[] laserdoinimigo = GameObject.FindGameObjectsWithTag("Laser");

        foreach (var item in laserdoinimigo)
        {
            Destroy(item);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AtivarBomba();
            Destroy(gameObject);
        }
    }
}
