using UnityEngine;

public class PizzaShooter : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public Transform shootPoint;
    public float shootForce = 10f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject PizzaPrefab = Instantiate(pizzaPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = PizzaPrefab.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(shootForce, 0);
    }
}
