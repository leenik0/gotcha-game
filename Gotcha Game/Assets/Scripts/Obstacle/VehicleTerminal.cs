using TMPro;
using UnityEngine;

public class VehicleTerminal : MonoBehaviour, Interactable
{

    public TMP_Text promptText;
    public GameObject vehicle;
    public GameObject door;

    public bool canEnter = true;
    public bool canLeave = true;

    public static Vehicle spawnedVehicle;


    private static bool hasSpawned = false;
    private PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (promptText && other.CompareTag("Player"))
        {
            promptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (promptText && other.CompareTag("Player"))
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (hasSpawned == false && canEnter)
        { 
            if(!vehicle)
            {
                Debug.Log("Vehicle Not Set Stupid");
                return;
            }

            player.SetCanMove(false);
            spawnedVehicle = Instantiate(vehicle, player.transform.position + Vector3.up * 10f, Quaternion.identity).GetComponent<Vehicle>();
            //obj.EnterVehicle();
            
            hasSpawned = true;
        }
        else if (canLeave)
        {
            if(spawnedVehicle == null)
            {
                Debug.Log("Spawned Vehicle Doesn't Exist smh");
                return;
            }
            Debug.Log("Leaving Vehicle");

            player.SetCanMove(true);
            spawnedVehicle.LeaveVehicle();
            Destroy(spawnedVehicle.gameObject);
            spawnedVehicle = null;
            hasSpawned = false;
        }

        if (door)
            door.SetActive(false);

    }
}
