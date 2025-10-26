using System.Collections;
using UnityEngine;

public class ItemRespawn : MonoBehaviour
{
    [SerializeField] private float respawnTime = 30f;
    private Collider2D itemCollider;
    private SpriteRenderer itemRenderer;

    private void Awake()
    {
        // Pega o Collider2D do próprio objeto
        itemCollider = GetComponent<Collider2D>();

        // Procura o SpriteRenderer neste objeto ou em um filho
        itemRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void StartRespawn()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        // Desativa apenas o visual e o collider
        if (itemRenderer != null)
            itemRenderer.enabled = false;

        if (itemCollider != null)
            itemCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // Reativa
        if (itemRenderer != null)
            itemRenderer.enabled = true;

        if (itemCollider != null)
            itemCollider.enabled = true;
    }
}
