using UnityEngine;

public class SwingingCrane : Crane
{


    [Range(0,180)]
    public float swingAngle = 45f;

    public float swingSpeed = 30f;

    private Transform cranePivot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cranePivot = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        cranePivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.PingPong(Time.time * swingSpeed, swingAngle) - swingAngle/2);
    }
}
