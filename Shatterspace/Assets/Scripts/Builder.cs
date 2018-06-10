using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Builder : MonoBehaviour {

    private GameObject movementTarget;

    private NavMeshAgent agent;

    public int team;

    private bool hasBuilding = false;

	void Start () {
        gameObject.tag = "Builder";
        agent = GetComponent<NavMeshAgent>();
	}

    void Update() {
        if (hasBuilding != true || (movementTarget.GetComponent<ConstructionController>().hasBuilder == true && movementTarget.GetComponent<ConstructionController>().builder != gameObject))
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Construction");
            if (targets.Length >= 1)
            {
                float lengthFromTarget = Vector3.Distance(targets[0].transform.position, transform.position);
                int targetID = -1;

                bool newBuilding = false;

                foreach (var target in targets)
                {
                    targetID++;
                    if (target.GetComponent<ConstructionController>().hasBuilder != true)
                    {
                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance < lengthFromTarget)
                        {
                            lengthFromTarget = distance;
                            newBuilding = true;
                        }
                    }
                }
                if (newBuilding)
                {
                    movementTarget = targets[targetID];
                    agent.SetDestination(movementTarget.transform.position);
                    hasBuilding = true;
                }
                else
                {
                    hasBuilding = false;
                    agent.SetDestination(transform.position);
                }
            }
        }
        
        foreach (var collider in Physics.OverlapSphere(transform.position, 3))
        {
            if (collider.tag == "Construction")
            {
                var cc = collider.GetComponent<ConstructionController>();
                cc.builderBuilding = true;
                cc.hasBuilder = true;
            }
        }
	}
}
