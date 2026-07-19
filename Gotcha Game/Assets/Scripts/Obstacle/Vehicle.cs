using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Vehicle : MonoBehaviour, Interactable
{

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    [Header("Audio Settings")]
    public AudioClip jumpSFX;
    public AudioClip rollSFX;

    private Rigidbody2D rb;
    private PlayerController player;

    private bool isRiding = false;
    private bool isGrounded = false;

    [Header("Tooltip Settings")]
    public TMP_Text tooltip;


    // INPUT STUFF
    private PlayerMechanics inputActions;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (tooltip)
            tooltip.gameObject.SetActive(false);
        player = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isRiding == false)
            return;
        
        player.transform.localPosition = new Vector3(0,0,0.1f);

        if(inputActions.Default.Jump.triggered && isGrounded)
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
        else
            LeaveVehicle();
    }

    public void EnterVehicle()
    {
        isRiding = true;
        player.SetCanMove(false);
        player.transform.parent = this.transform;
        player.transform.localPosition = new Vector3(0, 0, 0.1f);
        
    }

    public void LeaveVehicle()
    {
        isRiding = false;
        player.SetCanMove(true);
        player.transform.parent = null;
        player.transform.position = transform.position + Vector3.up * 2;
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

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}
