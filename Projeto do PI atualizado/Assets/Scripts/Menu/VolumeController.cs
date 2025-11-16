using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Start()
    {
        // Carrega o valor salvo (0 a 1), padrão 0.5 (meio do slider)
        float saved = PlayerPrefs.GetFloat("volume", 0.5f);

        slider.value = saved;
        AjustarVolume(saved);
    }

    public void AjustarVolume(float sliderValue)
    {
        // Converte o slider (0–1) para dB (-80 a 0)
        float volumeDB = Mathf.Lerp(-80f, 0f, sliderValue);

        // IMPORTANTE: usar o nome REAL do parâmetro exposto
        mixer.SetFloat("MyExposedParam", volumeDB);

        PlayerPrefs.SetFloat("volume", sliderValue);
    }
}
