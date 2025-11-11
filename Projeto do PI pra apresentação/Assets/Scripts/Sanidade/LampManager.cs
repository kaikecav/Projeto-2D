using System.Collections;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    //VARIÁVEIS
    [Header("Configurações da Lâmpada")]
    public float tempoMinimoAntesDePiscar = 60f;
    public float tempoMaximoAntesDePiscar = 180f;
    public float duracaoPiscada = 0.3f;
    public float intervaloMinimo = 10f;
    public float intervaloMaximo = 60f;
    public GameObject luzVisual;

    [Header("Referência de Sanidade")]
    public SanityManager sanityManager;

    private bool apagada = false;

    void Start()
    {
        //Inicia a função desde o começo do jogo
        StartCoroutine(ControlarLampada());
    }

    IEnumerator ControlarLampada()
    {
        //Randomiza o tempo para efetuar a primeira piscada
        float tempoInicial = Random.Range(tempoMinimoAntesDePiscar, tempoMaximoAntesDePiscar);
        yield return new WaitForSeconds(tempoInicial);

        //Randomiza a quantidade de piscadas
        int quantidadePiscadasAleatoria = Random.Range(1, 6);

        for (int i = 0; i < quantidadePiscadasAleatoria; i++)
        {
            //Tempo aleatório entre cada piscada
            float tempoAleatorio = Random.Range(intervaloMinimo, intervaloMaximo);
            yield return new WaitForSeconds(tempoAleatorio);

            //Apaga luz e começa a reduzir sanidade
            AlternarLuz(false);
            apagada = true;
            sanityManager.StartLosingSanity();

            yield return new WaitForSeconds(duracaoPiscada);

            //Acende luz e para a redução de sanidade
            AlternarLuz(true);
            apagada = false;
            sanityManager.StopLosingSanity();
        }

        //Lâmpada apaga permanentemente
        AlternarLuz(false);
        apagada = true;
        sanityManager.StartLosingSanity();

        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    //FUNÇÃO PARA MUDAR O ESTADO DA LUZ
    void AlternarLuz(bool estado)
    {
        if (luzVisual != null)
            luzVisual.SetActive(estado);
        else
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.enabled = estado;
        }
    }

    public bool EstaApagada() => apagada;

    //FUNÇÃO PARA REACENDER A LÂMPADA
    public void Reacender()
    {
        if (!apagada) return;

        //Ativa a Lâmpada e a luz e faz parar de perder sanidade
        gameObject.SetActive(true);
        apagada = false;
        AlternarLuz(true);
        sanityManager.StopLosingSanity();

        //Reseta o ciclo da lâmpada
        StopAllCoroutines();
        StartCoroutine(ControlarLampada());
    }
}
