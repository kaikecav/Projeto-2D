using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float Speed = 5f;
    public float JumpForce = 10f;

    private Rigidbody2D rig;
    private bool isGrounded = false;
    private bool isTouchingWall = false;

    [Header("Pulo Duplo")]
    public int extraJumps = 1; // 1 = pulo duplo
    private int jumpCount = 0;

    [Header("Rampa / Rotação")]
    public float rotationSmooth = 10f; // suavização da rotação
    private float currentAngle = 0f;
    private float angleVelocity = 0f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.freezeRotation = true;
        rig.gravityScale = 2f;

        // 🧱 Garante que o personagem não terá atrito físico
        PhysicsMaterial2D noFriction = new PhysicsMaterial2D("NoFriction");
        noFriction.friction = 0f;
        noFriction.bounciness = 0f;
        GetComponent<Collider2D>().sharedMaterial = noFriction;
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
        if (moveInput > 0)
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < extraJumps + 1)
        {
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0f);
            rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            jumpCount++;
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 avgNormal = Vector2.zero;
        int groundContacts = 0;
        bool wallContact = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Contato com chão/rampa
            if (contact.normal.y > 0.5f)
            {
                avgNormal += contact.normal;
                groundContacts++;
            }
            // Contato lateral (parede)
            else if (Mathf.Abs(contact.normal.x) > 0.7f && contact.normal.y < 0.3f)
            {
                wallContact = true;
            }
        }

        if (groundContacts > 0)
        {
            // 🟢 CHÃO / RAMPA
            isGrounded = true;
            isTouchingWall = false;
            jumpCount = 0;

            avgNormal.Normalize();
            float targetAngle = -Mathf.Atan2(avgNormal.x, avgNormal.y) * Mathf.Rad2Deg;

            // Rotação suave para rampas
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, 1f / rotationSmooth);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
        else if (wallContact)
        {
            // 🔵 PAREDE — desliza sem grudar
            isGrounded = false;
            isTouchingWall = true;

            // Garante que ele caia naturalmente
            if (rig.linearVelocity.y > -10f)
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, Mathf.Max(rig.linearVelocity.y - 0.2f, -10f));
        }
        else
        {
            // 🟠 NO AR
            isGrounded = false;
            isTouchingWall = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        isTouchingWall = false;

        // Volta suavemente à rotação neutra
        currentAngle = Mathf.SmoothDampAngle(currentAngle, 0f, ref angleVelocity, 1f / rotationSmooth);
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
