using UnityEngine;

public class PizzaProjectile : MonoBehaviour
{
    [Header("Tipo da pizza")]
    public string pizzaTipo; //Tipo carregado pelo PizzaShooter

    [Header("Configura��o")]
    public float lifetime = 3f; //vari�vel que determina por quanto tempo o proj�til fica ativo antes de sumir

    void Start()
    {
        //Evita clones
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Ignora colis�o com o player
        if (other.CompareTag("Player")) return;

        //Destr�i a pizza
        Debug.Log("Pizza bateu em: " + other.gameObject.name);
        Destroy(gameObject);
    }
}
