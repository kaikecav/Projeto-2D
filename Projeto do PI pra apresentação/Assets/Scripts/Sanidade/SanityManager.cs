using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    //VARIÁVEIS
    [Header("Slider de Sanidade")]
    public Slider sanitySlider;
    public int fullSanity = 12000;
    public int difficulty = 24;

    [Header("URP Global Volume")]
    public Volume volume;

    //Efeitos URP
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

    public UnityEvent onInsane;

    void Start()
    {
        //Pega cada elemento da URP salvo
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromatic);
        volume.profile.TryGet(out grain);
        volume.profile.TryGet(out distortion);

        //Tenta pegar o Slider
        if (sanitySlider == null)
            sanitySlider = GetComponent<Slider>();

        sanitySlider.maxValue = fullSanity; //Valor máximo da sanidade
        sanitySlider.value = fullSanity;    //Inicia com o valor da sanidade seno o valor máximo

        ResetEffects();
    }

    //FUNÇÃO PARA RESETAR OS EFEITOS AO ESTADO INICIAL
    void ResetEffects()
    {
        if (vignette != null) vignette.intensity.value = 0f;
        if (chromatic != null) chromatic.intensity.value = 0f;
        if (grain != null) grain.intensity.value = 0f;
        if (distortion != null) distortion.intensity.value = 0f;
    }

    //FUNÇÃO PARA INICIAR A PERDA DE SANIDADE
    public void StartLosingSanity()
    {
        if (!isLosingSanity)
            sanityRoutine = StartCoroutine(LoseSanity());
    }

    //FUNÇÃO PARA PARAR DE PERDER SANIDADE
    public void StopLosingSanity()
    {
        if (isLosingSanity && sanityRoutine != null)
        {
            StopCoroutine(sanityRoutine);
            sanityRoutine = null;
            isLosingSanity = false;
        }
    }

    //FUNÇÃO QUE EXECUTA A PERDA DE SANIDADE
    IEnumerator LoseSanity()
    {
        //Se estiver ativado
        isLosingSanity = true;

        //Enquanto a sanidade for maior que 0, começa a perder a sanidade
        while (sanitySlider.value > 0)
        {
            sanitySlider.value -= 2f * difficulty * Time.deltaTime * 10f;
            sanitySlider.value = Mathf.Clamp(sanitySlider.value, 0, sanitySlider.maxValue);

            percent = 1f - (sanitySlider.value / sanitySlider.maxValue);

            ApplyEffects(percent);

            yield return null;
        }

        //Se estiver louco, ou seja, sanidade zerada
        onInsane.Invoke();
        isLosingSanity = false;
        sanityRoutine = null;
    }

    //FUNÇÃO PARA APLICAR OS EFEITOS
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

    //FUNÇÃO PARA QUE OUTROS ASSETS AFETEM A SANIDADE
    public void AffectSanity(float value)
    {
        sanitySlider.value = Mathf.Clamp(sanitySlider.value + value, 0, sanitySlider.maxValue);
    }
}
