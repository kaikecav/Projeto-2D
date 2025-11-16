using UnityEngine;

public class SanitySoundController : MonoBehaviour
{
    public SanityManager sanityManager;   // arrastar o SanityManager aqui
    public AudioSource criticalLoop;       // arrastar o som aqui
    public float criticalThreshold = 9000f; // coloque o valor crítico (ajuste depois)

    void Update()
    {
        float currentSanity = sanityManager.sanitySlider.value; // pega a sanidade real

        if (currentSanity <= criticalThreshold)
        {
            if (!criticalLoop.isPlaying)
                criticalLoop.Play();
        }
        else
        {
            if (criticalLoop.isPlaying)
                criticalLoop.Stop();
        }
    }
}
