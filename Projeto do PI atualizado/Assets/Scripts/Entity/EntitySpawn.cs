using System.Collections;
using UnityEngine;

public class EntitySpawn : MonoBehaviour
{
    //VARIÁVEIS
    [Header("Pontos de spawn da entidade")]
    public Transform[] spawnPoints;

    [Header("Objeto(Sprite) da entidade")]
    public GameObject entityObject;

    [Header("Tempo para spawnar")]
    public float minTime = 30f;
    public float maxTime = 60f;

    [Header("Sanidade")]
    public SanityManager sanityManager;

    [Header("Aceleração baseada na sanidade (1 = normal, 6 = máxima)")]
    public float minTimeSpeed = 1f;
    public float maxTimeSpeed = 6f;

    [Header("Fury Mode abaixo de 10% de sanidade")]
    public bool furyModeEnabled = true;

    [Header("Painel do jumpscare")]
    public GameObject jumpscarePanel;

    // ======================================================
    // SOM DA ENTIDADE (ADICIONADO)
    // ======================================================
    [Header("Som da entidade aparecendo")]
    public AudioSource somAparece;
    // ======================================================

    private Transform lastPoint;
    public Transform currentSpawnPoint { get; private set; }


    void Start()
    {
        StartCoroutine(MainRoutine());
    }

    private IEnumerator MainRoutine()
    {
        while (true)
        {
            float sanityPercent = GetSanityPercent();

            if (sanityPercent >= 0.90f && furyModeEnabled)
            {
                yield return FuryMode();
                continue;
            }

            float espera = Random.Range(minTime, maxTime);
            yield return DynamicWait(espera, "[Espera]");

            TeleportEntity(); // <--- AQUI A ENTIDADE SURGE

            float activeMin = 10f;
            float activeMax = 30f;

            float baseActive = Mathf.Lerp(activeMin, activeMax, sanityPercent);
            float activeTime = Random.Range(baseActive * 0.8f, baseActive * 1.2f);

            entityObject.SetActive(true);

            yield return DynamicWait(activeTime, "[Teleportado]");

            entityObject.SetActive(false);
        }
    }

    private IEnumerator FuryMode()
    {
        Debug.Log("[Fúria] Ativado");

        entityObject.SetActive(true);

        while (GetSanityPercent() >= 0.90f)
        {
            TeleportEntity();

            Debug.Log("[Fúria] Teleportando novamente em 5s");
            yield return new WaitForSeconds(5f);
        }

        Debug.Log("[Fúria] Desativado — voltando ao comportamento normal.");
    }

    private IEnumerator DynamicWait(float targetTime, string debugLabel)
    {
        float elapsed = 0f;

        while (elapsed < targetTime)
        {
            yield return null;

            float sanityPercent = GetSanityPercent();
            float speed = Mathf.Lerp(minTimeSpeed, maxTimeSpeed, sanityPercent);

            float acceleratedDelta = Time.deltaTime * speed;
            elapsed += acceleratedDelta;
        }

        Debug.Log($"{debugLabel} Finalizado.");
    }

    private void TeleportEntity()
    {
        Transform chosen = ChooseRandomPoint();
        currentSpawnPoint = chosen;

        entityObject.transform.SetPositionAndRotation(
            chosen.position,
            chosen.rotation
        );

        entityObject.transform.localScale = chosen.localScale;

        Debug.Log($"Teleportado para: {chosen.name}");

        // ======================================================
        // SOM DA ENTIDADE APARECENDO (ADICIONADO)
        // ======================================================
        if (somAparece != null)
        {
            somAparece.Play();
        }
        // ======================================================
    }

    private Transform ChooseRandomPoint()
    {
        if (spawnPoints.Length == 1)
            return spawnPoints[0];

        Transform c;

        do c = spawnPoints[Random.Range(0, spawnPoints.Length)];
        while (c == lastPoint);

        lastPoint = c;
        return c;
    }

    public bool IsEntityAt(Transform puzzlePoint)
    {
        if (puzzlePoint == null)
        {
            Debug.LogWarning("PuzzlePoint está NULO!");
            return false;
        }

        if (currentSpawnPoint == null)
        {
            Debug.Log("CurrentSpawnPoint ainda não definido.");
            return false;
        }

        return currentSpawnPoint == puzzlePoint;
    }

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

    private float GetSanityPercent()
    {
        float s = sanityManager.sanitySlider.value;
        float m = sanityManager.sanitySlider.maxValue;
        return 1f - (s / m);
    }
}
