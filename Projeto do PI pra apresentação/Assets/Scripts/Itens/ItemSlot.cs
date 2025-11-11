using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler     //IPointerClickHandler permite a interação com objetos 2D como a UI do inventário
{
    [Header("UI")]
    public GameObject selectedShader;       //função que verifica se o painel de destaque (SelectedPanel) está ativo
    public bool thisItemSelected;       //função que verifica se o item está selecionado ou não
    public Item Item;
    public GameObject descriptionPanel;

    [Header("Runtime")]
    public Inventory inventory;     //pode arrastar no inspector ou será buscado no Start

    //ItemDescription
    public TMP_Text ItemDescription;

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
            inventory = GetComponentInParent<Inventory>();
            if (inventory == null)
            {
                inventory = FindObjectOfType<Inventory>();
            }
        }

        Debug.Log($"ItemSlot.Start - name: {gameObject.name} | inventory: {(inventory != null ? inventory.name : "null")} | selectedShader: {(selectedShader != null ? selectedShader.name : "null")}");
        descriptionPanel.SetActive(false);
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
    // ItemSlot.cs

    private void OnLeftClick()
    {
        if (inventory == null)
        {
            Debug.LogError($"ItemSlot {gameObject.name}: inventory está null ao clicar. Verifique referência.");
            return;
        }

        // Se o slot está vazio, deseleciona tudo e sai.
        if (Item == null)
        {
            inventory.DeselectAllSlots();
            // Se já estava selecionado, ele deseleciona e termina.
            // Se estava vazio e deselecionado, não faz nada.
            return;
        }
        // -----------------------------------------------------------------------

        // Se o slot JÁ estava selecionado, DESSELECIONA tudo (limpar seleção ativa)
        if (thisItemSelected)
        {
            inventory.DeselectAllSlots();
            return;
        }

        // Se o slot CONTÉM um item e não estava selecionado, SELECIONA ele.
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
        descriptionPanel.SetActive(true);

        thisItemSelected = true;


        ItemDescription.text = Item.description;

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
        descriptionPanel.SetActive(false);
    }


    // ItemSlot.cs

    internal void ClearSlot()
    {
        // CHAMA A REMOÇÃO VISUAL NO UI MANAGER (SE O ITEM EXISTE)
        if (Item != null)
        {
            UiManager.RemoveInventoryImage(Item);
        }

        // Limpa a referência interna (ESSENCIAL!)
        Item = null;

        // Garante que o shader de seleção desapareça
        Deselect();
    }
}