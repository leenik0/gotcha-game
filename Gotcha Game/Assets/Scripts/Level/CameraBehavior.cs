using System.Collections;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform player;
    public ObjectLauncher objectLauncher;

    public float scrollSpeed = 5f;

    [Header("Edge Thresholds")]
    [Range(0.6f, 1.0f)] public float rightEdge = 0.95f;
    [Range(0.0f, 0.4f)] public float leftEdge = 0.05f;

    [Header("Launch Settings")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private Camera cam;
    private bool isMoving = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (isMoving || player == null) return;

        if (objectLauncher.isLaunching)
        {
            transform.position = new Vector3(player.position.x, player.position.y, offset.z);
        }

        Vector3 viewPos = cam.WorldToViewportPoint(player.position);

        if (viewPos.x > rightEdge)
        {
            StartCoroutine(ScrollScreen(true));
        }
        else if (viewPos.x < leftEdge)
        {
            StartCoroutine(ScrollScreen(false));
        }
    }

    IEnumerator ScrollScreen(bool movingRight)
    {
        isMoving = true;

        float screenWorldWidth = cam.OrthographicBounds().size.x;

        float direction = movingRight ? 1f : -1f;
        Vector3 targetPos = transform.position + new Vector3(screenWorldWidth * direction, 0, 0);

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, scrollSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}

public static class CameraExtensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        return new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
    }
}
