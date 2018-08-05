using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarExplorer : MonoBehaviour {

    [SerializeField] private GameObject exploredAreaAbsorber;

    bool explored;

    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateStatus", 0f, 0.8f);
	}
	
	// Update is called once per frame
	void UpdateStatus () {
        explored = false;
        foreach (var collider in Physics.OverlapSphere(transform.position, (GetComponent<CapsuleCollider>().radius * 1.10f)))
        {
            if (collider.gameObject.tag == "FogExplored") {
                explored = true;
                break;
            }
        }

        if (!explored) {
            GameObject tempObjectVariable;
            tempObjectVariable = Instantiate(exploredAreaAbsorber, transform.position, Quaternion.identity);
            tempObjectVariable.GetComponent<CapsuleCollider>().radius = gameObject.GetComponent<CapsuleCollider>().radius;
            tempObjectVariable.GetComponent<CapsuleCollider>().height = gameObject.GetComponent<CapsuleCollider>().height;
            tempObjectVariable.GetComponent<CapsuleCollider>().contactOffset = gameObject.GetComponent<CapsuleCollider>().contactOffset;
        }
    }

    private void OnDestroy()
    {
        GameObject tempObjectVariable;
        tempObjectVariable = Instantiate(exploredAreaAbsorber, transform.position, Quaternion.identity);
        tempObjectVariable.GetComponent<CapsuleCollider>().radius = gameObject.GetComponent<CapsuleCollider>().radius;
        tempObjectVariable.GetComponent<CapsuleCollider>().height = gameObject.GetComponent<CapsuleCollider>().height;
        tempObjectVariable.GetComponent<CapsuleCollider>().contactOffset = gameObject.GetComponent<CapsuleCollider>().contactOffset;
    }
}
