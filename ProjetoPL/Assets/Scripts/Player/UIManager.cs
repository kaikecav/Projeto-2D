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

    //REMOVER IMAGEM DO INVENTÁRIO
    internal static void RemoveInventoryImage(Item usedItem)        //função para remover a imagem do inventário
    {
        //necessário criar a interação do item com o objeto na cena para formação desta parte do código
        throw new NotImplementedException();
    }
}