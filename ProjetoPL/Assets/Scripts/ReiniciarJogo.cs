using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarJogo : MonoBehaviour
{
    void Update()
    {
        // Se o jogador clicar em qualquer lugar da tela
        if (Input.GetMouseButtonDown(0))
        {
            // Carrega a fase principal (substitua "Fase1" se o nome for outro)
            SceneManager.LoadScene("Fase1");
        }
    }
}
