using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Side { left, right };

public class PlinkoLauncher : MonoBehaviour
{
    public float launchForce = 10f;
    public Transform spriteTransform;
    public Vector3 centerPoint;
    public Side side;
    private float previousAngle = 0f;
    private float angle;

    void Start()
    {
        centerPoint = transform.GetChild(0).transform.position;
        spriteTransform = transform;
        switch (side)
        {
            case Side.left:
                angle = 15;
                break;
            case Side.right:
                angle = -15;
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vehicle"))
        {
            StartCoroutine(LaunchPlinko());
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(transform.up * launchForce, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator LaunchPlinko()
    {
        float duration = 0.5f;

        DOVirtual.Float(0f, angle, duration, onVirtualUpdate: (float currentAngle) =>
            {
                float deltaAngle = currentAngle - previousAngle;
                spriteTransform.RotateAround(centerPoint, Vector3.forward, deltaAngle);
                previousAngle = currentAngle;
            }).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(1.5f);

        DOVirtual.Float(angle, 0f, duration, onVirtualUpdate: (float currentAngle) =>
            {
                float deltaAngle = currentAngle - previousAngle;
                spriteTransform.RotateAround(centerPoint, Vector3.forward, deltaAngle);
                previousAngle = currentAngle;
            }).SetEase(Ease.OutQuad);
    }
}
