using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    public float launchForce = 100f;
    public bool isLaunching = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Spring Entered");
        if (other.CompareTag("Player"))
        {
            isLaunching = true;
            Debug.Log("Launching");
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.AddForce(transform.up * launchForce, ForceMode2D.Impulse);
            }
        }

    }
}
