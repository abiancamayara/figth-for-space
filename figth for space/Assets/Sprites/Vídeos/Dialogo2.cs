using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Dialogo2 : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // O VideoPlayer que está tocando o vídeo

    void Start()
    {
        // Se o VideoPlayer não estiver atribuído, encontra o componente automaticamente
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Associa o evento de término do vídeo
        videoPlayer.loopPointReached += EndReached;
    }

    void Update()
    {
        // Verifica se a tecla "Space" foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CarregarCena();
        }
    }

    // Este método será chamado quando o vídeo terminar ou ao pressionar a tecla "Space"
    void EndReached(VideoPlayer vp)
    {
        CarregarCena();
    }

    void CarregarCena()
    {
        // Carregar a próxima cena
        SceneManager.LoadScene("Ruby");  // Substitua "Luca" pelo nome da sua cena
    }
}
