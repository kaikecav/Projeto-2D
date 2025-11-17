using UnityEngine;

public class CollectableONEItem : Interactable
{
    public Item item;

    public override void Interact()
    {
        if (item == null)
        {
            Debug.LogWarning($"O objeto {gameObject.name} não possui um Item definido!");
            return;
        }

        //Adiciona o item ao inventário
        Inventory.SetItem(item);
        Debug.Log($"Coletou {item.name}");

        //Destrói apenas ESTE objeto
        Destroy(gameObject);
    }
}
