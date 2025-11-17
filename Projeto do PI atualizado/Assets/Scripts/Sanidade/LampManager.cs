using System.Collections;
using UnityEngine;

public class LampManager : Interactable
{
    [Header("Configurações da Lâmpada")]
    public float tempoMinimoAntesDePiscar = 60f;
    public float tempoMaximoAntesDePiscar = 180f;
    public float duracaoPiscada = 0.3f;
    public float intervaloMinimo = 10f;
    public float intervaloMaximo = 60f;
    public GameObject luzVisual;
    public GameObject lamp;
    public GameObject lampBroken;

    [Header("Referência de Sanidade")]
    public SanityManager sanityManager;

    [Header("Sons da Lâmpada")]
    public AudioSource somPiscar;     // som curto ao piscar
    public AudioSource somApagar;     // som da lâmpada queimando

    private bool apagada = false;

    void Start()
    {
        StartCoroutine(ControlarLampada());
    }

    IEnumerator ControlarLampada()
    {
        float tempoInicial = Random.Range(tempoMinimoAntesDePiscar, tempoMaximoAntesDePiscar);
        yield return new WaitForSeconds(tempoInicial);

        int quantidadePiscadasAleatoria = Random.Range(1, 6);

        for (int i = 0; i < quantidadePiscadasAleatoria; i++)
        {
            float tempoAleatorio = Random.Range(intervaloMinimo, intervaloMaximo);
            yield return new WaitForSeconds(tempoAleatorio);

            // Apaga (piscada)
            AlternarLuz(false);
            apagada = true;
            sanityManager.StartLosingSanity();

            //  SOM DE PISCAR
            if (somPiscar != null)
                somPiscar.Play();

            yield return new WaitForSeconds(duracaoPiscada);

            // Acende (piscada)
            AlternarLuz(true);
            apagada = false;
            sanityManager.StopLosingSanity();
        }

        // APAGA PERMANENTEMENTE
        AlternarLuz(false);

        apagada = true;
        sanityManager.StartLosingSanity();

        //  SOM APAGAR DEFINITIVO
        if (somApagar != null)
            somApagar.Play();

        yield return new WaitForSeconds(0.2f);

        lamp.SetActive(false);
        lampBroken.SetActive(true);
    }

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

    public override void Interact()
    {
        if (apagada == true)
        {
            // Pega o item ATUALMENTE SELECIONADO (que agora deve conter a chave)
            Item selectedItem = Inventory.instance.GetSelectedItem();

            // Checagem: O item selecionado é a chave correta?
            if (selectedItem != null && selectedItem == conditionalItem)
            {
                // SUCESSO - ABERTO!
                Inventory.instance.RemoveItem(conditionalItem);
                Reacender();
            }
            else
            {
                // FALHA: (selectedItem é nulo ou o item errado)
                Debug.Log("Luz acesa ou não é uma lâmpada!");
            }
        }
        // ... (OpenDoor continua igual)
    }

    public void Reacender()
    {
        if (!apagada) return;

        lamp.SetActive(true);
        lampBroken.SetActive(false);
        apagada = false;
        AlternarLuz(true);
        sanityManager.StopLosingSanity();

        //  Som opcional ao reacender (se quiser)
         if (somPiscar != null) somPiscar.Play();

        StopAllCoroutines();
        StartCoroutine(ControlarLampada());
    }
}
