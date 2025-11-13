using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

public class EntidadeCabecaSanidade : MonoBehaviour
{
    [Header("Referências")]
    public SanityManager sanityManager;

    [Header("Sprite da cabeça")]
    public SpriteRenderer cabeca;

    [Header("Aparição (fração da sanidade)")]
    [Range(0f, 1f)] public float inicioVisivel = 0.8f;     // começa a aparecer
    [Range(0f, 1f)] public float totalmenteVisivel = 0.1f; // fica opaca

    [Header("Jumpscare")]
    public bool darJumpscare = true;
    public float distanciaMinima = 0.8f;   // não usamos movimento, mas usamos distância pra zoom
    public float velocidadeZoom = 3f;      // velocidade que ela cresce no jumpscare
    public float tamanhoFinal = 4f;        // tamanho do jumpscare no final

    private bool jumpscareAtivado = false;

    void Start()
    {
        if (cabeca != null)
        {
            Color c = cabeca.color;
            c.a = 0f;
            cabeca.color = c;
        }
    }

    void Update()
    {
        if (sanityManager == null || sanityManager.sanitySlider == null)
            return;

        float fracSanidade = sanityManager.sanitySlider.value / sanityManager.sanitySlider.maxValue;

        // --- Transparência gradual ---
        float alpha = Mathf.InverseLerp(inicioVisivel, totalmenteVisivel, fracSanidade);
        alpha = Mathf.Clamp01(alpha);

        Color c = cabeca.color;
        c.a = alpha;
        cabeca.color = c;

        // --- Jumpscare quando a sanidade acaba ---
        if (fracSanidade <= 0f && !jumpscareAtivado && darJumpscare)
        {
            StartCoroutine(Jumpscare());
        }
    }

    System.Collections.IEnumerator Jumpscare()
    {
        jumpscareAtivado = true;

        // Zoom (a cabeça vai crescendo)
        float tempo = 0f;

        Vector3 tamanhoInicial = transform.localScale;
        Vector3 tamanhoFinalVetor = new Vector3(tamanhoFinal, tamanhoFinal, tamanhoFinal);

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * velocidadeZoom;
            transform.localScale = Vector3.Lerp(tamanhoInicial, tamanhoFinalVetor, tempo);
            yield return null;
        }

        // Aqui você aciona seu game over:
         SceneManager.LoadScene("GameOver");

        Debug.Log("💀 JUMPSCARE DISPARADO!");

        yield break;
    }
}

