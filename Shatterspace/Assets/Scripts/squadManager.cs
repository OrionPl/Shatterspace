using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squadManager : MonoBehaviour {

    [SerializeField] private GameObject[] mans;
    [SerializeField] private GameObject[] placeholder;
    [SerializeField] private Camera cam; //maincamera - scene camera

    private UnityEngine.AI.NavMeshAgent aIController;

    // Use this for initialization
    public float squadHP;
    public float armour;

    void Start () {

        //Declare a variable for navmesh componnent
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();


        mans = GameObject.FindGameObjectsWithTag("man"); //find mans

        for (int i = 0; i < mans.Length; i++) //setup every man
        {
            mans[i].GetComponent<goWhereIClick>().SetPlaceholder(placeholder[i]); //set default positin
            mans[i].GetComponent<goWhereIClick>().GoPosition(); //send him to position

        }

    }
	
	void Update () {
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

    }

    public void DealDamage(GameObject entity)
    {

    }

    public void TakeDamage(float dmg)
    {
        squadHP -= dmg / armour;
    }
}
