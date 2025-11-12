using UnityEngine;

public class Destruicao : MonoBehaviour
{
    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _lockInterativo;
    [SerializeField] private GameObject _lockPuzzle;
    [SerializeField] private GameObject _vc;
    public void SelfDestroy()
{
        GameManager.Instance.UnPauseGame();
        _lockInterativo.SetActive(false);
        _lockPuzzle.SetActive(false);
        _vc.SetActive(false);
     
    }
}
