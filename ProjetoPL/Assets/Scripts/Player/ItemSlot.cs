using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    //ITEM DATA 
    public string pizzaName;    //variável que determina o nome da pizza
    public int quantity;    //variável que determina a quantidade da pizza
    public Sprite pizzaSprite;  //variável que determina a imagem/sprite da pizza
    public bool isFull; 
    public Sprite emptySprite;  //variável que determina a imagem/sprite caso não haja pizza

    [SerializeField]
    public int maxNumberOfItems;    //variável que determina a quantidade máxima da pizza

    //ITEM SLOT
    [SerializeField]
    public TMP_Text quantityText;   //variável que mostra a quantidade da pizza

    [SerializeField]
    public Image pizzaImage;   //variável que mostra a imagem da pizza

    //ITEM SELECIONADO A MOSTRA NA CAIXA
    public Image BoxPizzaImage;     //variável que mostra a imagem da pizza na HUD
    public TMP_Text BoxPizzaNameText;
    public TMP_Text BoxPizzaQuantityText;   //variável que mostra a quantidade da pizza na HUD


    //Variável de seleção do slot
    public GameObject selectedShader;   
    public bool thisItemSelected;

    private InventoryManager inventoryManager;      //variável de acesso ao script InventoryManager


    private void Start()
    {
        //acesso ao script InventoryManager dentro do jogo
        inventoryManager = GameObject.Find("Inventory Canva").GetComponent<InventoryManager>();
    }

    public int AddItem(string pizzaName, int quantity, Sprite pizzaSprite)
    {
        // Se o slot estiver vazio, atualiza nome e sprite
        if (this.quantity == 0)
        {
            this.pizzaName = pizzaName;
            this.pizzaSprite = pizzaSprite;
            pizzaImage.sprite = pizzaSprite;
        }

        // Soma a quantidade
        this.quantity += quantity;

        // Verifica limite máximo
        if (this.quantity >= maxNumberOfItems)
        {
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            isFull = true;
            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;
            return extraItems;
        }

        // Atualiza texto
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }


    //Função para funcionamento do click do mouse
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        //deixa todos os slots desselecionados
        inventoryManager.DeselectAllSlots();

        //Se o item está selecionado, então o panel mostra visualmente qual item está selecionado
        thisItemSelected = true;
        selectedShader.SetActive(true);

        //Atualiza a Hud com o item selecionado
        BoxPizzaNameText.text = pizzaName;
        BoxPizzaImage.sprite = pizzaSprite;
        BoxPizzaQuantityText.text = quantity.ToString();

        if (BoxPizzaImage.sprite == null)
        {
            BoxPizzaImage.sprite = emptySprite;
        }

        inventoryManager.selectedSlot = this; // informa ao inventário qual item está selecionado
    }


    public void OnRightClick()
    {

    }
}
