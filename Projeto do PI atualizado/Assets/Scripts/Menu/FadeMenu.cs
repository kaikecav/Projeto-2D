using UnityEngine;
using System.Collections;


public class MusicFadeIn : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2f;

    void Start()
    {
        audioSource.volume = 0;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0;

        while (time < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 0.6f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0.6f;
    }
}
