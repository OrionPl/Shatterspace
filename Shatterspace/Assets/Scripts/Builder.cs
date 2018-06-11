using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Builder : MonoBehaviour {

    [SerializeField] private GameObject movementTarget;

    private NavMeshAgent agent;

    public int team;

    [SerializeField] private bool hasBuilding = false;

	void Start () {
        gameObject.tag = "Builder";
        agent = GetComponent<NavMeshAgent>();
	}

    void Update() {
        if (movementTarget == null || (movementTarget.GetComponent<ConstructionController>().hasBuilder == true && movementTarget.GetComponent<ConstructionController>().builder != gameObject))
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Construction");
            if (targets.Length >= 1)
            {
                float lengthFromTarget = Vector3.Distance(targets[0].transform.position, transform.position);
                int targetID = 0;
                int id = -1;

                bool newBuilding = false;

                foreach (var target in targets)
                {
                    id++;
                    if (target.GetComponent<ConstructionController>().hasBuilder != true)
                    {
                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance < lengthFromTarget)
                        {
                            lengthFromTarget = distance;
                            newBuilding = true;
                            targetID = id;
                        }
                    }
                }
                if (newBuilding)
                {
                    movementTarget = targets[targetID];
                    targets[targetID].GetComponent<ConstructionController>().builder = gameObject;
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
