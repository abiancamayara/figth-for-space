using UnityEngine;
using TMPro;  // Importando TextMeshPro
using System.Collections;

public class TextSequencer : MonoBehaviour
{
    public TextMeshProUGUI textUI;  // Referência ao componente TextMeshProUGUI
    public float timeBetweenTexts = 2f;  // Tempo entre cada texto
    public float timeToDisplayText = 2f;  // Tempo para mostrar cada texto
    public GameObject SetaDireita;
    public GameObject SetaEsquerda;
    public GameObject SetaCima;
    public GameObject SetaBaixo;
    public GameObject TeclaX;
    public GameObject TeclaZ;

    private void Start()
    {
        // Inicia a sequência de textos
        StartCoroutine(ShowTextSequence());
    }

    private IEnumerator ShowTextSequence()
    {
        string[] texts = {
            "Use as setas direcionais para mover a direita e para esquerda",
            "Use as setas direcionais para mover para cima e para baixo",
            "Use seu ataque secundário pressionando a tecla x",
            "Use seu Dash pressionando a tecla Z",
            "Desvie dos meteoros para não levar dano",
            "Derrotando os inimigos você pode liberar algum power-up",
            "O lixo espacial aparecerá para coleta na próxima fase",
            "Ao coletar 5 lixos espaciais você aumenta sua vida",
            "Ao atingir sua meta de pontuação você libera a carta que contém a localização do primeiro boss",
        };

        // Definindo os GameObjects de controle (sempre que mudar de texto, as setas devem ser desativadas)
        GameObject[] allArrows = { SetaDireita, SetaEsquerda, SetaCima, SetaBaixo };
        GameObject[] allKeys = { TeclaX, TeclaZ };

        // Iterando sobre cada texto
        foreach (string text in texts)
        {
            // Limpar qualquer GameObject ativo de uma rodada anterior
            foreach (var obj in allArrows)
            {
                obj.SetActive(false);
            }
            foreach (var obj in allKeys)
            {
                obj.SetActive(false);
            }

            // Exibe o texto atual
            textUI.text = text;

            // Ativa os objetos correspondentes para o texto atual
            if (text.Contains("direcionais para mover a direita e para esquerda"))
            {
                SetaDireita.SetActive(true);
                SetaEsquerda.SetActive(true);
            }
            else if (text.Contains("direcionais para mover para cima e para baixo"))
            {
                SetaCima.SetActive(true);
                SetaBaixo.SetActive(true);
            }
            else if (text.Contains("pressionando a tecla x"))
            {
                TeclaX.SetActive(true);
            }
            else if (text.Contains("pressionando a tecla Z"))
            {
                TeclaZ.SetActive(true);
            }

            // Espera o tempo de exibição do texto e desativa os objetos no mesmo tempo
            yield return new WaitForSeconds(timeToDisplayText);

            // Apaga o texto e desativa os objetos no mesmo tempo
            textUI.text = "";  // Apaga o texto

            // Desativa os GameObjects que estavam ativados para esse texto
            foreach (var obj in allArrows)
            {
                obj.SetActive(false);
            }
            foreach (var obj in allKeys)
            {
                obj.SetActive(false);
            }

            // Espera o tempo de intervalo entre os textos
            yield return new WaitForSeconds(timeBetweenTexts);
        }
    }
}
