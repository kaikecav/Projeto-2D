using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PizzaPedidoBoss
{
    public string pizzaTipo;
    public int quantidadeNecessaria;
}

public class ComerPizzaBoss : MonoBehaviour
{
    [Header("Pedidos do Boss")]
    public List<PizzaPedidoBoss> pedidos = new List<PizzaPedidoBoss>();

    [Header("Referências")]
    public BossController bossController;
    public SpriteRenderer pedidoSpriteRenderer;
    public Sprite pizzaQueijoSprite;
    public Sprite pizzaCalabresaSprite;
    public Sprite pizzaFrangoSprite;
    public Sprite pizzaMargueritaSprite;
    public Sprite pizzaDoceSprite;
    public Sprite pizzaAbacaxiSprite;

    private bool estaSatisfeito = false;

    void Start()
    {
        if (bossController == null)
            bossController = GetComponent<BossController>();

        AtualizaPedidoVisual();
    }

    void AtualizaPedidoVisual()
    {
        if (pedidoSpriteRenderer == null)
            return;

        foreach (PizzaPedidoBoss pedido in pedidos)
        {
            if (pedido.quantidadeNecessaria > 0)
            {
                switch (pedido.pizzaTipo.ToLower().Replace(" ", ""))
                {
                    case "pizzaqueijo": pedidoSpriteRenderer.sprite = pizzaQueijoSprite; break;
                    case "pizzacalabresa": pedidoSpriteRenderer.sprite = pizzaCalabresaSprite; break;
                    case "pizzafrango": pedidoSpriteRenderer.sprite = pizzaFrangoSprite; break;
                    case "pizzamarguerita": pedidoSpriteRenderer.sprite = pizzaMargueritaSprite; break;
                    case "pizzadoce": pedidoSpriteRenderer.sprite = pizzaDoceSprite; break;
                    case "pizzaabacaxi": pedidoSpriteRenderer.sprite = pizzaAbacaxiSprite; break;
                    default: pedidoSpriteRenderer.sprite = null; break;
                }
                pedidoSpriteRenderer.gameObject.SetActive(true);
                return;
            }
        }

        pedidoSpriteRenderer.gameObject.SetActive(false);
        estaSatisfeito = true;
        if (bossController != null)
        {
            bossController.LevarDano(bossController.vida); // mata o boss
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (estaSatisfeito) return;

        PizzaProjectile pizza = other.GetComponent<PizzaProjectile>();
        if (pizza != null)
        {
            foreach (PizzaPedidoBoss pedido in pedidos)
            {
                if (pedido.quantidadeNecessaria > 0 &&
                    pizza.pizzaTipo.ToLower().Replace(" ", "") ==
                    pedido.pizzaTipo.ToLower().Replace(" ", ""))
                {
                    pedido.quantidadeNecessaria--;
                    Destroy(other.gameObject);
                    Debug.Log($"Boss recebeu uma {pizza.pizzaTipo}. Faltam {pedido.quantidadeNecessaria} desse tipo");
                    AtualizaPedidoVisual();
                    return;
                }
            }
            Debug.Log($"Boss recusou a {pizza.pizzaTipo}");
        }
    }
}
