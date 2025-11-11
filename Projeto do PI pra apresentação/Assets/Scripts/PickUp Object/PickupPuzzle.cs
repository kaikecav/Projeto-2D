using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickupPuzzle : MonoBehaviour
{
    [Header("Informações do puzile")]
    [SerializeField] private UnityEvent _evento;

    [Header("Inspeção de Objeto")]
    [SerializeField] private PickUp objectPickUp;

    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _pickupInterativo;
    [SerializeField] private GameObject _pickupPuzzle;
    [SerializeField] private GameObject _vc;

    [Header("Rotação da Sala")]
    [SerializeField] private RotacaoDaSala rotacaoDaSala;
    private bool _puzzlesStarts;

    [Header("Sistema da Entidade")]
    public EntitySpawn entitySpawn;

    [Header("Ponto de spawn que causa jumpscare neste puzzle")]
    public Transform puzzleSpawnPoint;

    [Header("Chance de jumpscare (0 = 0%, 1 = 100%)")]
    [Range(0f, 1f)]
    public float jumpscareChance = 0.35f;

    void Update()
    {
        //BLOQUEIO DE SAÍDA DO PUZZLE DURANTE INSPEÇÃO
        if (_puzzlesStarts == true)
        {
            if (objectPickUp != null && objectPickUp.inspecting)
                return;

            if (Input.GetMouseButtonDown(1))
            {
                EndPuzzle();
                return;
            }
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

        _pickupInterativo.SetActive(true);
        _pickupPuzzle.SetActive(false);
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
        _pickupInterativo.SetActive(false);
        _pickupPuzzle.SetActive(true);
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
