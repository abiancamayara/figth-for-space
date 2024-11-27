using UnityEngine;
using TMPro;
using System.Collections;

public class Efeitodesumir : MonoBehaviour
{
    public TextMeshProUGUI textUI;  // Referência ao componente TextMeshProUGUI
    public float timeBetweenTexts = 1f;  // Tempo entre cada texto
    public float timeToDisplayText = 2f;  // Tempo para mostrar cada texto

    private void Start()
    {
        // Inicia a sequência de textos
        StartCoroutine(ShowTextSequence());
    }

    private IEnumerator ShowTextSequence()
    {
        string[] texts = {
            "Usando \"Space\" você pode pular essa introdução e as demais cenas de narrativas ",
          
        };

        // Iterando sobre cada texto
        foreach (string text in texts)
        {
            // Exibe o texto atual
            textUI.text = text;

            // Espera o tempo de exibição do texto
            yield return new WaitForSeconds(timeToDisplayText);

            // Apaga o texto
            textUI.text = "";  // Apaga o texto

            // Espera o tempo de intervalo entre os textos
            yield return new WaitForSeconds(timeBetweenTexts);
        }
    }
}
