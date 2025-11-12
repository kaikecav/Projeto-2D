using System;
using UnityEngine;
using UnityEngine.Events;

public class Door : Interactable
{
    [Header("Configuração da Porta")]
    [Tooltip("A porta está trancada no início?")]
    public bool isLocked = true;

    [Tooltip("ID da Chave. Deve ser o mesmo 'Item' ScriptableObject do Inventory.")]
    // O campo 'conditionalItem' que você definiu na base será a sua CHAVE.

    public UnityEvent doorOpen;

    public override void Interact()
    {
        if (isLocked)
        {
            // Pega o item ATUALMENTE SELECIONADO (que agora deve conter a chave)
            Item selectedItem = Inventory.instance.GetSelectedItem();

            // Checagem: O item selecionado é a chave correta?
            if (selectedItem != null && selectedItem == conditionalItem)
            {
                // SUCESSO - ABERTO!
                Inventory.instance.RemoveItem(conditionalItem);
                OpenDoor();
            }
            else
            {
                // FALHA: (selectedItem é nulo ou o item errado)
                Debug.Log("Porta trancada. Encontre a chave da porta!");
            }
        }
        // ... (OpenDoor continua igual)
    }

    private void OpenDoor()
    {
        isLocked = false;

        // Se a porta for um modelo/item que precisa ser "sumido":
        // gameObject.SetActive(false); 

        // Se o objetivo é que o jogador possa passar por ela:
        // Desativar o Collider de bloqueio:
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        Debug.Log("Porta destrancada e liberada!");

        Destroy(gameObject);

        doorOpen.Invoke();
    }
}
