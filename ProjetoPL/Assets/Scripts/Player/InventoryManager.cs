using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryBagMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("BagInventory") && menuActivated)
        {
            Time.timeScale = 1;
            InventoryBagMenu.SetActive(false);
            menuActivated = false;

        }
        else if (Input.GetButtonDown("BagInventory") && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryBagMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddItem(string pizzaName, int quantity, Sprite pizzaSprite)
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem (pizzaName, quantity, pizzaSprite);
                return;
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0;i < itemSlot.Length;i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
