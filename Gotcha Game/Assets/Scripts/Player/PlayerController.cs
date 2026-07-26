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
    public float terminalVelocity = -10f;
    public float knockbackTime = 0.25f;
    public float jumpForce = 5f;
    public ObjectLauncher objectLauncher;

    [Header("SFX Settings")]
    public AudioClip walkSFX;
    public AudioClip jumpSFX;
    private AudioSource audioSource;


    [SerializeField]
    private int jumpCount = 0;
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private PlayerMechanics inputActions;

    private bool isFacingRight = true;

    private bool knockbacked = false;

    public Animator animator;

    //[Grabbed Variables]

    // whether the player has been grabbed by a crane
    private bool grabbed = false;

    [SerializeField]
    // whether the player can be grabbed by a crane
    private bool grabbable = true;

    // the amount of time before the player can be grabbed by a crane again
    private float timeTillGrabbable = 1f;

    //private Transform grabbedTransform;

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
        inputActions = new PlayerMechanics();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        animator.SetInteger("animState", 1);
        if (LevelManager.Instance != null)
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
        //rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocityX, moveInput.x * moveSpeed, moveAcceleration), rb.linearVelocityY); // New Movement Code to Allow for Knockback/Lerping
        float moveValX = Mathf.Lerp(rb.linearVelocityX, moveInput.x * moveSpeed, moveAcceleration);
        rb.linearVelocityX = moveValX;

        // falling makes it so you're not grounded lmao
        if (rb.linearVelocityY < -0.5f)
            isGrounded = false;


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
        else if (rb.linearVelocityX == 0 && isGrounded)
        {
            if (audioSource.clip && audioSource.isPlaying)
                audioSource.Stop();

            // idle anim state
            animator.SetInteger("animState", 1);
            animator.Play("PlayerIdle", 0);
        }
        // fall anim condition
        else if (rb.linearVelocityY < -7.5f)
        {
            if (audioSource.clip && audioSource.isPlaying)
                audioSource.Stop();

            // two jump anim state
            animator.SetInteger("animState", 7);
            animator.Play("PlayerFall", 0);
        }
        // jump anim condition
        else if (jumpCount <= 1 && rb.linearVelocityY > 0)
        {

            if (audioSource.clip && audioSource.isPlaying)
                audioSource.Stop();
            
            // jump anim state
            animator.SetInteger("animState", 2);
            animator.Play("PlayerJump", 0);
            

        }
        else if(jumpCount == 2)
        {
            if (audioSource.clip && audioSource.isPlaying)
                audioSource.Stop();

            // two jump anim state
            animator.SetInteger("animState", 5);
            animator.Play("PlayerJump2", 0);
        }
        
    }

    private void Update()
    {
        if (canMove == false)
            return;

        if (inputActions.Default.Jump.triggered && (jumpCount < 2 || grabbed))
        {

            Jump();

            if(grabbed)
                ReleaseGrab();
            jumpCount++;
        }

        if (grabbed)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector2.zero, moveAcceleration);

            if (animator.GetInteger("animState") > 40)
                return;

            int hangValue = Random.Range(1,4);
            string hangString = "PlayerHang_" + hangValue.ToString();


            // the hang states are labeled 41, 42, 43, with the 4 meaning the general hang state, and the 1,2,3 being the variants
            animator.SetInteger("animState", 40 + hangValue);
            animator.Play(hangString, 0);

            Debug.Log("HangValue: " + hangValue);
            Debug.Log("HangString: " + hangString);

        }
    }

    private void Jump()
    {
        rb.linearVelocityY = 0;
        rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        if(jumpSFX)
            AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
        isGrounded = false;
        
    }

    // sets the jump count variable to force animation and jump numbers
    public void SetJumpCount(int jumpNum)
    {
        jumpCount = jumpNum;

        if (jumpCount != 0)
            isGrounded = false;
    }

    // resets jump count if on ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f && !(rb.linearVelocityY > 0))
            {
                SetJumpCount(0);
                isGrounded = true;
                objectLauncher.isLaunching = false;
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
        return grabbable && grabbed == false;
    }

    // called when a crane grabs the player
    public void Grabbed(Transform craneTransform)
    {
        if (grabbable == false)
            return;

        StopAllCoroutines();
        //animator.SetInteger("animState", 4);


        grabbed = true;
        grabbable = false;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;

        Debug.Log("Crane Transform???: " + craneTransform);
        transform.parent = craneTransform;

        SetJumpCount(0);

        Crane crane = transform.parent.GetComponent<Crane>();
        crane.UpdateCraneSprite(false);

    }

    // resets the player's movement following the grab
    private void ReleaseGrab()
    {
        grabbed = false;

        //updates the crane sprite

        Crane crane = transform.parent.gameObject.GetComponent<Crane>();
        if(crane)
            crane.UpdateCraneSprite(true);

        transform.parent = null;
        transform.localRotation = Quaternion.identity;
        rb.gravityScale = 1f;
        StartCoroutine(GrabTime());
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        //playerCollider.isTrigger = !canMove;
        rb.gravityScale = canMove ? 1f : 0f;

        if (!canMove)
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
