using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float Speed = 5f;
    public float JumpForce = 100f;

    private Rigidbody2D rig;
    private bool isGrounded = false;

    [Header("Pulo Duplo")]
    public int extraJumps = 0;
    private int jumpCount = 0;

    [Header("Rampa / Rotação")]
    public float rotationSmooth = 10f; // suavização da rotação
    private float currentAngle = 0f;
    private float angleVelocity = 0f;

    void Start()
    {
        transform.position = new Vector3(-7.8f, -2.8f, 0f);
        rig = GetComponent<Rigidbody2D>();
        rig.freezeRotation = true; // impede rotação física
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rig.linearVelocity = new Vector2(moveInput * Speed, rig.linearVelocity.y);

        // Flip horizontal
        if (moveInput > 0) transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        else if (moveInput < 0) transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < extraJumps + 1)
        {
            // Zera velocidade vertical antes do pulo
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0f);
            rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            jumpCount++;
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica se há contato com o chão (normal para cima)
        Vector2 avgNormal = Vector2.zero;
        int groundContacts = 0;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.1f)
            {
                avgNormal += contact.normal;
                groundContacts++;
            }
        }

        if (groundContacts > 0)
        {
            isGrounded = true;
            jumpCount = 0;

            avgNormal.Normalize();

            // Calcula ângulo da rampa baseado na normal média
            float targetAngle = -Mathf.Atan2(avgNormal.x, avgNormal.y) * Mathf.Rad2Deg;

            // Rotação suave
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, 1f / rotationSmooth);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Quando sai do chão
        isGrounded = false;

        // Rotação volta suavemente para 0
        currentAngle = Mathf.SmoothDampAngle(currentAngle, 0f, ref angleVelocity, 1f / rotationSmooth);
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
