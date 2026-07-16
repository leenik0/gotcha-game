using UnityEngine;

public class MoveableObstacle : MonoBehaviour
{

    public Vector2 pos1;
    public Vector2 pos2;

    private Vector2 initialPos;

    private bool movingToPos1 = true;

    public float moveSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(movingToPos1)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos1 + initialPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2 + initialPos, Time.deltaTime * moveSpeed);
        }

        // checks if close to pos1
        if (movingToPos1 == true && Vector3.Distance(transform.position, pos1 + initialPos) < 0.15f)
            movingToPos1 = false;

        // checks if close to pos2
        if (movingToPos1 == false && Vector3.Distance(transform.position, pos2 + initialPos) < 0.15f)
            movingToPos1 = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.darkGreen;
        Gizmos.DrawLine(transform.position, (Vector3)pos1 + transform.position);
        Gizmos.DrawLine(transform.position, (Vector3)pos2 + transform.position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine((Vector3)pos1 + transform.position, (Vector3)pos2 + transform.position);
    }
}
