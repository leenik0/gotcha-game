using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Vehicle : MonoBehaviour, Interactable
{

    public bool enterOnStart = false;
    public bool enterOnCollision = false;
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public bool canExit = true;
    public bool canJump = false;

    [Header("Audio Settings")]
    public AudioClip jumpSFX;
    public AudioClip rollSFX;
    public AudioClip thudSFX;

    private Rigidbody2D rb;
    private PlayerController player;
    private Collider2D playerCollider;
    private Animator animator;
    private AudioSource audioSource;

    private bool isRiding = false;
    private bool isGrounded = false;

    [Header("VFX Settings")]
    public GameObject entranceVFX;
    public GameObject exitVFX;


    // INPUT STUFF
    private PlayerMechanics inputActions;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        inputActions = new PlayerMechanics();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        playerCollider = player.GetComponent<Collider2D>();
        animator.SetInteger("animState", 0);
        audioSource.clip = rollSFX;

        tag = "Vehicle";

        if (enterOnStart)
            EnterVehicle();
    }

    // Update is called once per frame
    void Update()
    {

        if (audioSource.isPlaying == false && isGrounded && (rb.linearVelocityX != 0 || rb.totalTorque != 0))
            audioSource.Play();
        if(audioSource.isPlaying == true && (isGrounded == false || (rb.linearVelocityX == 0 && rb.totalTorque == 0)))
            audioSource.Stop();

        if (isRiding == false)
            return;
        
        player.transform.localPosition = new Vector3(0,0,0.1f);

        if(canJump && inputActions.Default.Jump.triggered && isGrounded)
        {
            Jump();
        }

        

    }

    private void FixedUpdate()
    {
        if (isRiding == false)
            return;

        Vector2 moveInput = inputActions.Default.Move.ReadValue<Vector2>();
        rb.AddTorque(-moveInput.x * moveSpeed);

        if (isGrounded == false)
            rb.AddForceX(moveInput.x * moveSpeed * 0.5f);
    }

    public void Interact()
    {
        Debug.Log("Interacted");

        if (isRiding == false)
            EnterVehicle();
        else if (canExit)
            LeaveVehicle();
    }

    public void EnterVehicle()
    {
        if(entranceVFX)
            Instantiate(entranceVFX);
        
        isRiding = true;
        playerCollider.enabled = false;
        animator.SetInteger("animState", 1);
        player.SetCanMove(false);
        player.transform.parent = this.transform;
        player.transform.localPosition = new Vector3(0, 0, 0.1f);
        rb.linearVelocity = Vector2.zero;
        
    }

    public bool GetIsRiding()
    {
        return isRiding;
    }

    public void LeaveVehicle()
    {
        isRiding = false;
        animator.SetInteger("animState", 0);
        player.SetCanMove(true);
        playerCollider.enabled = true;
        player.transform.parent = null;
        player.transform.position = transform.position;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (jumpSFX)
            AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (thudSFX)
            AudioSource.PlayClipAtPoint(thudSFX, transform.position);

        if (enterOnCollision && collision.gameObject.CompareTag("Player"))
            EnterVehicle();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
