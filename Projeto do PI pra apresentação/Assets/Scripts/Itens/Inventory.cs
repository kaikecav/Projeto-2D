using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();     //função que cria uma lista de itens
    public List<ItemSlot> itemSlots = new List<ItemSlot>();     //função que permite atualiazar essa lista com cada item selecionado

    public static Inventory instance;       //torna o inventário uma instância que pode ser usada em outros scripts

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

    // Inventory.cs

    public static void SetItem(Item item)
    {
        if (instance == null) return;

        // 1. ADICIONA O ITEM NA LISTA PRINCIPAL
        instance.items.Add(item);

        // 2. ENCONTRA O PRIMEIRO SLOT VAZIO E ATUALIZA ELE
        for (int i = 0; i < instance.itemSlots.Count; i++)
        {
            ItemSlot slot = instance.itemSlots[i];

            // **CHECA SE O SLOT ESTÁ VAZIO (Item == null)**
            if (slot.Item == null)
            {
                // O ItemSlot agora armazena a referência do ScriptableObject
                slot.Item = item;

                // Coloca a imagem desse item no UI do inventário (Sua função original)
                UiManager.SetInventoryImage(item);

                Debug.Log($"Item adicionado ao Slot {i}: {item.itemName}");
                return; // Item adicionado, pare de procurar
            }
        }

        Debug.LogWarning("Inventário cheio! Item não adicionado ao slot.");
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


    //Checa seus campos e métodos existentes, incluindo SetSelectedSlot)

    // Retorna o Item que está no slot selecionado atualmente
    public Item GetSelectedItem()
    {
        // 1. Checa se há um slot selecionado
        if (selectedSlot != null)
        {
            // 2. Checa se o slot tem um item
            return selectedSlot.Item;
        }

        // Se não houver slot ou item, retorna nulo
        return null;
    }

    // Inventory.cs (Adicione no final da classe, antes da última '}')

    // NOVO MÉTODO: Função que remove um Item do inventário

    // Inventory.cs

    public void RemoveItem(Item itemToRemove)
    {
        if (itemToRemove == null) return;

        // 1. Remove da lista principal de itens (List<Item> items)
        if (instance.items.Contains(itemToRemove))
        {
            instance.items.Remove(itemToRemove);

            // 2. Procura o slot que contém este item para limpá-lo visualmente e referencialmente
            for (int i = 0; i < instance.itemSlots.Count; i++)
            {
                ItemSlot slot = instance.itemSlots[i];

                // Checa se este slot armazena a chave que está sendo removida
                if (slot != null && slot.Item == itemToRemove)
                {
                    // Chama a função de limpeza do slot (Etapa 2)
                    slot.ClearSlot();

                    // Garante que o slot selecionado no Inventory não aponte mais para o slot limpo
                    if (instance.selectedSlot == slot)
                    {
                        instance.selectedSlot = null;
                    }

                    // Item e slot limpos, pode sair do loop
                    return;
                }
            }
        }
    }
}