using UnityEngine;

public class PizzaShooter : MonoBehaviour
{
    [Header("Prefabs de Pizza")]
    public GameObject pizzaQueijoPrefab;
    public GameObject pizzaCalabresaPrefab;
    public GameObject pizzaFrangoPrefab;
    public GameObject pizzaMargueritaPrefab;

    [Header("Configurações de Tiro")]
    public Transform shootPoint;
    public float shootForce = 10f;
    private bool facingRight = true;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("Inventory Canva").GetComponent<InventoryManager>();
    }

    void Update()
    {
        HandleFlip();

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void HandleFlip()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void Shoot()
    {
        if (inventoryManager.selectedSlot == null)
        {
            Debug.Log("Nenhuma pizza selecionada!");
            return;
        }

        ItemSlot slot = inventoryManager.selectedSlot;

        // Checa se tem pizza suficiente
        if (slot.quantity <= 0)
        {
            Debug.Log("Não há pizza suficiente neste slot!");
            return;
        }

        string pizzaName = slot.pizzaName;
        GameObject prefab = GetPizzaPrefab(pizzaName);

        if (prefab == null)
        {
            Debug.LogWarning("Prefab da pizza não encontrado para: " + pizzaName);
            return;
        }

        // Instancia projétil
        GameObject pizzaObj = Instantiate(prefab, shootPoint.position, Quaternion.identity);

        // Adiciona Rigidbody2D se não tiver
        Rigidbody2D rb = pizzaObj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = pizzaObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        float direction = facingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * shootForce, 0f);

        // Define o tipo da pizza no projétil
        PizzaProjectile proj = pizzaObj.GetComponent<PizzaProjectile>();
        if (proj == null)
        {
            proj = pizzaObj.AddComponent<PizzaProjectile>();
        }
        proj.pizzaTipo = pizzaName;
        proj.lifetime = 5f; // opcional: tempo de vida do projétil

        // ⚡ Consome 1 unidade da pizza do inventário
        slot.quantity--;
        slot.quantityText.text = slot.quantity.ToString();
        slot.BoxPizzaQuantityText.text = slot.quantity.ToString();

        // Se zerar, limpa o slot
        if (slot.quantity <= 0)
        {
            slot.pizzaName = "";
            slot.pizzaSprite = null;
            slot.pizzaImage.sprite = slot.emptySprite;
            slot.BoxPizzaImage.sprite = slot.emptySprite;

            slot.isFull = false;
            inventoryManager.DeselectAllSlots();
        }
    }
    GameObject GetPizzaPrefab(string pizzaName)
    {
        string name = pizzaName.ToLower().Replace(" ", "");

        switch (name)
        {
            case "pizzaqueijo": return pizzaQueijoPrefab;
            case "pizzacalabresa": return pizzaCalabresaPrefab;
            case "pizzafrango": return pizzaFrangoPrefab;
            case "pizzamarguerita": return pizzaMargueritaPrefab;
            default: return null;
        }
    }
}
