using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string pizzaName;   //variável que determina o nome da pizza

    [SerializeField]
    private int quantity;   //variável que determina a quantidade da pizza

    [SerializeField]
    private Sprite pizzaSprite; //variável que determina a imagem/sprite da pizza

    //Variável que permite usar o script InventoryManager neste script
    private InventoryManager inventoryManager;

    void Start()
    {
        //Encontra o script InventoryManager dentro do jogo
        inventoryManager = GameObject.Find("Inventory Canva").GetComponent<InventoryManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Adiciona a função do player coletar a pizza
        if (collision.CompareTag("Player"))
        {
            //Adiciona as variáveis da pizza no inventário
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

    //auxilia o script ItemRespawn, garantindo que a pizza ira aparecer de novo
    IEnumerator Retorno(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        //Ativa o collider e o objeto do ITEM
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(true);
    }
}
