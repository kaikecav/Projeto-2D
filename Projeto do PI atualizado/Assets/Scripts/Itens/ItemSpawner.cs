using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject remedioPrefab;
    public GameObject lampadaPrefab;

    [Header("Pontos de Spawn")]
    public List<Transform> spawnPoints;

    [Header("Referência da Sala Giratória")]
    public Transform salaGiratoria;  // <<< arraste aqui a sala

    void Start()
    {
        SpawnItens(remedioPrefab, 2);
        SpawnItens(lampadaPrefab, 2);
    }

    void SpawnItens(GameObject prefab, int quantidade)
    {
        List<int> pontosDisponiveis = new List<int>();

        for (int i = 0; i < spawnPoints.Count; i++)
            pontosDisponiveis.Add(i);

        for (int i = 0; i < quantidade; i++)
        {
            if (pontosDisponiveis.Count == 0)
            {
                Debug.LogWarning("Não há pontos de spawn suficientes!");
                return;
            }

            int index = Random.Range(0, pontosDisponiveis.Count);
            int spawnIndex = pontosDisponiveis[index];

            Transform ponto = spawnPoints[spawnIndex];

            GameObject item = Instantiate(prefab, ponto.position, ponto.rotation);

            // 🔥 AQUI ESTÁ A LINHA QUE RESOLVE O PROBLEMA
            item.transform.SetParent(salaGiratoria);

            pontosDisponiveis.RemoveAt(index);
        }
    }
}
