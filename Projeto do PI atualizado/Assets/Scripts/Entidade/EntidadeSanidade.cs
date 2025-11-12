using UnityEngine;
using UnityEngine.SceneManagement; // opcional: carregar tela de Game Over

public class EntidadeSanidade : MonoBehaviour
{
    [Header("Referências")]
    public SanityManager sanityManager;
    [Tooltip("Alvo da entidade (pode ser a Casa, Câmera ou Player)")]
    public Transform alvo; // agora pode ser a câmera

    [Header("Partes Visuais")]
    public GameObject olhos;
    public GameObject cabeca;
    public GameObject corpo;

    [Header("Limiar de Aparição (fração da sanidade)")]
    [Range(0f, 1f)] public float limOlhos = 0.7f;
    [Range(0f, 1f)] public float limCabeca = 0.4f;
    [Range(0f, 1f)] public float limCorpoMove = 0.1f;

    [Header("Movimento")]
    [Tooltip("Velocidade base; aumenta conforme a sanidade cai")]
    public float velocidadeBase = 0.02f;
    [Tooltip("Distância mínima para acionar o Game Over")]
    public float alcanceGameOver = 0.6f;
    public bool olharParaAlvo = true;

    [Header("Efeito Final")]
    [Tooltip("Se verdadeiro, a entidade aumenta de tamanho perto do alvo (efeito jumpscare)")]
    public bool efeitoJumpscare = true;
    public float aumentoEscala = 2f;
    public float velocidadeAumento = 1.5f;

    private bool jaPerdeu = false;
    private bool corpoAtivo = false;

    void Awake()
    {
        SetPartes(false, false, false);
    }

    void Update()
    {
        if (sanityManager == null || sanityManager.sanitySlider == null)
            return;

        float fracSanidade = sanityManager.sanitySlider.value / sanityManager.sanitySlider.maxValue;

        // Menor sanidade → mais aparição
        if (fracSanidade <= limCorpoMove)
        {
            // Ativa só o corpo e começa o movimento
            if (!corpoAtivo)
            {
                SetPartes(false, false, true); // desativa olhos e cabeça
                corpoAtivo = true;
            }
            MoverAteAlvo();
        }
        else if (fracSanidade <= limCabeca)
        {
            SetPartes(true, true, false);
            corpoAtivo = false;
        }
        else if (fracSanidade <= limOlhos)
        {
            SetPartes(true, false, false);
            corpoAtivo = false;
        }
        else
        {
            SetPartes(false, false, false);
            corpoAtivo = false;
        }
    }

    void SetPartes(bool olhosAtivos, bool cabecaAtiva, bool corpoAtivo)
    {
        if (olhos != null) olhos.SetActive(olhosAtivos);
        if (cabeca != null) cabeca.SetActive(cabecaAtiva);
        if (corpo != null) corpo.SetActive(corpoAtivo);
    }

    void MoverAteAlvo()
    {
        if (alvo == null || jaPerdeu) return;

        // Calcula intensidade da insanidade (0 = cheio, 1 = vazio)
        float fracSanidade = sanityManager.sanitySlider.value / sanityManager.sanitySlider.maxValue;
        float intensidade = Mathf.Clamp01(1f - fracSanidade);

        // Velocidade aumenta conforme insanidade cresce
        float velocidadeAtual = velocidadeBase * (0.3f + intensidade * 0.7f);

        transform.position = Vector3.MoveTowards(
            transform.position,
            alvo.position,
            velocidadeAtual * Time.deltaTime
        );

        // Vira para o alvo (somente no eixo X)
        if (olharParaAlvo)
        {
            Vector3 escala = transform.localScale;
            escala.x = alvo.position.x < transform.position.x ? -Mathf.Abs(escala.x) : Mathf.Abs(escala.x);
            transform.localScale = escala;
        }

        // Efeito de "crescer" ao se aproximar
        if (efeitoJumpscare)
        {
            float distancia = Vector3.Distance(transform.position, alvo.position);
            float t = Mathf.InverseLerp(5f, 0.5f, distancia); // quanto mais perto, maior
            float escala = Mathf.Lerp(1f, aumentoEscala, t);
            transform.localScale = new Vector3(escala, escala, escala);
        }

        // Chegou no alvo (Game Over)
        float dist = Vector3.Distance(transform.position, alvo.position);
        if (dist <= alcanceGameOver)
        {
            jaPerdeu = true;
            Debug.Log("💀 Game Over — a entidade alcançou o jogador!");
            // SceneManager.LoadScene("GameOver");
            // ou: Time.timeScale = 0f;
        }
    }
}

