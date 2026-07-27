using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlinkoLauncher : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpRef;
    public float launchForce = 10f;
    public Transform spriteTransform;
    public Vector3 centerPoint;
    private float previousAngle = 0f;

    void Start()
    {
        centerPoint = transform.GetChild(0).transform.position;
        spriteTransform = transform;
    }

    private void OnEnable()
    {
        jumpRef.action.Enable();
    }

    private void OnDisable()
    {
        jumpRef.action.Disable();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Vehicle"))
        {
            if (jumpRef.action.triggered)
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
    }

    private IEnumerator LaunchPlinko()
    {
        float duration = 0.5f;

        DOVirtual.Float(0f, -15f, duration, onVirtualUpdate: (float currentAngle) =>
            {
                float deltaAngle = currentAngle - previousAngle;
                spriteTransform.RotateAround(centerPoint, Vector3.forward, deltaAngle);
                previousAngle = currentAngle;
            }).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(1.5f);

        DOVirtual.Float(-15f, 0f, duration, onVirtualUpdate: (float currentAngle) =>
            {
                float deltaAngle = currentAngle - previousAngle;
                spriteTransform.RotateAround(centerPoint, Vector3.forward, deltaAngle);
                previousAngle = currentAngle;
            }).SetEase(Ease.OutQuad);
    }
}
