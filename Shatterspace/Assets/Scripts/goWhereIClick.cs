using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goWhereIClick : MonoBehaviour {

    [SerializeField] private Camera cam; //maincamera - scene camera

    private UnityEngine.AI.NavMeshAgent aIController;
    [SerializeField]  private GameObject placeholder; //default position

    // one time run
    void Start()
    {
        //Declare a variable for navmesh componnent
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    //that will called every frame
    void Update()
    {
        // if we click anywhere on screen with right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // send ray
            RaycastHit hit;
            Ray clickRay = cam.ScreenPointToRay(Input.mousePosition);

            // if raycast hit  to an object
            if (Physics.Raycast(clickRay, out hit))
            {
                // set hit.point as target
                aIController.destination = hit.point;
            }
        }

        // Check if we've reached the destination (or near of the destination)
        if (aIController.remainingDistance < 3.0f)
        {
            GoPosition();
                
        }
    }

    //function for send him to default position
    public void GoPosition() {
        aIController.destination = placeholder.transform.position;
    }

    public void SetPlaceholder(GameObject target)
    {
        placeholder = target;
    }

}
