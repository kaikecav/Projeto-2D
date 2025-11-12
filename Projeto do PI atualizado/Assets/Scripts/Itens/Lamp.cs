using UnityEngine;
using UnityEngine.Events;

public class Lamp : Interactable
{
    public Item item;                  // Permite vincular com o script "Item"
    public LampManager lampManager;    // Referência ao LampManager
    public UnityEvent recuperaSanidade; // Evento para qualquer efeito extra (opcional)

    public override void Interact()
    {
        if (item == null)
        {
            Debug.LogWarning($"O objeto {gameObject.name} não possui um Item definido!");
            return;
        }

        // Destrói o objeto coletável
        Destroy(gameObject);

        // Reativa o GameObject do LampManager caso esteja desativado
        if (lampManager != null)
        {
            lampManager.gameObject.SetActive(true);
            lampManager.Reacender(); // reinicia o ciclo de piscadas
        }

        // Invoca evento adicional, se houver
        recuperaSanidade?.Invoke();
    }
}
