using System.Collections;
using UnityEngine;

public class WindowSpawn : MonoBehaviour
{
    [Header("Referências")]
    public Transform spawnPoint;                // ÚNICO ponto de spawn
    public GameObject entityObject;             // Objeto da entidade
    public SanityManager sanityManager;         // Para dano de sanidade

    [Header("Verificação externa")]
    [SerializeField] private GameObject _vc;    // Se estiver ativo → chance de spawn ativa

    [Header("Chance de spawn (0.5 = 50%)")]
    [Range(0f, 1f)]
    public float spawnChance = 0.5f;

    [Header("Chance de jumpscare (0.9 = 90%)")]
    [Range(0f, 1f)]
    public float jumpscareChance = 0.9f;

    [Header("Jumpscare")]
    public GameObject jumpscarePanel;

    public bool IsSpawned { get; private set; } = false;

    void Start()
    {
        entityObject.SetActive(false);
    }

    //FUNÇÃO CHAMADA QUANDO O PUZZLE É ABERTO
    public void TrySpawnEntity()
    {
        // Só tenta spawnar se o VC estiver ativo
        if (_vc != null && _vc.activeSelf)
        {
            float roll = Random.value;
            Debug.Log("[WindowSpawn] Sorteio de spawn: " + roll);

            if (roll <= spawnChance)
            {
                SpawnNow();
            }
            else
            {
                Debug.Log("[WindowSpawn] Entidade decidiu NÃO aparecer.");
                IsSpawned = false;
            }
        }
        else
        {
            // VC não ativo → nunca spawn
            IsSpawned = false;
        }
    }

    //SPAWN
    private void SpawnNow()
    {
        entityObject.transform.SetPositionAndRotation(
            spawnPoint.position,
            spawnPoint.rotation
        );

        entityObject.transform.localScale = spawnPoint.localScale;
        entityObject.SetActive(true);

        IsSpawned = true;

        Debug.Log("[WindowSpawn] ENTIDADE spawnada na janela");
    }

    //FUNÇÃO CHAMADA QUANDO O PUZZLE É ANALISADO
    public IEnumerator TryJumpscare()
    {
        // Só rola jumpscare se ela realmente apareceu
        if (!IsSpawned)
        {
            Debug.Log("[WindowSpawn] Sem jumpscare porque a entidade não spawnou.");
            yield break;
        }

        float roll = Random.value;
        Debug.Log("[WindowSpawn] Sorteio Jumpscare: " + roll);

        if (roll <= jumpscareChance)
        {
            Debug.Log("[WindowSpawn] JUMPSCARE ativado!");
            yield return TriggerJumpscare();
        }
        else
        {
            Debug.Log("[WindowSpawn] Jumpscare falhou.");
        }
    }

    //JUMPSCARE
    public IEnumerator TriggerJumpscare()
    {
        if (jumpscarePanel != null)
            jumpscarePanel.SetActive(true);

        if (sanityManager != null)
            sanityManager.AffectSanity(-2000);

        yield return new WaitForSecondsRealtime(1f);

        if (jumpscarePanel != null)
            jumpscarePanel.SetActive(false);
    }

    //RESET QUANDO VOCÊ SAI DO PUZZLE
    public void Despawn()
    {
        entityObject.SetActive(false);
        IsSpawned = false;
    }
}
