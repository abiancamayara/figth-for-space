using UnityEngine;
using TMPro;
using System.Collections;

public class SequenciaDeTexto : MonoBehaviour
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
            "Em uma galáxia distante, além das estrelas mais brilhantes, existe um planeta de riquezas incomparáveis e beleza indescritível: Astrion. " +
            "Localizado na majestosa Galáxia de Andrômeda, Astrion é governado pelo poderoso Império Celestial, " +
            "que explorou durante séculos as vastas fronteiras do espaço, protegendo seus habitantes das ameaças do cosmos.",
            "Mas uma nova escuridão se aproxima. Os temidos Devoradores do Caos, liderados pelo implacável general Zarak, " +
            "emergiram das sombras com um único objetivo: dominar toda a galáxia de Andrômeda e subjugar os pacíficos povos de Astrion. " +
            "As forças do mal avançam, espalhando caos e destruição, e a esperança começa a vacilar.",
            "Em meio à desesperança, surgem três heróis destinados a virar a maré da guerra. Luna, a pilota destemida e precisa, filha de um lendário comandante. " +
            "Luca, o engenheiro brilhante, mestre das tecnologias avançadas e estrategista nato. " +
            "E Ruby, a atiradora de elite, cuja mira impecável é conhecida por toda a galáxia de Andrômeda. " +
            "Juntos, eles formam o Esquadrão Estelar, a última linha de defesa contra os Devoradores do Caos.",
            "Você está prestes a embarcar em uma jornada épica ao lado desses corajosos pilotos. " +
            "Enfrente hordas de inimigos em batalhas espaciais eletrizantes, derrote os comandantes do caos e desafie o temível general Zarak em um confronto decisivo. " +
            "O destino de Astrion, e de toda a galáxia de Andrômeda, está em suas mãos.",
            "Prepare-se. A batalha por Andrômeda começa agora.",
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
