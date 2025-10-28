using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int vida;    //variável da quantidade de vida
    public int vidamaxima;    //variável da quantidade de vida máxima

    public Image[] heart;   //array de imagens
    public Sprite cheio;    //imagem do coração com vida
    public Sprite vazio;    //imagem do coração sem vida

    private bool morto = false; //evita que morra mais de uma vez



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = 3;   //inicializa o player com 3 corações de vida
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

        //determina que a vida máxima não é ultrapassada
        if (vida > vidamaxima)
        {
            vida = vidamaxima;
        }

        //condição que indica qual o nível de vida
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