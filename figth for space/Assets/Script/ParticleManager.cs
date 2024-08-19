using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject prefabFumacinha;

    private void OnEnable()
    {
        ParticleObserver.ParticleSpawnEvent += SpawanarParticulas;
    }

    private void OnDisable()
    {
        ParticleObserver.ParticleSpawnEvent -= SpawanarParticulas;
    }

    public void SpawanarParticulas(Vector3 posicao)
    {
        Instantiate(prefabFumacinha, posicao, Quaternion.identity);
    }
}
