using UnityEngine;

public class Crane : MonoBehaviour
{
    // this goes on the crane grabber object

    public Sprite closedSprite;
    public Sprite openSprite;

    private SpriteRenderer spriteRend;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();

        if (!openSprite)
            openSprite = spriteRend.sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Collider Triggered: " + other.name);
        if(other.CompareTag("Player"))
        {
            
            PlayerController controller = other.GetComponent<PlayerController>();

            if(controller.CanBeGrabbed())
                controller.Grabbed(transform);
        }
    }

    public void UpdateCraneSprite(bool open)
    {
        //Debug.Log("Closed Sprite: " + closedSprite.name);
        //Debug.Log("Opened Sprite: " + openSprite.name);

        spriteRend.sprite = open ? openSprite : closedSprite;
    }
}
