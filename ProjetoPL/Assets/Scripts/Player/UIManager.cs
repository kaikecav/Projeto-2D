using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager instance;       //permite outros scripts acessarem este script atrav�s de inst�ncia

    public Image[] iventoryImages;      //fun��o para as imagnes no invent�rio

    public GameObject interactionPanel;     //fun��o para os panels que indicam o slot selecionado


    private void Awake()        //fun��o que ocorre antes do primeiro frame
    {

        //se n�o houver nenhuma inst�ncia, ent�o ser� essa
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }





    //DETERMINA A IMAGEM NO INVENT�RIO
    public static void SetInventoryImage(Item item)     //fun��o para colocar a imagem do item
    {

        //se n�o tiver nada, n�o faz nada
        if (instance == null)
        {
            return;
        }

        //determina a imagem do item em cada slot, pulando para o pr�ximo caso o slot j� possua uma imagem de item
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

    //REMOVER IMAGEM DO INVENT�RIO
    internal static void RemoveInventoryImage(Item usedItem)        //fun��o para remover a imagem do invent�rio
    {
        //necess�rio criar a intera��o do item com o objeto na cena para forma��o desta parte do c�digo
        throw new NotImplementedException();
    }
}