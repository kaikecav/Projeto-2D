using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SanityManager : MonoBehaviour
{
    // =============================
    // VARIÁVEIS DE SANIDADE
    // =============================
    [Header("Slider de Sanidade")]
    public Slider sanitySlider;
    public int fullSanity = 12000;
    public int difficulty = 24;

    [Header("URP Global Volume")]
    public Volume volume;

    // Efeitos URP
    private Vignette vignette;
    private ChromaticAberration chromatic;
    private FilmGrain grain;
    private LensDistortion distortion;

    [Header("Valores Máximos dos Efeitos")]
    public float maxVignette = 1f;
    public float maxChromatic = 1f;
    public float maxGrain = 1f;
    public float maxDistortion = -0.5f;

    private float percent;
    private bool isLosingSanity = false;
    private Coroutine sanityRoutine;

    [Header("Lampada")]
    public LampManager lampManager;

    // =============================
    // JUMPSCARE / GAME OVER CUSTOM
    // =============================
    [Header("Jumpscare")]
    public GameObject jumpscarePanel;        // painel que contém o vídeo
    public VideoPlayer jumpscareVideo;       // componente VideoPlayer
    public float delayBeforeJumpscare = 1f;  // tempo antes de aparecer o painel
    public float delayAfterVideo = 0.5f;     // tempo após o vídeo terminar
    public string menuSceneName = "Menu(provisorio)";    // nome da cena do menu

    public UnityEvent onInsane;

    void Start()
    {
        // Recupera os efeitos no Volume
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromatic);
        volume.profile.TryGet(out grain);
        volume.profile.TryGet(out distortion);

        // Configura slider
        if (sanitySlider == null)
            sanitySlider = GetComponent<Slider>();

        sanitySlider.maxValue = fullSanity;
        sanitySlider.value = fullSanity;

        ResetEffects();

        // Certifica que o painel começa desligado
        if (jumpscarePanel != null)
            jumpscarePanel.SetActive(false);

        // Prepara evento do VideoPlayer
        if (jumpscareVideo != null)
            jumpscareVideo.loopPointReached += OnVideoFinished;
    }

    // Reset dos efeitos
    void ResetEffects()
    {
        if (vignette != null) vignette.intensity.value = 0f;
        if (chromatic != null) chromatic.intensity.value = 0f;
        if (grain != null) grain.intensity.value = 0f;
        if (distortion != null) distortion.intensity.value = 0f;
    }

    // Inicia perda de sanidade
    public void StartLosingSanity()
    {
        if (!isLosingSanity)
            sanityRoutine = StartCoroutine(LoseSanity());
    }

    // Para perda de sanidade
    public void StopLosingSanity()
    {
        if (isLosingSanity && sanityRoutine != null)
        {
            StopCoroutine(sanityRoutine);
            sanityRoutine = null;
            isLosingSanity = false;
        }
    }

    IEnumerator LoseSanity()
    {
        isLosingSanity = true;

        while (sanitySlider.value > 0)
        {
            sanitySlider.value -= 2f * difficulty * Time.deltaTime * 10f;
            sanitySlider.value = Mathf.Clamp(sanitySlider.value, 0, sanitySlider.maxValue);

            percent = 1f - (sanitySlider.value / sanitySlider.maxValue);

            ApplyEffects(percent);

            yield return null;
        }

        // SANIDADE ZERADA → INICIA SEQUÊNCIA
        onInsane.Invoke();
        StartCoroutine(InsanitySequence());

        isLosingSanity = false;
        sanityRoutine = null;
    }

    // Aplica os efeitos URP
    void ApplyEffects(float p)
    {
        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(0f, maxVignette, p);

        if (chromatic != null)
            chromatic.intensity.value = Mathf.Lerp(0f, maxChromatic, p);

        if (grain != null)
            grain.intensity.value = Mathf.Lerp(0f, maxGrain, p);

        if (distortion != null)
            distortion.intensity.value = Mathf.Lerp(0f, maxDistortion, p);
    }

    // Para outros scripts alterarem a sanidade
    public void AffectSanity(float value)
    {
        sanitySlider.value = Mathf.Clamp(sanitySlider.value + value, 0, sanitySlider.maxValue);
    }

    // =============================
    //  SEQUÊNCIA FINAL (JUMPSCARE)
    // =============================
    IEnumerator InsanitySequence()
    {
        // Espera inicial (antes de mostrar o painel)
        yield return new WaitForSeconds(delayBeforeJumpscare);

        // Liga o painel
        jumpscarePanel.SetActive(true);

        // Começa o vídeo
        if (jumpscareVideo != null)
            jumpscareVideo.Play();
    }

    // Chamado automaticamente quando o vídeo termina
    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(LoadMenuAfterDelay());
    }

    IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterVideo);
        SceneManager.LoadScene(menuSceneName);
    }
}
