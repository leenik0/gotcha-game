using TMPro;
using UnityEditor;
using UnityEngine;

public class Vehicle : MonoBehaviour, Interactable
{
    private bool isRiding = false;
    private PlayerController player;

    public TMP_Text tooltip;


    // INPUT STUFF
    private PlayerMechanics inputActions;
    private void Awake()
    {
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

        if(inputActions.Default.Jump.triggered)
        {
            
        }

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
}
