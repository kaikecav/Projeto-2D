using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 2f;
    private bool movingRight = true;

    [Header("Detec��o de ambiente")]
    public Transform groundCheck;   // objeto que fica na frente dos p�s
    public Transform wallCheck;     // objeto que fica no corpo
    public LayerMask groundLayer;
    public float checkDistance = 0.5f; // dist�ncia do raycast

    [Header("Detec��o do Jogador")]
    public float detectionRange = 6f;
    private Transform player;
    private bool chasingPlayer = false;

    [Header("Ataque")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    private float attackTimer = 0f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        CheckEnvironment();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        chasingPlayer = distanceToPlayer <= detectionRange;

        if (chasingPlayer)
        {
            ChasePlayer();
            HandleAttack();
        }
        else
        {
            MoveEnemy(); // anda sozinho at� encontrar parede ou buraco
        }
    }

    // === MOVIMENTO AUTOM�TICO ===
    void MoveEnemy()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * (movingRight ? 1 : -1));
    }

    // === PERSEGUI��O DO JOGADOR ===
    void ChasePlayer()
    {
        // Move em dire��o ao jogador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * 1.2f * Time.deltaTime);

        // Inverte sprite conforme a posi��o do jogador
        if (player.position.x > transform.position.x && !movingRight)
            Flip();
        else if (player.position.x < transform.position.x && movingRight)
            Flip();
    }

    // === ATAQUE ===
    void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void Attack()
    {
        if (projectilePrefab == null || player == null) return;

        if (projectilePrefab == null || firePoint == null) return;

        // Instancia o proj�til
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Define dire��o inicial com base no flip do inimigo
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;

        // Passa a dire��o inicial e o alvo (opcional)
        tiroboss tiro = proj.GetComponent<tiroboss>();
        tiro.SetDirection(dir);
        tiro.SetTarget(player); // opcional: permite que o proj�til persiga o jogador
    }

    // === DETEC��O DE CH�O E PAREDE ===
    void CheckEnvironment()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, checkDistance + 0.1f, groundLayer);

        // Se n�o tiver ch�o ou tiver parede, vira
        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    // === INVERTE O SPRITE ===
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // === DEBUG NO EDITOR ===
    private void OnDrawGizmosSelected()
    {
        // Raio de detec��o do jogador
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * checkDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (movingRight ? Vector3.right : Vector3.left) * (checkDistance + 0.1f));
        }
    }
}
