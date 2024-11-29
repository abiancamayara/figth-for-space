using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class SequenciaDeTexto : MonoBehaviour
{
    public TextMeshProUGUI textUI;  // Referência ao componente TextMeshProUGUI
    public float timeBetweenTexts = 1f;  // Tempo entre cada texto
    public float timeToDisplayText = 2f;  // Tempo para mostrar cada texto
    public float typingSpeed = 0.05f;  // Velocidade do efeito de digitação
    public float timeBeforeAutoLoad = 2f;  // Tempo antes da mudança automática para a próxima cena (após o último texto)

    private void Start()
    {
        // Inicia a sequência de textos
        StartCoroutine(ShowTextSequence());
    }
    
    void Update()
    {
        // Verifica se a tecla "Space" foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CarregarCena();
        }
    }
    
    void CarregarCena()
    {
        // Carregar a próxima cena
        SceneManager.LoadScene("Opções");  // Substitua "Opções" pelo nome da sua cena
    }

    private IEnumerator ShowTextSequence()
    {
        string[] texts = {
            "Em uma galáxia distante, além das estrelas mais brilhantes, existe um planeta de riquezas incomparáveis: Astrion. " +
            "Localizado na Galáxia de Andrômeda, Astrion é governado pelo poderoso Império Celestial, " +
            "que protege seus habitantes das ameaças do cosmos.",
            "Mas uma nova escuridão se aproxima. Os Devoradores do Caos, liderados pelo general Zarak, " +
            "emergem das sombras para dominar a galáxia e subjugar Astrion.",
            "Em meio à desesperança, surgem três heróis: Luna, a pilota destemida, Luca, o engenheiro brilhante, " +
            "e Ruby, a atiradora de elite. Juntos, eles formam o Esquadrão Estelar, a última linha de defesa.",
            "Prepare-se para batalhas espaciais, enfrente hordas de inimigos e desafie o general Zarak. " +
            "O destino de Astrion e de Andrômeda está em suas mãos.",
            "Prepare-se. A batalha pela defesa de Andrômeda começa agora.",
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

        // Depois que todos os textos foram exibidos, aguarda o tempo antes de carregar a próxima cena automaticamente
        yield return new WaitForSeconds(timeBeforeAutoLoad);

        // Carregar a próxima cena
        CarregarCena();
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
