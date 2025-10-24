using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BagController : MonoBehaviour
{
    [Header("Sprites da Bolsa")]
    public Sprite bolsaAberta;
    public Sprite bolsaFechada;
    public Image bolsaImage;

    [Header("Itens de Pizza")]
    public Image pizzaImage; // imagem que mostra a pizza atual
    private int currentPizzaIndex = 0;

    [Header("Configurações")]
    public float tempoFechamento = 0.3f; // tempo que a bolsa fica fechada antes de reabrir

    private bool isClosing = false;

    private void Start()
    {
        AtualizarPizza();
    }

    private void Update()
    {
        if (isClosing) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            TrocarPizza(1); // próxima
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            TrocarPizza(-1); // anterior
        }
    }

    void TrocarPizza(int direcao)
    {
        if (BagInventory.instance.items.Count == 0) return;

        currentPizzaIndex += direcao;

        if (currentPizzaIndex >= BagInventory.instance.items.Count)
            currentPizzaIndex = 0;
        else if (currentPizzaIndex < 0)
            currentPizzaIndex = BagInventory.instance.items.Count - 1;

        StartCoroutine(FecharEAbrirBolsa());
    }

    System.Collections.IEnumerator FecharEAbrirBolsa()
    {
        isClosing = true;

        // Fecha
        bolsaImage.sprite = bolsaFechada;
        pizzaImage.enabled = false;

        yield return new WaitForSeconds(tempoFechamento);

        // Atualiza a pizza enquanto está "fechando"
        AtualizarPizza();

        // Abre
        bolsaImage.sprite = bolsaAberta;
        pizzaImage.enabled = true;

        isClosing = false;
    }

    void AtualizarPizza()
    {
        if (BagInventory.instance.items.Count == 0)
        {
            pizzaImage.enabled = false;
            return;
        }

        Item pizzaAtual = BagInventory.instance.items[currentPizzaIndex];
        pizzaImage.sprite = pizzaAtual.itemImage;
        pizzaImage.enabled = true;
    }

    public Item GetPizzaAtual()
    {
        if (BagInventory.instance.items.Count == 0) return null;
        return BagInventory.instance.items[currentPizzaIndex];
    }
}
