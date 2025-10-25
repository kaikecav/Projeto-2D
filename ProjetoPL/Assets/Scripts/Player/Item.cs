using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string pizzaName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite pizzaSprite;

    private InventoryManager inventoryManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = GameObject.Find("Inventory Canva").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inventoryManager.AddItem(pizzaName, quantity, pizzaSprite);
            Destroy(gameObject);
        }
    }
}
