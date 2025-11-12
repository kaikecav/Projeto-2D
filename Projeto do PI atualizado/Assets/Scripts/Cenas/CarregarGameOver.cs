using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarregarGameOver : MonoBehaviour
{
    public Image fadeImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGameOver()
    {
        StartCoroutine(LoadSceneAsync("GameOver"));
        StartCoroutine(FadeAndLoad("GameOver"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // (Opcional) Espera um tempinho pra fazer fade ou mostrar tela de transição
        yield return new WaitForSeconds(1.5f);

        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        Color c = fadeImage.color;
        float t = 0f;

        // Fade para preto
        while (t < 1f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t);
            fadeImage.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Carrega a nova cena de forma assíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;
    }
}
