using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMemberManager : MonoBehaviour {

    [SerializeField] private int team = 0; //it will be choosen when this man spawned, by squadManager script.  TODO: remove serialize field


    public squadManager mySquadManager;

    public float speed; // !!Editing navmesh agents speed does nothing. Edit this from Inspector.
    private Camera cam; //maincamera - scene camera

    private UnityEngine.AI.NavMeshAgent aIController;
    private GameObject placeholder;
    private GameManager GameRuleManager;

    private bool selected;


    void Start()
    {
        Invoke("LateStart", 0.001f); // TODO: It's temp fix for "One man  bug".
    }

    // one time run
    void LateStart()
    {
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();
        aIController.speed = speed; //set speed
        GameRuleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();  //find and set main manager
        cam = Camera.main;  
    }

    //that will called every frame
    void Update()
    {
        if (mySquadManager.GetSquadTeam() == team && selected)
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
        mySquadManager = target.transform.parent.GetComponent<squadManager>();
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

    public void SetMyManager(GameObject manager) {
        mySquadManager = manager.GetComponent<squadManager>();
    }

    public void Select(bool input) {
        selected = input;
    }
}
