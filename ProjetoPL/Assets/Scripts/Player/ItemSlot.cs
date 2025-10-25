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

    public void AddItem(string pizzaName, int quantity, Sprite pizzaSprite)
    {
        this.pizzaName = pizzaName;
        this.quantity = quantity;
        this.pizzaSprite = pizzaSprite;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        pizzaImage.sprite = pizzaSprite;
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
        selectedShader.SetActive(true);
        thisItemSelected = true;
        BoxPizzaNameText.text = pizzaName;
        BoxPizzaImage.sprite = pizzaSprite;
        if (BoxPizzaImage.sprite == null)
        {
            BoxPizzaImage.sprite = emptySprite;
        }
    }

    public void OnRightClick()
    {

    }
}
