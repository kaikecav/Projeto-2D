using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Box : Interactable
{
    [Header("Configuração da Porta")]
    public GameObject key;
    [Tooltip("A porta está trancada no início?")]
    public bool isLocked = true;



    public UnityEvent doorOpen;

    private void Start()
    {
        key.SetActive(false);
    }

    public override void Interact()
    {
        if (isLocked)
        {
            Item selectedItem = Inventory.instance.GetSelectedItem();

            if (selectedItem != null && selectedItem == conditionalItem)
            {
                Inventory.instance.RemoveItem(conditionalItem);
                OpenDoor();
            }
            else
            {
                Debug.Log("Caixa trancada. Encontre a chave da caixa!");
            }
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isLocked = false;

        Debug.Log("Porta destrancada e liberada!");

        key.SetActive(true);

        // Caso tenha animação ou eventos extras
        doorOpen.Invoke();
    }
}

