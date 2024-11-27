using UnityEngine;
using TMPro;
using System.Collections;

public class Textofinal : MonoBehaviour
{
    public TextMeshProUGUI textUI;  // Referência ao componente TextMeshProUGUI
    public float timeBetweenTexts = 1f;  // Tempo entre cada texto
    public float timeToDisplayText = 2f;  // Tempo para mostrar cada texto
    public float typingSpeed = 0.05f;  // Velocidade do efeito de digitação

    private void Start()
    {
        // Inicia a sequência de textos
        StartCoroutine(ShowTextSequence());
    }

    private IEnumerator ShowTextSequence()
    {
        string[] texts = {
            "Parabéns! Você completou a jornada épica do Esquadrão Estelar e salvou a galáxia de Andrômeda da ameaça dos Devoradores do Caos. " +
            "Sua coragem, habilidade e estratégia levaram à vitória, e o Império Celestial agora pode respirar em paz graças ao seu esforço.",
            "Mas lembre-se: em um universo vasto e imprevisível, a luta pela justiça nunca acaba. " +
            "O esquadrão está sempre pronto para o próximo desafio, e novas aventuras aguardam nas estrelas. " +
            "Você provou ser um verdadeiro guardião da galáxia.",
            "Agora, olhe para o futuro com confiança, sabendo que você fez a diferença. " +
            "Que sua lenda viva em Andrômeda e inspire gerações de heróis.",
            "A batalha pode ter terminado, mas o legado do Esquadrão Estelar continuará a brilhar nas estrelas."
            
        };

        // Iterando sobre cada texto
        foreach (string text in texts)
        {
            // Exibe o texto com o efeito de digitação
            yield return StartCoroutine(TypeText(text));

            // Espera o tempo de exibição do texto completo
            yield return new WaitForSeconds(timeToDisplayText);

            // Apaga o texto após o tempo de exibição
            textUI.text = "";  // Apaga o texto

            // Espera o tempo de intervalo entre os textos
            yield return new WaitForSeconds(timeBetweenTexts);
        }
    }

    // Coroutine para digitar o texto com o efeito digitalizador
    private IEnumerator TypeText(string text)
    {
        textUI.text = "";  // Inicializa o campo de texto vazio

        foreach (char letter in text)
        {
            textUI.text += letter;  // Adiciona uma letra por vez
            yield return new WaitForSeconds(typingSpeed);  // Espera um pouco entre cada letra
        }
    }
}
