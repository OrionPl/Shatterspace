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
    private GameObject nearestBuilder;

    private GameObject building;
    public GameObject Building
    {
        get
        {
            return building;
        }

        set
        {
            building = value;
        }
    }

    void Start () {

        agent = GetComponent<NavMeshAgent>();
        movementTarget = null;
    }

    void LateUpdate() {
        if (Building == null)
        {
            FindnSetConstruction();
        }
    }

    private void FindnSetConstruction()
    {
        constructions = GameObject.FindGameObjectsWithTag("Construction");
        builders = GameObject.FindGameObjectsWithTag("Builder");
        if (constructions.Length > 0)
        {
            float minimumDistance = 10000000f; //10000000 as placeholder
            foreach (GameObject target in constructions)
            {

                foreach (GameObject otherBuilder in builders)
                {
                    if (otherBuilder.GetComponent<Builder>().Building == null)
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

            if (movementTarget == null)
            {
                Finish();
            }
            else if (!movementTarget.GetComponent<ConstructionController>().hasBuilder && movementTarget != null)
            {
                if (nearestBuilder == gameObject)
                {
                    GoWork(movementTarget);

                }
            }
            else
            {
                Finish();
            }
        }
    }

    public void GoWork(GameObject getTarget) {
        agent.SetDestination(getTarget.transform.position);
        movementTarget = getTarget;
        Building = getTarget; //I have used two different variables to prevent "foreach loop" issues
        getTarget.GetComponent<ConstructionController>().hasBuilder = true;
    }

    public void Finish() {
        Building = null;
        movementTarget = null;
    }

}
