using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    //ITEM DATA 
    public string pizzaName;
    public int quantity;
    public Sprite pizzaSprite;
    public bool isFull;
    public Sprite emptySprite;

    [SerializeField]
    private int maxNumberOfItems;

    //ITEM SLOT
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image pizzaImage;

    //ITEM SELECIONADO A MOSTRA NA CAIXA
    public Image BoxPizzaImage;
    public TMP_Text BoxPizzaNameText;
    public TMP_Text BoxPizzaQuantityText;



    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;


    private void Start()
    {
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
        inventoryManager.DeselectAllSlots();

        thisItemSelected = true;
        selectedShader.SetActive(true);

        BoxPizzaNameText.text = pizzaName;
        BoxPizzaImage.sprite = pizzaSprite;
        BoxPizzaQuantityText.text = quantity.ToString();

        if (BoxPizzaImage.sprite == null)
        {
            BoxPizzaImage.sprite = emptySprite;
        }
    }


    public void OnRightClick()
    {

    }
}
