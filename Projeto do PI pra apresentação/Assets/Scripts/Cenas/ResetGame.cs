using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
