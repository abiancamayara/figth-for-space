using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horda : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab do inimigo
    public float spawnInterval = 0.5f; // Intervalo entre spawn
    public Transform spawnPoint; // Ponto onde os inimigos serão instanciados
    
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval; // Inicializa o tempo do próximo spawn
    }

    void Update()
    {
        // Verifica se é hora de instanciar um novo inimigo
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval; // Atualiza o próximo tempo de spawn
        }
    }

    void SpawnEnemy()
    {
        // Instancia o inimigo na posição do spawnPoint
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

