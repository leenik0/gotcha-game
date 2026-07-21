using UnityEngine;

public class VehicleTerminal : MonoBehaviour, Interactable
{

    public GameObject vehicle;
    public GameObject door;

    public bool canEnter = true;
    public bool canLeave = true;

    private bool hasSpawned = false;
    public static Vehicle spawnedVehicle;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            spawnedVehicle = Instantiate(vehicle, transform.position + Vector3.back * 0.1f, Quaternion.identity).GetComponent<Vehicle>();
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
            spawnedVehicle.LeaveVehicle();
            Destroy(spawnedVehicle.gameObject);
            spawnedVehicle = null;
            hasSpawned = false;
        }

        if (door)
            door.SetActive(false);

    }
}
