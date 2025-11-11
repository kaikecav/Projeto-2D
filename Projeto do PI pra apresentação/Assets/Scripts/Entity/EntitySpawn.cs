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
    public float minTime = 60f;
    public float maxTime = 120f;

    [Header("Sanidade")]
    public SanityManager sanityManager;

    [Header("Aceleração baseada na sanidade (1 = normal, 6 = máxima)")]
    public float minTimeSpeed = 1f;
    public float maxTimeSpeed = 6f;

    [Header("Fury Mode abaixo de 10% de sanidade")]
    public bool furyModeEnabled = true;

    [Header("Painel do jumpscare")]
    public GameObject jumpscarePanel;

    private Transform lastPoint;
    public Transform currentSpawnPoint { get; private set; }    //Verifica em qual spawn a entidade está

    void Start()
    {
        //Inicia todo o sistema
        StartCoroutine(MainRoutine());
    }

    //FUNÇÃO PRINCIPAL
    private IEnumerator MainRoutine()
    {
        //Enquanto essa função for verdadeira
        while (true)
        {
            float sanityPercent = GetSanityPercent();   //Indica a porcentagem da sanidade através do GetSanityPercent

            //Mantém o Modo Fúria se a sanidade estiver menor ou igual a 10%
            if (sanityPercent >= 0.90f && furyModeEnabled)
            {
                yield return FuryMode();
                continue;
            }

            //Decisão de quanto tempo para spawnar
            float espera = Random.Range(minTime, maxTime);
            yield return DynamicWait(espera, "[Espera]");

            TeleportEntity();

            //Mantém ela ativa na cena entre 10 segundos a 30 segundos
            float activeMin = 10f;
            float activeMax = 30f;

            //Faz a randomização do tempo baseado na sanidade
            float baseActive = Mathf.Lerp(activeMin, activeMax, sanityPercent);
            float activeTime = Random.Range(baseActive * 0.8f, baseActive * 1.2f);


            entityObject.SetActive(true);

            yield return DynamicWait(activeTime, "[Teleportado]");

            entityObject.SetActive(false);
        }
    }

    //MODO FÚRIA
    private IEnumerator FuryMode()
    {
        Debug.Log("[Fúria] Ativado");

        entityObject.SetActive(true);

        //Enquanto a sanidade for menor ou igual a 10%, ativa o Modo Fúria
        while (GetSanityPercent() >= 0.90f)
        {
            //Teleporta a entidade a cada 5 segundos
            TeleportEntity();
            Debug.Log("[Fúria] Teleportando novamente em 5s");

            yield return new WaitForSeconds(5f);
        }

        Debug.Log("[Fúria] Desativado — voltando ao comportamento normal.");
    }

    //FUNÇÃO PARA A ESPERA DINÂMICA
    private IEnumerator DynamicWait(float targetTime, string debugLabel)
    {
        float elapsed = 0f;

        //Determina o novo tempo de espera
        while (elapsed < targetTime)
        {
            yield return null;

            //Usa a sanidade para determinar o novo tempo
            float sanityPercent = GetSanityPercent();
            //Aumenta ou diminui a velocidade de acordo com a sanidade
            float speed = Mathf.Lerp(minTimeSpeed, maxTimeSpeed, sanityPercent);

            float acceleratedDelta = Time.deltaTime * speed;
            elapsed += acceleratedDelta;
        }

        Debug.Log($"{debugLabel} Finalizado.");
    }

    //FUNÇÃO PARA TELEPORTAR A ENTIDADE
    private void TeleportEntity()
    {
        //Escolhe um spawn aleatório
        Transform chosen = ChooseRandomPoint();
        currentSpawnPoint = chosen;

        //Ativa a função seguindo a posição e a rotação
        entityObject.transform.SetPositionAndRotation(
            chosen.position,
            chosen.rotation
        );

        entityObject.transform.localScale = chosen.localScale;

        Debug.Log($"Teleportado para: {chosen.name}");
    }

    //FUNÇÃO PARA ESCOLHER OS SPAWNS
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

    //FUNÇÃO PARA DETECTAR SE A ENTIDADE ESTÁ NO PUZZLE
    public bool IsEntityAt(Transform puzzlePoint)
    {

        //Se o puzzlePoint não existir, alerta e não executa nada
        if (puzzlePoint == null)
        {
            Debug.LogWarning("PuzzlePoint está NULO!");
            return false;
        }

        //Se o currentSpawnPoint não existir, alerta e não executa nada
        if (currentSpawnPoint == null)
        {
            Debug.Log("CurrentSpawnPoint ainda não definido.");
            return false;
        }

        //Se tiver o currentSpawnPoint, determine para ele ser igual ao puzzlePoint
        return currentSpawnPoint == puzzlePoint;
    }

    //FUNÇÃO PARA O SISTEMA DE JUMPSCARE
    public IEnumerator TriggerJumpscare()
    {

        //Se o panel estiver desativado, ativa
        if (jumpscarePanel != null)
            jumpscarePanel.SetActive(true);

        //Retira 2000 da sanidade
        if (sanityManager != null)
            sanityManager.AffectSanity(-2000);

        //Mantém o Jumpscare por 1 segundo
        yield return new WaitForSecondsRealtime(1f);

        //Se o panel estiver ativado, desativa
        if (jumpscarePanel != null)
            jumpscarePanel.SetActive(false);
    }

    //FUNÇÃO PARA O VINCULO COM A SANIDADE
    private float GetSanityPercent()
    {
        float s = sanityManager.sanitySlider.value;
        float m = sanityManager.sanitySlider.maxValue;
        return 1f - (s / m);
    }
}
