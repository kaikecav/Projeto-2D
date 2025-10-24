using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler     //IPointerClickHandler permite a intera��o com objetos 2D como a UI do invent�rio
{
    [Header("UI")]
    public GameObject selectedShader;       //fun��o que verifica se o painel de destaque (SelectedPanel) est� ativo
    public bool thisItemSelected;       //fun��o que verifica se o item est� selecionado ou n�o
    public Item Item;

    [Header("Runtime")]
    public BagInventory inventory;     //pode arrastar no inspector ou ser� buscado no Start

    private void Awake()
    {
        //garante que o shader comece invis�vel
        if (selectedShader != null)
            selectedShader.SetActive(false);
    }

    private void Start()
    {
        //tenta resolver o inventory automaticamente se n�o foi definido no inspector
        if (inventory == null)
        {
            inventory = GetComponentInParent<BagInventory>();
            if (inventory == null)
            {
                inventory = FindObjectOfType<BagInventory>();
            }
        }

        Debug.Log($"ItemSlot.Start - name: {gameObject.name} | inventory: {(inventory != null ? inventory.name : "null")} | selectedShader: {(selectedShader != null ? selectedShader.name : "null")}");
    }

    //faz o invent�rio poder ser clic�vel
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"ItemSlot.OnPointerClick on {gameObject.name} button: {eventData.button}");
        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick();
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick();
    }

    //faz a verifica��o se o item est� no slot ou se � um slot vazio
    private void OnLeftClick()
    {
        if (inventory == null)
        {
            Debug.LogError($"ItemSlot {gameObject.name}: inventory est� null ao clicar. Verifique refer�ncia.");
            return;
        }

        //se o slot j� estava selecionado, desseleciona tudo
        if (thisItemSelected)
        {
            inventory.DeselectAllSlots();
            return;
        }

        //desseleciona todos os slots e seleciona este
        inventory.DeselectAllSlots();
        Select();
    }

    private void OnRightClick()
    {

    }

    public void Select()
    {
        if (selectedShader != null)
            selectedShader.SetActive(true);

        thisItemSelected = true;

        //atualiza o slot selecionado no Inventory
        if (inventory != null)
        {
            inventory.SetSelectedSlot(this);
        }
    }


    public void Deselect()
    {
        if (selectedShader != null)
            selectedShader.SetActive(false);
        thisItemSelected = false;
        // Debug.Log($"ItemSlot.Deselect -> {gameObject.name}");
    }

    internal void ClearSlot()
    {
        throw new NotImplementedException();
    }
}
