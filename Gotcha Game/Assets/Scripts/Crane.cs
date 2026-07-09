using UnityEngine;

public class Crane : MonoBehaviour
{
    // this goes on the crane grabber object


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Collider Triggered: " + other.name);
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Grabbed(this.transform);
        }
    }
}
