using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager instance;       //permite outros scripts acessarem este script através de instância

    public Image[] iventoryImages;      //função para as imagnes no inventário

    public GameObject interactionPanel;     //função para os panels que indicam o slot selecionado


    private void Awake()        //função que ocorre antes do primeiro frame
    {

        //se não houver nenhuma instância, então será essa
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }





    //DETERMINA A IMAGEM NO INVENTÁRIO
    public static void SetInventoryImage(Item item)     //função para colocar a imagem do item
    {

        //se não tiver nada, não faz nada
        if (instance == null)
        {
            return;
        }

        //determina a imagem do item em cada slot, pulando para o próximo caso o slot já possua uma imagem de item
        for (int i = 0; i < instance.iventoryImages.Length; i++)
        {
            //coloca a imagem do item no slot indicado
            if (!instance.iventoryImages[i].gameObject.activeInHierarchy)
            {
                instance.iventoryImages[i].sprite = item.itemImage;
                instance.iventoryImages[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    // Remove a imagem do inventário (Limpa o slot visualmente)
    internal static void RemoveInventoryImage(Item usedItem)
    {
        if (instance == null || usedItem == null)
        {
            return;
        }

        // Procura na lista de imagens qual tem o sprite correspondente ao item usado
        for (int i = 0; i < instance.iventoryImages.Length; i++)
        {
            Image img = instance.iventoryImages[i];

            // Verifica se a imagem está ativa e se o sprite corresponde ao item usado
            if (img.gameObject.activeInHierarchy && img.sprite == usedItem.itemImage)
            {
                // LIMPEZA VISUAL:
                img.sprite = null; // Zera o sprite
                img.gameObject.SetActive(false); // Desativa o objeto (igual ao que SetInventoryImage checa)

                return; // Item visualmente removido
            }
        }
    }
}