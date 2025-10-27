using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int vida;
    public int vidamaxima;

    public Image[] heart;
    public Sprite cheio;
    public Sprite vazio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = 3;
    }

    // Update is called once per frame
    void Update()
    {
        HealthLogic();

    }

    void HealthLogic()
    {


        if (vida > vidamaxima)
        {
            vida = vidamaxima;
        }

        for (int i = 0; i < heart.Length; i++)
        {
            if(i<vida)
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
}
