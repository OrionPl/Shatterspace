using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMemberManager : MonoBehaviour {

    private Camera cam; //maincamera - scene camera
    private UnityEngine.AI.NavMeshAgent aIController;
    private GameObject placeholder; 

    public float speed; // !!Editing navmesh agents speed does nothing. Edit this from Inspector.
    public GameObject mySquadManager;

    [SerializeField] private int team = 0; //it will be choosen when this man spawned, by squadManager script.  TODO: remove serialize field

    private GameRuleManager GameRuleManager;

    // one time run
    void Start()
    {
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();
        aIController.speed = speed; //set speed
        GameRuleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameRuleManager>();  //find and set main manager
        cam = Camera.main;
    }

    //that will called every frame
    void Update()
    {
        if(GameRuleManager.GetTeam() == team)
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

    //set placeholder and squad manager of this man
    public void SetPlaceholder(GameObject target)
    {
        placeholder = target;
        mySquadManager = target.transform.parent.transform.parent.gameObject;
    }

    //set speed of this man
    public void SetMySpeed(float getSpeed) { 
        speed = getSpeed;
        aIController.speed = getSpeed;
    }

    //Team will be set by SquadManager
    public void SetTeam(int getTeam)
    { 
        team = getTeam;
    }
}
