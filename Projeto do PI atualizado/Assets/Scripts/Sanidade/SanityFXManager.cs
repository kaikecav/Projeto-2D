using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SanityFXManager : MonoBehaviour
{
    //VARIÁVEIS
    [Header("Referência ao Slider de Sanidade (0-100)")]
    public Slider sanitySlider;

    [Header("Volume do Post Processing (Global Volume)")]
    public Volume postProcessVolume;

    //Overrides
    private Vignette vignette;
    private ChromaticAberration chroma;
    private FilmGrain grain;
    private LensDistortion distortion;

    [Header("Intensidades Máximas")]
    public float maxVignette = 1f;
    public float maxChromatic = 0.8f;
    public float maxGrain = 0.7f;
    public float maxDistortion = -0.5f;

    [Header("Oscilação do efeito (Wobble)")]
    public float wobbleSpeed = 3f;
    public float wobbleIntensity = 0.02f;

    private void Start()
    {
        //Pega os efeitos do Volume
        postProcessVolume.profile.TryGet(out vignette);
        postProcessVolume.profile.TryGet(out chroma);
        postProcessVolume.profile.TryGet(out grain);
        postProcessVolume.profile.TryGet(out distortion);
    }

    private void Update()
    {
        //Pega o valor da sanidade
        float sanity01 = sanitySlider.value / sanitySlider.maxValue;
        float inverted = 1f - sanity01; //0 = normal, 1 = insano

        //Vignette
        vignette.intensity.value = Mathf.Lerp(0f, maxVignette, inverted);

        //Aberração Cromática
        chroma.intensity.value = Mathf.Lerp(0f, maxChromatic, inverted);

        //Grain
        grain.intensity.value = Mathf.Lerp(0f, maxGrain, inverted);

        //Lens Distortion com wobble para dar um efeito de “respiração”
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleIntensity * inverted;
        distortion.intensity.value = Mathf.Lerp(0f, maxDistortion, inverted) + wobble;

        //Segurança
        distortion.scale.value = 1f;
    }
}
