using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BookShelfInteract : MonoBehaviour
{
    [Header("Informações do puzile")]
    [SerializeField] private UnityEvent _evento;

    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _bookshelfInterativo;
    [SerializeField] private GameObject _bookshelf;
    [SerializeField] private GameObject _vc;

    [Header("Rotação da Sala")]
    [SerializeField] private RotacaoDaSala rotacaoDaSala;
    private bool _puzzlesStarts;

    //ENTIDADE
    [Header("Sistema da Entidade")]
    public EntitySpawn entitySpawn;

    [Header("Ponto de spawn que causa jumpscare neste puzzle")]
    public Transform puzzleSpawnPoint;

    [Header("Chance de jumpscare (0 = 0%, 1 = 100%)")]
    [Range(0f, 1f)]
    public float jumpscareChance = 0.35f;

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

        _bookshelfInterativo.SetActive(true);
        _bookshelf.SetActive(false);
        _vc.SetActive(false);
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
        yield return null;      // Aguarda 1 frame para validar posição
        yield return new WaitForSeconds(0.5f);
        _bookshelfInterativo.SetActive(false);
        _bookshelf.SetActive(true);
        _puzzlesStarts = true;

        //JUMPSCARE
        if (entitySpawn != null && puzzleSpawnPoint != null)
        {
            //verifica se a entidade está no mesmo ponto que o puzzle acessado
            if (entitySpawn.IsEntityAt(puzzleSpawnPoint))
            {
                //sorteia o valor para o jumpscare
                float roll = Random.value;
                Debug.Log("[JUMPSCARE CHECK] Sorteio: " + roll);

                //Se o valor for maior que a chance, então acontece o jumpscare
                if (roll <= jumpscareChance)
                {
                    Debug.Log("[JUMPSCARE] Sucesso");
                    yield return entitySpawn.TriggerJumpscare();
                }
                //Se não, sem jumpscare
                else
                {
                    Debug.Log("[JUMPSCARE] Falhou");
                }
            }
            //Se ela estiver em outro puzzle
            else
            {
                Debug.Log("[JUMPSCARE] Entidade não está no spawn deste puzzle.");
            }
        }

        // Pequena espera antes do pause
        yield return new WaitForSecondsRealtime(0.1f);

        // Agora pausa o jogo
        GameManager.Instance.PauseGame();
    }
}

