using UnityEngine;

public class PingPong : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float distance = 5f;

    private Vector2 startPos;

    void Start()
    {
        startPos = new Vector2(transform.position.x - distance, transform.position.y);
    }


    void Update()
    {
        float pingPongX = Mathf.PingPong(Time.time * speed, 2 * distance);

        transform.position = new Vector2(startPos.x + pingPongX, startPos.y);
    }
}
