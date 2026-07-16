using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamagingObstacle : MonoBehaviour
{

    public int damage = 1;
    public float bounceForce = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // damage the player
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth.CanBeDamaged() == false)
            return;

        playerHealth.Damage(damage);


        // player bounce upon damage
        if (bounceForce == 0)
            return;

        Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
        PlayerController playerController = other.GetComponent<PlayerController>();

        Vector2 playerDirection = (other.transform.position - transform.position).normalized;

        Debug.Log("PlayerDirection: " + playerDirection);

        playerRB.AddForce(playerDirection * bounceForce, ForceMode2D.Impulse);
        StartCoroutine(playerController.Knockback());
    }
}
