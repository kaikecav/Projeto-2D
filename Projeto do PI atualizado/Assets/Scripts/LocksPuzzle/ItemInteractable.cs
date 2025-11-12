using UnityEngine;
using UnityEngine.Events;

public class ItemInteractable : MonoBehaviour
{
    [Tooltip("Se for verdadeiro, o item pode ser interagido múltiplas vezes.")]
    [SerializeField] private bool canInteractAgain = true;

    private bool wasInteracted = false;

    [Tooltip("Lista de funções a serem disparadas quando este item for interagido.")]
    [SerializeField] private UnityEvent OnInteract = new UnityEvent();

    public void Interact()
    {
        if (!canInteractAgain && wasInteracted)
        {
            return;
        }
        OnInteract.Invoke();

        if (!canInteractAgain)
        {
            wasInteracted = true;
        }

        Debug.Log($"Item '{gameObject.name}' interagido. Ações disparadas.");
    }

    private void OnMouseDown()
    {
        Interact();
    }
}
