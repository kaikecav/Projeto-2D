using UnityEngine;
using UnityEngine.SceneManagement; // <- Import necess�rio para mudar de cena

public class BossController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 2f;
    private bool movingRight = true;

    [Header("Detec��o de ambiente")]
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public float checkDistance = 0.5f;

    [Header("Detec��o do Jogador")]
    public float detectionRange = 6f;
    private Transform player;
    private bool chasingPlayer = false;

    [Header("Ataque")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    private float attackTimer = 0f;

    [Header("Sprites / Estados")]
    public Sprite idleSprite;
    public Sprite walkSprite;
    public Sprite attackSprite;
    public Sprite deathSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Vida do Boss")]
    public int vida = 5;
    private bool isDead = false;

    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SetState(BossState.Idle);
    }

    void Update()
    {
        if (isDead || player == null) return;

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
            MoveEnemy();
        }
    }

    // === MOVIMENTO ===
    void MoveEnemy()
    {
        SetState(BossState.Walk);
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * (movingRight ? 1 : -1));
    }

    void ChasePlayer()
    {
        SetState(BossState.Walk);
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * 1.2f * Time.deltaTime);

        if (player.position.x > transform.position.x && !movingRight) Flip();
        else if (player.position.x < transform.position.x && movingRight) Flip();
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
        if (projectilePrefab == null || firePoint == null) return;

        SetState(BossState.Attack);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;

        tiroboss tiro = proj.GetComponent<tiroboss>();
        if (tiro != null)
        {
            tiro.SetDirection(dir);
            tiro.SetTarget(player);
        }

        // Retorna pro estado de Idle ap�s pequeno delay
        Invoke(nameof(ReturnToIdle), 0.5f);
    }

    void ReturnToIdle()
    {
        if (!isDead) SetState(BossState.Idle);
    }

    // === DETEC��O DE CH�O E PAREDE ===
    void CheckEnvironment()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, checkDistance + 0.1f, groundLayer);

        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // === DANO AO PLAYER POR CONTATO ===
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.HealthLogic();
                playerHealth.vida--;
                Debug.Log("Jogador levou dano por contato com o Boss!");
            }
        }
    }

    // === DANO RECEBIDO (usado pelo ComerPizzaBoss) ===
    public void LevarDano(int dano)
    {
        if (isDead) return;

        vida -= dano;
        Debug.Log($"Boss recebeu {dano} de dano. Vida restante: {vida}");

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        if (isDead) return;

        isDead = true;
        SetState(BossState.Death);
        Debug.Log("Boss derrotado!");

        // Impede o Boss de continuar se mexendo
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        // Aguarda 1,5 segundos para mudar a cena (tempo da anima��o de morte)
        Invoke(nameof(CarregarTelaVitoria), 1.5f);
    }

    void CarregarTelaVitoria()
    {
        SceneManager.LoadScene("TelaVitoria");
    }

    // === GERENCIAMENTO DE ESTADOS VISUAIS ===
    enum BossState { Idle, Walk, Attack, Death }

    void SetState(BossState state)
    {
        if (spriteRenderer == null) return;

        switch (state)
        {
            case BossState.Idle:
                spriteRenderer.sprite = idleSprite;
                break;
            case BossState.Walk:
                spriteRenderer.sprite = walkSprite;
                break;
            case BossState.Attack:
                spriteRenderer.sprite = attackSprite;
                break;
            case BossState.Death:
                spriteRenderer.sprite = deathSprite;
                break;
        }
    }

    // === DEBUG VISUAL ===
    private void OnDrawGizmosSelected()
    {
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
