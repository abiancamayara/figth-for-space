using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject painelDeGameOver;
    public bool gameOver;
    public TextMeshProUGUI quantidadeCartasText;
    public TextMeshProUGUI contadorLixoText;

    public int Lvalor;
    

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AtualizarQuantidadeCartasUI(int value)
    {
        quantidadeCartasText.text = "Carta: " + value.ToString();
    }
    
    public void AtualizarContadorTexto(int LixoV)
    {
        Lvalor += LixoV;
        contadorLixoText.text ="Lixo: " + Lvalor.ToString();
    }
    public void GameOver()
    {
        gameOver = true;
        painelDeGameOver.SetActive(true);
    }
}
