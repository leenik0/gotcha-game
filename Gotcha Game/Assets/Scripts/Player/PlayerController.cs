using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveAcceleration = 2.5f; // DELETE THIS IF SOMETHING ELSE WORKS
    public float knockbackTime = 0.25f;
    public float jumpForce = 5f;

    [SerializeField]
    private int jumpCount = 0;

    private Rigidbody2D rb;
    private PlayerMechanics inputActions;
    private GachaController gachaController;
    // private bool isGrounded = true;

    private bool knockbacked = false;

    //[Grabbed Variables]
    // whether the player has been grabbed by a crane
    private bool grabbed = false;

    // whether the player can be grabbed by a crane
    private bool grabbable = true;

    // the amount of time before the player can be grabbed by a crane again
    private float timeTillGrabbable = 1f;

    private Transform grabbedTransform;

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
        // make this more limited if FixedUpdate is expanded
        if (grabbed || knockbacked)
            return;

        Vector2 moveInput = inputActions.Default.Move.ReadValue<Vector2>();

        //rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY); // OG Movement Code
        rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocityX, moveInput.x * moveSpeed, moveAcceleration), rb.linearVelocityY); // New Movement Code to Allow for Knockback/Lerping
    }

    private void Update()
    {
        if (inputActions.Default.Jump.triggered && (jumpCount < 2 || grabbed))
        {

            Jump();

            ReleaseGrab();
            jumpCount++;
        }

        if (inputActions.Default.Interact.triggered)
        {
            gachaController.Gacha();
        }

        if(grabbedTransform && grabbed)
        {
            transform.position = grabbedTransform.position;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // resets jump count if on ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f && !(rb.linearVelocityY > 0))
            {
                jumpCount = 0;
                return;
            }
        }
    }

    // called when a crane grabs the player
    public void Grabbed(Transform grabbedTransform)
    {
        if (grabbable == false)
            return;

        this.grabbedTransform = grabbedTransform;
        
        grabbed = true;
        grabbable = false;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;

        jumpCount = 0;
    }

    // resets the player's movement following the grab
    private void ReleaseGrab()
    {
        grabbed = false;
        transform.parent = null;
        rb.gravityScale = 1f;
        StartCoroutine(GrabTime());
    }

    // waits until the timeTillGrabbable has passed before allowing the player to become grabbable again
    private IEnumerator GrabTime()
    {
        yield return new WaitForSeconds(timeTillGrabbable);
        grabbable = true;
    }

    // makes the player unable to move for `knockbackTime` seconds
    public IEnumerator Knockback()
    {
        knockbacked = true;
        yield return new WaitForSeconds(knockbackTime);
        knockbacked = false;
    }
}
