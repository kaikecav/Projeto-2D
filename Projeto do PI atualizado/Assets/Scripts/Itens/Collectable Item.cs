using UnityEngine;

public class CollectableItem : Interactable     //Determina que o script pertence ao tipo interativo
{
    public Item item;       //permite vincular com o script "Item"

    public override void Interact()     //função de interação para o item coletável
    {
        if (item == null)
        {
            Debug.LogWarning($"O objeto {gameObject.name} não possui um Item definido!");
            return;
        }

        // Adiciona o item ao inventário
        Inventory.SetItem(item);
        Debug.Log($"Coletou {item.name}");

        // Encontra todos os objetos do tipo, inclusive desativados
        CollectableItem[] allItems = Resources.FindObjectsOfTypeAll<CollectableItem>();

        //Indica o que ocorre com cada item coletado
        foreach (CollectableItem obj in allItems)
        {
            //determina que se o objeto é defierente de null e é igual a item
            if (obj != null && obj.item == item)
            {
                if (obj.gameObject.scene.IsValid()) // ignora prefabs fora da cena
                {
                    Destroy(obj.gameObject);        //destrói o objeto e seus clones dentro da cena
                }
            }
        }
    }
}
