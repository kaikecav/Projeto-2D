using System.Collections.Generic;
using UnityEngine;

public class BagInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();     //fun��o que cria uma lista de itens
    public List<ItemSlot> itemSlots = new List<ItemSlot>();     //fun��o que permite atualiazar essa lista com cada item selecionado

    public static BagInventory instance;       //torna o invent�rio uma inst�ncia que pode ser usada em outros scripts

    private void Awake()        //fun��o chamada antes do primeiro frame
    {
        //permite que o invent�rio seja salvo para ser mantido o mesmo independente da cena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Tenta pegar slots filhos primeiro
        itemSlots = new List<ItemSlot>(GetComponentsInChildren<ItemSlot>());

        // Se nada foi encontrado, tenta buscar globalmente (fallback)
        if (itemSlots.Count == 0)
        {
            ItemSlot[] found = FindObjectsOfType<ItemSlot>();
            itemSlots = new List<ItemSlot>(found);
            Debug.LogWarning($"Inventory: nenhum ItemSlot filho encontrado, usando fallback. Slots achados = {itemSlots.Count}");
        }
        else
        {
            Debug.Log($"Inventory: slots encontrados como filhos = {itemSlots.Count}");
        }
    }

    public static void SetItem(Item item)       //fun��o que adiciona o item ao invent�rio
    {
        if (instance == null) return;       //se n�o tiver nada, retorna

        instance.items.Add(item);       //adiciona o item ao invent�rio
        UiManager.SetInventoryImage(item);      //coloca a imagem desse item no UI do invent�rio
    }

    public static bool HasItem(Item item)       //fun��o para a verifica��o se o item existe no invent�rio
    {
        if (instance == null) return false;     //se n�o tiver nada, retorna falso
        return instance.items.Contains(item);       //retorna a quatidade e se cont�m o item
    }


    public ItemSlot selectedSlot;       //fun��o para slots do invent�rio selecionados

    //SELECIONA O SLOT
    public void SetSelectedSlot(ItemSlot slot)
    {
        selectedSlot = slot;
    }


    //DESSELECIONA O SLOT
    public void DeselectAllSlots()      //fun��o para desselecionar os slots
    {
        //faz a contagem dos slots
        for (int i = 0; i < itemSlots.Count; i++)
        {
            //verifica se o slot est� selecionado e desseleciona ele
            var slot = itemSlots[i];
            if (slot != null)
            {
                slot.Deselect();
            }
        }
    }
}
