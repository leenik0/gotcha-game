using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{

    public float interactRange = 2f;

    private Interactable nearestInteractable;

    // INPUT STUFF
    private PlayerMechanics inputActions;
    private void Awake()
    {
        inputActions = new PlayerMechanics();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputActions.Default.Interact.triggered)
        {
            FindNearestInteractable();

            if (nearestInteractable == null)
                return;

            nearestInteractable.Interact();
        }
    }

    // makes a circle around the player and checks for any interactable objects. Sets the closest object to the `nearestInteractable` var
    public void FindNearestInteractable()
    {
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, interactRange, ContactFilter2D.noFilter, results);

        IEnumerable<Collider2D> interactables = results.Where(collider => collider.GetComponent<Interactable>() != null);

        float minDistance = Mathf.Infinity;
        foreach(Collider2D collider in interactables)
        {
            float tempDistance = Vector2.Distance(transform.position, collider.transform.position);
            if (tempDistance < minDistance)
            {
                Interactable tempInteractable = collider.GetComponent<Interactable>();
                if (tempInteractable.GetType() == typeof(Vehicle) && ((Vehicle)tempInteractable).GetIsRiding())
                    continue;


                minDistance = tempDistance;
                nearestInteractable = tempInteractable;
            }
        }
    }

    // draws a circle to display the interact range
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.limeGreen;
        Handles.DrawWireDisc(transform.position, Vector3.forward, interactRange);
    }
}
