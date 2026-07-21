using UnityEngine;

public class SwapCamera : MonoBehaviour
{
    public CameraBehavior cameraBehavior;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (cameraBehavior)
        {
            if (collision.CompareTag("Player"))
            {
                cameraBehavior.isNotPlinko = false;
            }
        }
    }
}
