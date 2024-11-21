using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class VidaDosPlayers : MonoBehaviour
{
    public Slider barraDeVidaDoJogador;
    public Slider barraDeEnergiaDoEscudo;
    public GameObject escudoDoJogador;

    public int vidaMaximaDoJogador;
    public int vidaAtualDojogador;
    public int vidaMaximaDoEscudo;
    public int vidaAtualDoEscudo;

    public bool temEscudo;
    [SerializeField] private Animator anim1;
    [SerializeField] private Animator anim2;
    private Animator animator;
    [SerializeField] private Animator anim3;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.vidaDosPlayers = this;
        vidaAtualDojogador = vidaMaximaDoJogador;
        vidaAtualDoEscudo = vidaMaximaDoEscudo;

        barraDeVidaDoJogador.maxValue = vidaMaximaDoJogador;
        barraDeVidaDoJogador.value = vidaAtualDojogador;

        barraDeEnergiaDoEscudo.maxValue = vidaMaximaDoEscudo;
        barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;

        barraDeEnergiaDoEscudo.gameObject.SetActive(false);
        DesativarEscudo();

        animator = GetComponent<Animator>();  // Inicializa o Animator
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AtivarEscudo()
    {
        barraDeEnergiaDoEscudo.gameObject.SetActive(true);
        vidaAtualDoEscudo = vidaMaximaDoEscudo;
        barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;
        escudoDoJogador.SetActive(true);
        temEscudo = true;
    }

    public void DesativarEscudo()
    {
        escudoDoJogador.SetActive(false);
        barraDeEnergiaDoEscudo.gameObject.SetActive(false);
        temEscudo = false;
    }

    public void GanharVida(int vidaParaReceber)
    {
        if (vidaAtualDojogador + vidaParaReceber <= vidaMaximaDoJogador)
        {
            vidaAtualDojogador += vidaParaReceber;
        }
        else
        {
            vidaAtualDojogador = vidaMaximaDoJogador;
        }

        barraDeVidaDoJogador.value = vidaAtualDojogador;
    }

    public void MachucarJogador(int danoParaReceber)
    {
        if (temEscudo)
        {
            // Primeiro aplica o dano ao escudo
            vidaAtualDoEscudo -= danoParaReceber;
            barraDeEnergiaDoEscudo.value = vidaAtualDoEscudo;

            // Se a vida do escudo for menor ou igual a 0, desativa o escudo
            if (vidaAtualDoEscudo <= 0)
            {
                DesativarEscudo();
                // Se sobrar dano, aplica ao jogador
                int danoRestante = Mathf.Abs(vidaAtualDoEscudo);  // danoRestante é o valor absoluto da vida negativa do escudo
                vidaAtualDojogador -= danoRestante;
                barraDeVidaDoJogador.value = vidaAtualDojogador;
            }
        }
        else
        {
            // Aplica o dano ao jogador se não há escudo
            vidaAtualDojogador -= danoParaReceber;
            barraDeVidaDoJogador.value = vidaAtualDojogador;
        }

        // Verifica se a vida do jogador é menor ou igual a 0
        if (vidaAtualDojogador <= 0)
        {
            PlayerLuna luna = GetComponent<PlayerLuna>();
            PlayerLuca luca = GetComponent<PlayerLuca>();
            PlayerRuby ruby = GetComponent<PlayerRuby>();
            barraDeEnergiaDoEscudo.gameObject.SetActive(false);
            barraDeVidaDoJogador.gameObject.SetActive(false);

            SetTransition(Transition.Death);
            if (luna != null)
                luna.Morreu();  // Chama a animação de dano do Player Luna

            if (luca != null)
                luca.Morreu();  // Chama a animação de dano do Player Luca

            if (ruby != null)
                ruby.Morreu(); 

        }
        else
        {
            // Chama a animação de dano para os jogadores
            PlayerLuna luna = GetComponent<PlayerLuna>();
            PlayerLuca luca = GetComponent<PlayerLuca>();
            PlayerRuby ruby = GetComponent<PlayerRuby>();

            if (luna != null)
                luna.ReceiveDamage();  // Chama a animação de dano do Player Luna

            if (luca != null)
                luca.ReceiveDamage();  // Chama a animação de dano do Player Luca

            if (ruby != null)
                ruby.ReceiveDamage();  // Chama a animação de dano do Player Ruby
        }
    }

    // Função que define a transição de animação de acordo com o estado
    public void SetTransition(Transition transition)
    {
        // esse aqui funciona melhor
        if (animator == null) return;  // Certifique-se de que o Animator está presente
     
             switch (transition)
             {
                 case Transition.Hit:
                     animator.SetTrigger("TakeDamage");  // Transição para animação de dano
                     break;
                 case Transition.Death:
                     Debug.Log("Chamou morrendo");
                     animator.SetTrigger("morrendo");  
                     break;
               Debug.Log("chamou o transition");
          default:
                // Se não for nenhum dos tipos conhecidos, logue ou trate de outra forma
                Debug.LogWarning("Tipo de jogador não reconhecido para animação de morte.");
                break;
        }
    }


}

public enum Transition
{
    Hit = 2,   // Animação de dano
    Death = 3  // Animação de morte
}
