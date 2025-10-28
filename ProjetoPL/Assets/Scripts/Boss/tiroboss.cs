using UnityEngine;

public class tiroboss : MonoBehaviour
{
    [Header("Configura��es")]
    public float speed = 6f;
    public float damage = 10f;
    public float lifetime = 3f;
    public float rotateSpeed = 200f; // velocidade de rota��o se estiver perseguindo

    private Vector2 direction; // dire��o do tiro
    private Transform target;     // jogador (opcional)
    private Rigidbody2D rb;
    private bool followPlayer = false;

    // Define a dire��o inicial do proj�til
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotaciona o proj�til para apontar na dire��o
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    public void SetTarget(Transform player)
    {
        if (player != null)
        {
            target = player;
            followPlayer = true;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // destr�i ap�s tempo de vida
    }

    void Update()
    {
        // Move na dire��o definida
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void FixedUpdate()
    {
        if (followPlayer && target != null)
        {
            // Persegue o jogador
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.linearVelocity = transform.up * speed;
        }
        else
        {
            // Movimento reto baseado na dire��o inicial
            rb.linearVelocity = direction * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tiroboss colidiu com: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.HealthLogic();
                playerHealth.vida--;
                Debug.Log("Jogador levou dano do tiro!");
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
