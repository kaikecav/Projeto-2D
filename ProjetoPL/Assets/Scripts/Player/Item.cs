using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string pizzaName;   //vari�vel que determina o nome da pizza

    [SerializeField]
    private int quantity;   //vari�vel que determina a quantidade da pizza

    [SerializeField]
    private Sprite pizzaSprite; //vari�vel que determina a imagem/sprite da pizza

    //Vari�vel que permite usar o script InventoryManager neste script
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
        //Adiciona a fun��o do player coletar a pizza
        if (collision.CompareTag("Player"))
        {
            //Adiciona as vari�veis da pizza no invent�rio
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
