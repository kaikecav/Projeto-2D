using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour
{
    [Header("Informações do puzile")]
    [SerializeField] private UnityEvent _evento;

    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _windowInterativo;
    [SerializeField] private GameObject _window;
    [SerializeField] private GameObject _vc;

    [Header("Rotação da Sala")]
    [SerializeField] private RotacaoDaSala rotacaoDaSala;
    private bool _puzzlesStarts;

    //ENTIDADE
    [Header("Sistema da Entidade")]
    public WindowSpawn windowSpawn;

    [Header("Ponto de spawn que causa jumpscare neste puzzle")]
    public Transform puzzleSpawnPoint;

    void Update()
    {
        //Sair do puzzle
        if (Input.GetMouseButtonDown(1))
        {
            EndPuzzle();
            return;
        }
    }

    public void StartPuzzle()
    {
        StartCoroutine(PuzzleStart());
        //Desativa o código LockPuzzleum
        if (rotacaoDaSala.pickupPuzzle != null)
        {
            rotacaoDaSala.pickupPuzzle.enabled = true;
        }
    }

    public void EndPuzzle()
    {
        GameManager.Instance.UnPauseGame();

        //Reativa rotação da sala e restaura paredes normais
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = true;
            rotacaoDaSala.RestaurarParedesPadrao();
        }

        _windowInterativo.SetActive(true);
        _window.SetActive(false);
        _vc.SetActive(false);
        if (windowSpawn != null)
            windowSpawn.Despawn();
        _puzzlesStarts = false;
    }

    IEnumerator PuzzleStart()
    {
        //Desabilita rotação da sala e exibe todas as paredes
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = false;
            rotacaoDaSala.ForcarTodasAsParedes(true);
        }

        _vc.SetActive(true);
        if (windowSpawn != null)
            windowSpawn.TrySpawnEntity();
        yield return null;      // Aguarda 1 frame para validar posição
        yield return new WaitForSeconds(0.5f);
        _windowInterativo.SetActive(false);
        _window.SetActive(true);
        _puzzlesStarts = true;

        //JUMPSCARE
        if (windowSpawn != null)
        {
            yield return windowSpawn.TryJumpscare();
        }

        // Pequena espera antes do pause
        yield return new WaitForSecondsRealtime(0.1f);

        // Agora pausa o jogo
        GameManager.Instance.PauseGame();
    }
}


