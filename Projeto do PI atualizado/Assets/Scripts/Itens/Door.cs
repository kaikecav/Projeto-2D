using System;
using UnityEngine;
using UnityEngine.Events;

public class Door : Interactable
{
    [Header("Configuração da Porta")]
    [Tooltip("A porta está trancada no início?")]
    public bool isLocked = true;

    [Header("Áudio da Porta")]
    public AudioSource audioSource;  // o som da porta abrindo

    public UnityEvent doorOpen;

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
                Debug.Log("Porta trancada. Encontre a chave da porta!");
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

        // TOCA O SOM
        if (audioSource != null)
            audioSource.Play();

        Debug.Log("Porta destrancada e liberada!");

        // LIBERA PASSAGEM (desativa o collider)
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
            doorCollider.enabled = false;

        // TORNA A PORTA INVISÍVEL (SEM DESTRUIR)
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
            mesh.enabled = false;

        // Caso tenha animação ou eventos extras
        doorOpen.Invoke();
    }
}
