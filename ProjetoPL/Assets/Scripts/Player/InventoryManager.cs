using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryBagMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            Debug.Log("Slot " + i + " = " + itemSlot[i].gameObject.name);
        }
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

    public int AddItem(string pizzaName, int quantity, Sprite pizzaSprite)
    {
        // 1️⃣ Primeiro: tenta achar um slot que JÁ TENHA essa pizza
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].pizzaName == pizzaName && itemSlot[i].quantity > 0 && !itemSlot[i].isFull)
            {
                int leftOver = itemSlot[i].AddItem(pizzaName, quantity, pizzaSprite);
                if (leftOver > 0)
                    return AddItem(pizzaName, leftOver, pizzaSprite);
                return 0;
            }
        }

        // 2️⃣ Depois: se não achar, procura um slot vazio
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].quantity == 0 && !itemSlot[i].isFull)
            {
                int leftOver = itemSlot[i].AddItem(pizzaName, quantity, pizzaSprite);
                if (leftOver > 0)
                    return AddItem(pizzaName, leftOver, pizzaSprite);
                return 0;
            }
        }

        // 3️⃣ Se o inventário estiver cheio
        return quantity;
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
