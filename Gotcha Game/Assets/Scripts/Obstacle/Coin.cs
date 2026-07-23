using UnityEngine;

public class Coin : MonoBehaviour
{

    public int coinValue = 1;
    public float rotateSpeed = 1f;
    public AudioClip pickupSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Vehicle"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            inventory.Collect(coinValue);
            Destroy(this.gameObject);
        }
    }
}
