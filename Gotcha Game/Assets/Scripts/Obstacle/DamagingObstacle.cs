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
        // bounce
        Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();
        if (otherRB != null && bounceForce != 0)
        {
            Vector2 otherDirection = (other.transform.position - transform.position).normalized;

            Debug.Log("OtherDirection: " + otherDirection);

            otherRB.AddForce(otherDirection * bounceForce, ForceMode2D.Impulse);
        }



        if (!(other.CompareTag("Player")))
            return;
        
        // player knockback upon damage
        PlayerController playerController = other.GetComponent<PlayerController>();
        StartCoroutine(playerController.Knockback());

        // damage the player
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth.CanBeDamaged() == false)
            return;

        playerHealth.Damage(damage);


    }
}
