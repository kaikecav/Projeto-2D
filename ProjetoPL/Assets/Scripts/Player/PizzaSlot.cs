using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler     //IPointerClickHandler permite a interação com objetos 2D como a UI do inventário
{
    [Header("UI")]
    public GameObject selectedShader;       //função que verifica se o painel de destaque (SelectedPanel) está ativo
    public bool thisItemSelected;       //função que verifica se o item está selecionado ou não
    public Item Item;

    [Header("Runtime")]
    public BagInventory inventory;     //pode arrastar no inspector ou será buscado no Start

    private void Awake()
    {
        //garante que o shader comece invisível
        if (selectedShader != null)
            selectedShader.SetActive(false);
    }

    private void Start()
    {
        //tenta resolver o inventory automaticamente se não foi definido no inspector
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

    //faz o inventário poder ser clicável
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"ItemSlot.OnPointerClick on {gameObject.name} button: {eventData.button}");
        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick();
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick();
    }

    //faz a verificação se o item está no slot ou se é um slot vazio
    private void OnLeftClick()
    {
        if (inventory == null)
        {
            Debug.LogError($"ItemSlot {gameObject.name}: inventory está null ao clicar. Verifique referência.");
            return;
        }

        //se o slot já estava selecionado, desseleciona tudo
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
