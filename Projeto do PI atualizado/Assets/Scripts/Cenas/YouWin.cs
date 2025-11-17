using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouWin : MonoBehaviour
{
    [Header("Fim do Jogo")]
    public GameObject fimPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadResetWin();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator TriggerJumpscare()
    {
        if (fimPanel != null)
            fimPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
    }
    public void LoadResetWin()
    {
        StartCoroutine(LoadSceneAsync(0));
    }



    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        asyncLoad.allowSceneActivation = false;

        // (Opcional) Espera um tempinho pra fazer fade ou mostrar tela de transição
        yield return new WaitForSeconds(45f);

        asyncLoad.allowSceneActivation = true;
    }

}
