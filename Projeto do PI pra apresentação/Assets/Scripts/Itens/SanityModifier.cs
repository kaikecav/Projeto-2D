using UnityEngine;
using UnityEngine.Events;

public class SanityModifier : Interactable
{
    public Item item;       //permite vincular com o script "Item"
    public UnityEvent recuperaSanidade;

    public override void Interact()     //função de interação para o item coletável
    {
        if (item == null)
        {
            Debug.LogWarning($"O objeto {gameObject.name} não possui um Item definido!");
            return;
        }
        Destroy(gameObject);

        recuperaSanidade.Invoke();
    }
}
