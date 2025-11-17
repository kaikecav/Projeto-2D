using UnityEngine;
using UnityEngine.UI;

public class Modificarvelocidade : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        // Carrega o valor salvo anteriormente, se existir
        float savedValue = PlayerPrefs.GetFloat("DragRotationSpeed", 100f);
        slider.value = savedValue;

        // Atualiza sempre que o slider mudar
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("DragRotationSpeed", value);
        PlayerPrefs.Save();
    }
}
