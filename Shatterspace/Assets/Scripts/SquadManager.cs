using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts2;

public class SquadManager : MonoBehaviour {

    [SerializeField] private int team; //will be set on spawn by barracks at this script's SetSquadTeam() func.
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

    [SerializeField] private List<GameObject> squadMembers;
    public List<GameObject> SquadMembers
    {
        get
        {
            return squadMembers;
        }

        set
        {
            squadMembers = value;
        }
    }

    [SerializeField] private List<GameObject> placeholders;
    public List<GameObject> Placeholders
    {
        get
        {
            return placeholders;
        }

        set
        {
            placeholders = value;
        }
    }

    [SerializeField] private SquadData squadInfo;
    public SquadData SquadInfo
    {
        get
        {
            return squadInfo;
        }

        set
        {
            squadInfo = value;
        }
    }

    [SerializeField] private LayerMask raycastMask;

    private int manCount;
    public int ManCount
    {
        get
        {
            return manCount;
        }

        set
        {
            manCount = value;
        }
    }

    private bool alive = false;
    public bool Alive
    {
        get
        {
            return alive;
        }

        set
        {
            alive = value;
        }
    }

    private float squadHP;
    private float armour;
    private float speedMultiplier = 1; //don't change if you are not testing anything.
    private float squadSpeed;

    private Camera cam; //maincamera - scene camera
    private UnityEngine.AI.NavMeshAgent aIController;
    private GameManager _gameManager;

    private bool selected = false;

    public void Setup(){
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();  //find and set main manager
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();
        cam = Camera.main;

        UpdatePlaceholders();
        UpdateSquadMembers();

        squadHP = SquadInfo.UnitHp;
        armour = SquadInfo.UnitArmor;
    }

    public void UpdateSquadMembers() //updates members and sets up manager
    {

        SquadMembers.Clear();

        GameObject squadParent = null;
        float squadSpeedTemp = 100000f; //declare a temp speed

        foreach (var go in GetComponentsInChildren<Transform>())
        {
            if (go.name == "SquadMembers")
            {
                squadParent = go.gameObject;
            }

        }

        foreach (var member in squadParent.GetComponentsInChildren<Transform>())
        {
            if (member.GetComponent<SquadMemberManager>() != null)
                SquadMembers.Add(member.gameObject);
        }

        int i = 0;
        foreach (var member in SquadMembers) //search for slowest man in squaad and setup members
        {
            SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();

            if (memberManager.speed < squadSpeedTemp) //is it slower than others?
            {
                squadSpeedTemp = memberManager.speed;
            }
            memberManager.SetPlaceholder(Placeholders[i]); //set default positin
            memberManager.GoPosition(); //send him to position 
            memberManager.Team = Team;
            memberManager.SetMyManager(gameObject); //set his manager

            i++;
        }

        SetSquadSpeed(squadSpeedTemp); //set everyones speed to slowest man in squad
        SetSquadTeam(Team);
        manCount = squadMembers.Count;
        Alive = true;
    }

    private void UpdatePlaceholders()
    {
        Placeholders.Clear();

        List<GameObject> placeholdersTemp = new List<GameObject>();
        placeholdersTemp.Clear();
        
        foreach (var go in GetComponentsInChildren<Transform>())
        {
            if (go.gameObject.name == "Placeholders")
            {
                foreach (var placeholder in go.GetComponentsInChildren<Transform>())
                {
                    if (placeholder.name != "Placeholders")
                        placeholdersTemp.Add(placeholder.gameObject);
                }
                break;
            }
        }

        foreach (var placeholder in placeholdersTemp)
        {
            Placeholders.Add(placeholder.gameObject);
        }
    }

    void Update () {
     if (selected && Alive)
        {
            if (Input.GetMouseButtonDown(1))
            {
                // send ray
                RaycastHit hit;
                Ray clickRay = cam.ScreenPointToRay(Input.mousePosition);

                // if raycast hit  to an object
                if (Physics.Raycast(clickRay, out hit, raycastMask))
                {
                    // set hit.point as target
                    aIController.destination = hit.point;
                }
            }
        }
    }

    public void DealDamage(GameObject entity)
    {
    
    }

    public void TakeDamage(float dmg)
    {
        squadHP -= (dmg / armour) * SquadInfo.BaseDamage;
    }

    public void SetSquadSpeed(float setSpeed) { //sets squad speed, public function for traps, potions etc.

        setSpeed = setSpeed * speedMultiplier;

        foreach (var member in SquadMembers) //set everyones speed to given speed
        {
             SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();
             memberManager.SetMySpeed(setSpeed);
        }

        //set speed of squad manager
        aIController.speed = setSpeed * 2; //placeholders will go faster than mans for prevent bugs, to achive this its multipled with 2

    }

    //team will be set by player on spawn
    public void SetSquadTeam(int getTeam) {
        Team = getTeam; //set squad's team

        foreach (var member in SquadMembers) //set everyones team
        {
            SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();
            memberManager.Team = Team;
        }
    }

    //select or deselect
    public void Select(bool input)
    {
        foreach (var member in SquadMembers) //set up selection for members
        {
            SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();
            memberManager.Select(input);
        }
        selected = input;
    }

}
