using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PuzzleMaberta : MonoBehaviour
{
    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _maletaAberta;
    [SerializeField] private GameObject _vc;
    [SerializeField] private GameObject _maletaModel;

    [SerializeField] private RotacaoDaSala rotacaoDaSala;

    private bool _puzzlesStarts;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_puzzlesStarts == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Sair();
            }
        }
    }
    public void Comecar()
    {
        StartCoroutine("Entrar");
        if (rotacaoDaSala.PuzzleMaberta != null)
        {
            rotacaoDaSala.PuzzleMaberta.enabled = true;
        }
    }

    public void Sair()
    {
        GameManager.Instance.UnPauseGame();
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = true;
            rotacaoDaSala.RestaurarParedesPadrao();
        }
        _maletaAberta.SetActive(true);
        _maletaModel.SetActive(true);
        _vc.SetActive(false);
        _puzzlesStarts = false;
    }
    IEnumerator Entrar()
    {
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = false;
            rotacaoDaSala.ForcarTodasAsParedes(true);
        }
        _vc.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.PauseGame();
        _maletaAberta.SetActive(true);
        _maletaModel.SetActive(true);
        _puzzlesStarts = true;
    }

}
