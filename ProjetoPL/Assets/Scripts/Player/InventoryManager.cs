using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public ItemSlot selectedSlot;

    public GameObject InventoryBagMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //incializa garantindo que os slots estejam funcionando
        for (int i = 0; i < itemSlot.Length; i++)
        {
            Debug.Log("Slot " + i + " = " + itemSlot[i].gameObject.name);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //fecha o inventário e despausa o jogo
        if (Input.GetButtonDown("BagInventory") && menuActivated)
        {
            Time.timeScale = 1;
            InventoryBagMenu.SetActive(false);
            menuActivated = false;

        }
        //abre o inventário e pausa o jogo
        else if (Input.GetButtonDown("BagInventory") && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryBagMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public int AddItem(string pizzaName, int quantity, Sprite pizzaSprite)
    {
        //Tenta achar um slot que já tenha essa pizza
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

        //Se não achar, procura um slot vazio
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

        //Se o inventário estiver cheio
        return quantity;
    }



    public void DeselectAllSlots()
    {
        //condição para deselecionar os slots
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }

        selectedSlot = null; // limpa o slot selecionado
    }
}
