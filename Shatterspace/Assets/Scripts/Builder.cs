﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Builder : MonoBehaviour {

    private GameObject movementTarget;

    private GameObject[] constructions;
    private GameObject[] builders;
    private NavMeshAgent agent;
    private GameObject nearestBuilder;
    private GameObject constructionSite;

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

    private int team;
    public int Team
    {
        get
        {
            return team;
        }

        set
        {
            team = value;
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
            float minimumDistance = 99999999999f; //99999999999 as placeholder
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
        ConstructionController targetController = getTarget.GetComponent<ConstructionController>();
        if (!targetController.hasBuilder)
        {
            agent.SetDestination(targetController.BoundingBox.min);
            movementTarget = getTarget;
            Building = getTarget; //I have used two different variables to prevent "foreach loop" issues
            constructionSite = GameObject.CreatePrimitive(PrimitiveType.Cube);
            constructionSite.GetComponent<Collider>().enabled = false;
            constructionSite.transform.localScale = targetController.BoundingBox.size;
            constructionSite.transform.position = targetController.BoundingBox.center;
            targetController.hasBuilder = true;
        }
    }

    public void Finish() {
        Destroy(constructionSite);
        Building = null;
        movementTarget = null;
    }

}
