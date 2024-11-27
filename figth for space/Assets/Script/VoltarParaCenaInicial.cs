using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para manipular cenas

public class VoltarParaCenaInicial : MonoBehaviour
{
    public void VoltarParaCena()
    {
        SceneManager.LoadScene("menu principal");  // Substitua pelo nome da sua primeira cena
    }
}

