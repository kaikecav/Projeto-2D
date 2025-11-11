using UnityEngine;

public class DestruicaoMaleta : MonoBehaviour
{
    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _maletaInterativo;
    [SerializeField] private GameObject _maletaPuzzle;
    [SerializeField] private GameObject _maletaAberta;
    [SerializeField] private GameObject _vc;

    public void SelfDestroy()
    {
        GameManager.Instance.UnPauseGame();
        if (_maletaInterativo != null)
        {
            Destroy(_maletaInterativo);
        }
        if (_maletaPuzzle != null)
        {
            Destroy(_maletaPuzzle);
        }

        // 3. ATIVA a versão aberta da maleta (ou o resultado do puzzle)
        _maletaAberta.SetActive(true);

        // 4. DESTRÓI o objeto de controle da visão (câmera ou controle do puzzle)
        if (_vc != null)
        {
            Destroy(_vc);
        }
    }
}
