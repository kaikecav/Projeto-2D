using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    [Header("Deathscare")]
    public GameObject deathscarePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadReset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator TriggerJumpscare()
    {
        if (deathscarePanel != null)
            deathscarePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
    }
    public void LoadReset()
    {
        StartCoroutine(LoadSceneAsync(0));
    }



    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        asyncLoad.allowSceneActivation = false;

        // (Opcional) Espera um tempinho pra fazer fade ou mostrar tela de transição
        yield return new WaitForSeconds(1.5f);

        asyncLoad.allowSceneActivation = true;
    }

}
