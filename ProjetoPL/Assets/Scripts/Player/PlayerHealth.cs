using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int vida;    //vari�vel da quantidade de vida
    public int vidamaxima;    //vari�vel da quantidade de vida m�xima

    public Image[] heart;   //array de imagens
    public Sprite cheio;    //imagem do cora��o com vida
    public Sprite vazio;    //imagem do cora��o sem vida

    private bool morto = false; //evita que morra mais de uma vez



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = 3;   //inicializa o player com 3 cora��es de vida
    }

    // Update is called once per frame
    void Update()
    {
        HealthLogic();

        //Verifica se o plyaer morreu
        if (vida <= 0 && !morto)
        {
            Morrer();
        }
    }

    public void HealthLogic()
    {

        //determina que a vida m�xima n�o � ultrapassada
        if (vida > vidamaxima)
        {
            vida = vidamaxima;
        }

        //condi��o que indica qual o n�vel de vida
        for (int i = 0; i < heart.Length; i++)
        {
            if (i < vida)
            {
                heart[i].sprite = cheio;
            }
            else
            {
                heart[i].sprite = vazio;
            }

            if (i < vidamaxima)
            {
                heart[i].enabled = true;
            }
            else
            {
                heart[i].enabled = false;
            }


        }
    }
    private void OnCollisionEnter2D(Collision2D dano)
    {
        //Reduz a vida toda vez que toca em no inimigo
        if (dano.gameObject.tag == "Inimigo")
        {
            vida--;
        }
    }

    void Morrer()
    {
        morto = true;
        //Reiniciar a cena
        SceneManager.LoadScene("TelaReiniciar");
    }
}