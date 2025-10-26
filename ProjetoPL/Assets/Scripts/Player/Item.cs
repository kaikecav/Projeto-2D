using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int leftOverItems = inventoryManager.AddItem(pizzaName, quantity, pizzaSprite);
            if (leftOverItems <= 0)
            {
                GetComponent<ItemRespawn>().StartRespawn(); // usa o script de respawn
            }
            else
            {
                quantity = leftOverItems;
            }
        }
    }

    IEnumerator Retorno(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        //Ativa o collider e o objeto do ITEM
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(true);
    }
}
