using UnityEngine;

public class PizzaProjectile : MonoBehaviour
{
    [Header("Tipo da pizza")]
    public string pizzaTipo; //Tipo carregado pelo PizzaShooter

    [Header("Configuração")]
    public float lifetime = 3f; //variável que determina por quanto tempo o projétil fica ativo antes de sumir

    void Start()
    {
        //Evita clones
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Ignora colisão com o player
        if (other.CompareTag("Player")) return;

        //Destrói a pizza
        Debug.Log("Pizza bateu em: " + other.gameObject.name);
        Destroy(gameObject);
    }
}
