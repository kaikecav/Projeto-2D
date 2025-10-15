using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    private Rigidbody2D rig;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetAxis("Horizontal") > 0)
        {
            transform.Translate(0.05f, 0f, 0f);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.Translate(-0.05f, 0f, 0f);
        }
        */
        Move();
        Jump();
    }
    
    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime *Speed;
    }
    
    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
        }
    }
}
