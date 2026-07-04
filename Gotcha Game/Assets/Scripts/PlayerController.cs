using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 100f;

    private Rigidbody2D rb;
    private PlayerMechanics inputActions;
    private bool isGrounded = true;


    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerMechanics();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputActions.Default.Move.ReadValue<Vector2>();
        Vector2 targetVelocity = moveInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocity.x, targetVelocity.y);
    }

    private void Update()
    {
        if (inputActions.Default.Jump.triggered && isGrounded)
        {
            Jump();
        }

        if (inputActions.Default.Jump.triggered)
        {
            Debug.Log(isGrounded);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Debug.Log("Jumpin' " + jumpForce);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        isGrounded = false;
        Debug.Log("Not Grounded smh");
    }
}
