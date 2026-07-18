using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered Trigger");
        if (other.CompareTag("Void"))
        {
            Debug.Log("Respawning");
            transform.position = respawnPoint.position;
        }
    }
}
