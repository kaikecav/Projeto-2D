using UnityEngine;
using UnityEngine.Events;

//PRECISA MONTAR O CÓDIGO DA MANEIRA CORRETA

public class UsableObject : Interactable
{
    [Header("Item Required")]
    public Item requiredItem; // qual item precisa para interagir
    public bool consumeItem = true; // se true, remove do inventário ao usar

    [Header("Events")]
    public UnityEvent onUseItem; // eventos visuais ou lógicos

    public override void Interact()
    {
        if (InventoryHasSelectedItem())
        {
            Debug.Log($"Usando {Inventory.instance.selectedSlot.Item.itemName} em {gameObject.name}");

            // dispara evento
            onUseItem.Invoke();

            // remove do inventário
            if (consumeItem)
            {
                InventoryUseSelectedItem();
            }

            //Para a seleção
            Inventory.instance.DeselectAllSlots();
        }
        else
        {
            Debug.Log($"Não tem o item correto selecionado para usar em {gameObject.name}");
        }
    }

    private bool InventoryHasSelectedItem()
    {
        if (Inventory.instance.selectedSlot == null || Inventory.instance.selectedSlot.Item == null)
            return false;

        Item selectedItem = Inventory.instance.selectedSlot.Item;
        Debug.Log($"Selecionado: {selectedItem.itemID}, Requerido: {requiredItem.itemID}");

        return selectedItem.itemID == requiredItem.itemID;
    }



    private void InventoryUseSelectedItem()
    {
        Item usedItem = Inventory.instance.selectedSlot.Item;
        Inventory.instance.selectedSlot.Deselect();
        Inventory.instance.selectedSlot = null;

        Inventory.instance.items.Remove(usedItem);
        UiManager.RemoveInventoryImage(usedItem);
    }
}
