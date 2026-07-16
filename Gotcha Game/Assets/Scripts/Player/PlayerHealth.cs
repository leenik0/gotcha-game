using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    public float immunityTime = 0.25f;
    private float lastDamageTime = 0f;

    [Header("SFX Settings")]
    public AudioClip hurtSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Damages the Player
    public void Damage(int damage)
    {
        // allows for invulnerability frames
        if (!CanBeDamaged())
            return;

        lastDamageTime = Time.time;

        // makes sound when hurt
        if (hurtSFX)
            AudioSource.PlayClipAtPoint(hurtSFX, transform.position); // i dont wanna put an actual audiosource smh my head
        
        currentHealth -= damage;

        // kills the player lmao
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    // returns true if the player can be damaged, which is based on if the immunity time has passed since last being damaged
    public bool CanBeDamaged()
    {
        return Time.time > lastDamageTime + immunityTime;
    }

    public void Die()
    {
        // DIE ANIMATION OR SMTH LMAO
        Debug.Log($"Dead ({currentHealth} health)");

        Respawn(true);
    }

    // respawns the player
    public void Respawn(bool resetHealth)
    {
        // add respawn animation perhaps

        if (resetHealth)
            currentHealth = maxHealth;
        transform.position = LevelManager.Instance.GetRespawnPosition();
    }
}
