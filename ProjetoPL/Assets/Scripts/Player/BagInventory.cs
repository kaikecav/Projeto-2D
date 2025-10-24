using System.Collections.Generic;
using UnityEngine;

public class BagInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();     //função que cria uma lista de itens
    public List<ItemSlot> itemSlots = new List<ItemSlot>();     //função que permite atualiazar essa lista com cada item selecionado

    public static BagInventory instance;       //torna o inventário uma instância que pode ser usada em outros scripts

    private void Awake()        //função chamada antes do primeiro frame
    {
        //permite que o inventário seja salvo para ser mantido o mesmo independente da cena
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

    public static void SetItem(Item item)       //função que adiciona o item ao inventário
    {
        if (instance == null) return;       //se não tiver nada, retorna

        instance.items.Add(item);       //adiciona o item ao inventário
        UiManager.SetInventoryImage(item);      //coloca a imagem desse item no UI do inventário
    }

    public static bool HasItem(Item item)       //função para a verificação se o item existe no inventário
    {
        if (instance == null) return false;     //se não tiver nada, retorna falso
        return instance.items.Contains(item);       //retorna a quatidade e se contém o item
    }


    public ItemSlot selectedSlot;       //função para slots do inventário selecionados

    //SELECIONA O SLOT
    public void SetSelectedSlot(ItemSlot slot)
    {
        selectedSlot = slot;
    }


    //DESSELECIONA O SLOT
    public void DeselectAllSlots()      //função para desselecionar os slots
    {
        //faz a contagem dos slots
        for (int i = 0; i < itemSlots.Count; i++)
        {
            //verifica se o slot está selecionado e desseleciona ele
            var slot = itemSlots[i];
            if (slot != null)
            {
                slot.Deselect();
            }
        }
    }
}
