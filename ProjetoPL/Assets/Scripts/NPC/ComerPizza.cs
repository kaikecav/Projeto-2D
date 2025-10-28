using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PizzaPedido
{
    public string pizzaTipo; // ex: "Pizza Queijo"
    public int quantidadeNecessaria;
}

public class ComerPizza : MonoBehaviour
{
    [Header("Pedidos do NPC")]
    public List<PizzaPedido> pedidos = new List<PizzaPedido>();     //Cria uma lista com os pedidos dos NPC

    [Header("Sprites do NPC")]
    public Sprite triste;
    public Sprite feliz;

    [Header("Pedido Visual")]
    public SpriteRenderer pedidoSpriteRenderer; // SpriteRenderer acima da cabeça do NPC
    public Sprite pizzaQueijoSprite;
    public Sprite pizzaCalabresaSprite;
    public Sprite pizzaFrangoSprite;
    public Sprite pizzaMargueritaSprite;
    public Sprite pizzaDoceSprite;
    public Sprite pizzaAbacaxiSprite;

    private SpriteRenderer spriteRenderer;
    private bool estaSatisfeito = false;

    void Start()
    {
        //Adiciona o componente que faz a sprite do pedido aparecer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && triste != null)
            spriteRenderer.sprite = triste;

        AtualizaPedidoVisual();
    }

    // Atualiza o pedido visual com a primeira pizza que ainda falta
    void AtualizaPedidoVisual()
    {
        if (pedidoSpriteRenderer == null)
            return;

        foreach (PizzaPedido pedido in pedidos)
        {
            if (pedido.quantidadeNecessaria > 0)
            {
                // Define o sprite baseado no tipo
                switch (pedido.pizzaTipo.ToLower().Replace(" ", ""))
                {
                    case "pizzaqueijo":
                        pedidoSpriteRenderer.sprite = pizzaQueijoSprite;
                        break;
                    case "pizzacalabresa":
                        pedidoSpriteRenderer.sprite = pizzaCalabresaSprite;
                        break;
                    case "pizzafrango":
                        pedidoSpriteRenderer.sprite = pizzaFrangoSprite;
                        break;
                    case "pizzamarguerita":
                        pedidoSpriteRenderer.sprite = pizzaMargueritaSprite;
                        break;
                    case "pizzadoce":
                        pedidoSpriteRenderer.sprite = pizzaDoceSprite;
                        break;
                    case "pizzaabacaxi":
                        pedidoSpriteRenderer.sprite = pizzaAbacaxiSprite;
                        break;
                    default:
                        pedidoSpriteRenderer.sprite = null;
                        break;
                }

                pedidoSpriteRenderer.gameObject.SetActive(true);
                return;
            }
        }

        // Se não houver mais pizzas pendentes
        pedidoSpriteRenderer.gameObject.SetActive(false);
        estaSatisfeito = true;

        if (spriteRenderer != null && feliz != null)
            spriteRenderer.sprite = feliz;

        // ✅ Se for inimigo e estiver satisfeito, desaparece
        if (CompareTag("Inimigo"))
        {
            // pode colocar um pequeno delay se quiser ver o sprite feliz antes de sumir
            Destroy(gameObject, 0.8f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (estaSatisfeito) return;

        PizzaProjectile pizza = other.GetComponent<PizzaProjectile>();
        if (pizza != null)
        {
            foreach (PizzaPedido pedido in pedidos)
            {
                if (pedido.quantidadeNecessaria > 0 &&
                    pizza.pizzaTipo.ToLower().Replace(" ", "") ==
                    pedido.pizzaTipo.ToLower().Replace(" ", ""))
                {
                    // Entrega válida
                    pedido.quantidadeNecessaria--;
                    Destroy(other.gameObject);
                    Debug.Log($"{gameObject.name} recebeu uma {pizza.pizzaTipo}. Faltam {pedido.quantidadeNecessaria} desse tipo");

                    AtualizaPedidoVisual();
                    return;
                }
            }

            // Pizza não desejada
            Debug.Log($"{gameObject.name} recusou a {pizza.pizzaTipo}");
        }
    }
}
