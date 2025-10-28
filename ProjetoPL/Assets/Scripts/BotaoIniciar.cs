using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoIniciar : MonoBehaviour
{
    //Variável de identificação
    [SerializeField] private string nomeDoLevelDeJogo;

    public void Jogar()     //Muda para a Fase determinada
    {
        SceneManager.LoadScene("Fase1");
    }

    public void SairJogo()      //Fecha o jogo
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
        Debug.Log("Jogo fechado");
    }
}
