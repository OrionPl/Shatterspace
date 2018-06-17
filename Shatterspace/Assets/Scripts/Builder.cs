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
        Finish();
	}

    void Update() {
        if (movementTarget == null)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Construction");
            if (targets.Length >= 1)
            {
                float lengthFromTarget = Vector3.Distance(targets[0].transform.position, transform.position);
                foreach (var target in targets)
                {

                    if (target.GetComponent<ConstructionController>().builder == null)
                    {
                        float distance = Vector3.Distance(target.transform.position, transform.position);

                        if (distance < lengthFromTarget)
                        {
                            lengthFromTarget = distance;
                            StartWorking(target);
                        }
                    }
                }
            }
        }
	}

    void StartWorking(GameObject getTarget) {
            movementTarget = getTarget;
            getTarget.GetComponent<ConstructionController>().builder = gameObject;
            agent.SetDestination(movementTarget.transform.position);
            hasBuilding = true;

    }

    public void Construct(ConstructionController cc) {  //Its more easy to use different func.
        cc.hasBuilder = true;
        cc.BuilderStartBuilding();
        movementTarget = cc.gameObject;
    }

    public void Finish()
    {
        hasBuilding = false;
        agent.SetDestination(transform.position);
        movementTarget = null;
    }
}
