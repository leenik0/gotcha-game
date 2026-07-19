using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveAcceleration = 2.5f; // DELETE THIS IF SOMETHING ELSE WORKS
    public float knockbackTime = 0.25f;
    public float jumpForce = 5f;

    [Header("SFX Settings")]
    public AudioClip walkSFX;
    public AudioClip jumpSFX;
    private AudioSource audioSource;


    [SerializeField]
    private int jumpCount = 0;
    private bool isGrounded = false;

    private Rigidbody2D rb;
<<<<<<< Updated upstream
    private Collider2D playerCollider;
    private PlayerMechanics inputActions;
=======
    public PlayerMechanics inputActions;
    [SerializeField] private GachaController gachaController;
>>>>>>> Stashed changes
    private bool isFacingRight = true;

    private bool knockbacked = false;

    private Animator animator;

    //[Grabbed Variables]
    
    // whether the player has been grabbed by a crane
    private bool grabbed = false;

    // whether the player can be grabbed by a crane
    private bool grabbable = true;

    // the amount of time before the player can be grabbed by a crane again
    private float timeTillGrabbable = 1f;

    private Transform grabbedTransform;

    // allows the player to move when true; useful for when otherwise occupied
    private bool canMove = true;

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
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        inputActions = new PlayerMechanics();
    }

    private void Start()
    {
        animator.SetInteger("animState", 1);
        if(LevelManager.Instance != null)
            transform.position = LevelManager.Instance.GetRespawnPosition();
        if (audioSource.clip == null)
            audioSource.clip = walkSFX;
    }

    private void FixedUpdate()
    {

        
        // make this more limited if FixedUpdate is expanded
        if (grabbed || knockbacked || canMove == false)
            return;

        Vector2 moveInput = inputActions.Default.Move.ReadValue<Vector2>();

        //rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY); // OG Movement Code
        rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocityX, moveInput.x * moveSpeed, moveAcceleration), rb.linearVelocityY); // New Movement Code to Allow for Knockback/Lerping

        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }

        // walk anim condition
        if (rb.linearVelocityX != 0 && isGrounded)
        {
            // if moving on ground, play walk sound
            if (audioSource.clip && audioSource.isPlaying == false)
                audioSource.Play();
            // walk anim state
            animator.SetInteger("animState", 3);
            animator.Play("PlayerWalk", 0);
        }
        // idle anim condition
        else if(rb.linearVelocityX == 0 && isGrounded)
        {
            if (audioSource.clip && audioSource.isPlaying)
                audioSource.Stop();

            // idle anim state
            animator.SetInteger("animState", 1);
            animator.Play("PlayerIdle", 0);
        }
        // jump anim condition
        else
        {

            if (audioSource.clip && audioSource.isPlaying)
                audioSource.Stop();

            // jump anim state
            animator.SetInteger("animState", 2);
            animator.Play("PlayerJump", 0);
        }
    }

    private void Update()
    {
        if (canMove == false)
            return;

        if (inputActions.Default.Jump.triggered && (jumpCount < 2 || grabbed))
        {

            Jump();

            ReleaseGrab();
            jumpCount++;
        }

        if (grabbedTransform && grabbed)
        {
            transform.position = grabbedTransform.position;
            animator.SetInteger("animState", 4);

        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if(jumpSFX)
            AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
        isGrounded = false;
    }

    // resets jump count if on ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f && !(rb.linearVelocityY > 0))
            {
                jumpCount = 0;
                isGrounded = true;
                return;
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public bool CanBeGrabbed()
    {
        return grabbable;
    }

    // called when a crane grabs the player
    public void Grabbed(Transform grabbedTransform)
    {
        if (grabbable == false)
            return;

        animator.SetInteger("animState", 4);
        

        grabbed = true;
        grabbable = false;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        
        this.grabbedTransform = grabbedTransform;
        
        jumpCount = 0;
    }

    // resets the player's movement following the grab
    private void ReleaseGrab()
    {
        grabbable = false;
        grabbed = false;
        transform.parent = null;
        rb.gravityScale = 1f;
        StartCoroutine(GrabTime());
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        playerCollider.enabled = canMove;
        rb.gravityScale = canMove ? 1f : 0f;

        if(!canMove)
        {
            rb.linearVelocity = Vector2.zero;
        }
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
