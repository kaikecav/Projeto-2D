using UnityEngine;

public class PizzaProjectile : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Se não colidir, some depois de 3s
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // Some ao colidir
    }
}
