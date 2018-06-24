using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadMemberManager : MonoBehaviour {


    [SerializeField] private Slider uiHealthBar;


    public SquadManager mySquadManager;

    public float speed; // !!Editing navmesh agents speed does nothing. Edit this from Inspector.


    private Camera cam; //maincamera - scene camera

    private UnityEngine.AI.NavMeshAgent aIController;
    private GameObject placeholder;
    private GameManager GameRuleManager;

    private bool selected;
    private bool living = false;

    private float time;

    [SerializeField] private int team; //will be set by builder
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

    void Start()
    {
        cam = Camera.main;
    }

    // one time run
    public void Setup(float getTime)
    {

        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();
        aIController.speed = speed; //set speed
        GameRuleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();  //find and set main manager
        uiHealthBar.maxValue = getTime;
        time = getTime;
        InvokeRepeating("Spawn", 0f, 0.1f);
    }

    void Spawn() {

        uiHealthBar.value = uiHealthBar.value + 0.1f;
        if (uiHealthBar.value >= time)
        {
            living = true;
            uiHealthBar.gameObject.SetActive(false);
            mySquadManager.GetComponent<SquadManager>().Setup();
            GoPosition();
            CancelInvoke("Spawn");
        }

    }

    //that will called every frame
    void Update()
    {
        if (living)
        {
            FollowMouse();

        }

        uiHealthBar.gameObject.transform.position = cam.WorldToScreenPoint(gameObject.transform.position);

        // Check if we've reached the destination (or near of the destination)
        if (aIController.remainingDistance < 3.0f && living)
        {
            GoPosition();
        }
    }

    private void FollowMouse()
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
    }

    //function for send him to default position
    public void GoPosition() { 
        aIController.destination = placeholder.transform.position;
    }

    //set placeholder and squad manager of this man
    public void SetPlaceholder(GameObject target)
    {
        placeholder = target;
        mySquadManager = target.transform.parent.GetComponent<SquadManager>();
    }

    //set speed of this man
    public void SetMySpeed(float getSpeed) { 
        speed = getSpeed;
        aIController.speed = getSpeed;
    }

    public void SetMyManager(GameObject manager) {
        mySquadManager = manager.GetComponent<SquadManager>();
    }

    public void Select(bool input) {
        selected = input;
    }
}
