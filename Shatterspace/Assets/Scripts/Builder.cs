using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Builder : MonoBehaviour {

    private GameObject movementTarget;

    public int team;

    private GameObject[] constructions;
    private GameObject[] builders;
    private NavMeshAgent agent;
    private GameObject building;
    private GameObject nearestBuilder;
    void Start () {

        agent = GetComponent<NavMeshAgent>();
        movementTarget = null;
    }

    void LateUpdate() {
        if (building == null)
        {
            constructions = GameObject.FindGameObjectsWithTag("Construction");
            builders = GameObject.FindGameObjectsWithTag("Builder");

            Debug.Log("Checkpoint");
            if (constructions.Length > 0)
            {
                Debug.Log("Checkpoint");
                float minimumDistance = 10000000f; //10000000 as placeholder
                foreach (GameObject target in constructions)
                {

                    foreach (GameObject otherBuilder in builders)
                    {
                        if (otherBuilder.GetComponent<Builder>().building == null)
                        {
                            float distance = Vector3.Distance(otherBuilder.transform.position, target.transform.position);
                            ConstructionController targetInfo = target.GetComponent<ConstructionController>();
                            if (!targetInfo.hasBuilder && minimumDistance > distance && targetInfo.Placed)
                            {
                                minimumDistance = distance;
                                movementTarget = target;
                                nearestBuilder = otherBuilder;
                            }
                        }
                    }
                }

                if (!movementTarget.GetComponent<ConstructionController>().hasBuilder && movementTarget != null)
                {
                    if (nearestBuilder == gameObject) {
                        GoWork(movementTarget);
                        movementTarget.GetComponent<ConstructionController>().hasBuilder = true;
                    }
                }
                else
                {
                    Finish();
                }
            }
        }
	}

    public void GoWork(GameObject getTarget) {
        agent.SetDestination(getTarget.transform.position);
        movementTarget = getTarget;
        building = getTarget; //I have used two different variables to prevent "foreach loop" issues
    }

    public void Finish() {
        building = null;
        movementTarget = null;
    }

}
