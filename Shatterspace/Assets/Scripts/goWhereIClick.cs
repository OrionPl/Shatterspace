using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goWhereIClick : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent aIController;

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
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // if raycast hit  to an object
            if (Physics.Raycast(clickRay, out hit))
            {
                // set hit.point as target
                aIController.destination = hit.point;
            }
        }
    }

    public void setPlaceholder(GameObject placeholder)
    {

    }

    public void goPosition()
    {

    }
}
