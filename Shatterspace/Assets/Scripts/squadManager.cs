using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squadManager : MonoBehaviour {

    [SerializeField] private List<GameObject> squadMembers;
    [SerializeField] private List<GameObject> placeholders;

    [Header("Set it manually for now")] // TODO: Remove serialize field.
    [SerializeField] private int team; //will be set on spawn by PlayerController script at this script's SetSquadTeam() func.

    private Camera cam; //maincamera - scene camera
    private UnityEngine.AI.NavMeshAgent aIController;

    public float squadHP;
    public float armour;
    public float speedMultiplier = 1; //don't change if you are not testing anything.
    private float squadSpeed;

    private GameRuleManager _GameRuleManager;




    void Start() {
        Invoke("LateStart", 0.001f); // TODO: It's temp fix for "One man  bug".
    }

    void LateStart(){
        _GameRuleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameRuleManager>();  //find and set main manager
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();
        cam = Camera.main;
        UpdatePlaceholders();
        UpdateSquadMembers();
    }

    private void UpdateSquadMembers() //updates members and sets up manager
    {

        squadMembers.Clear();

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
                squadMembers.Add(member.gameObject);
        }

        int i = 0;
        foreach (var member in squadMembers) //search for slowest man in squaad and setup members
        {
            SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();

            if (memberManager.speed < squadSpeedTemp) //is it slower than others?
            {
                squadSpeedTemp = memberManager.speed;
            }
            memberManager.SetPlaceholder(placeholders[i]); //set default positin
            memberManager.GoPosition(); //send him to position 
            memberManager.SetTeam(team); // TODO: remove it later FOR TESTING. Use SetSquadTeam() from player when squad spawned

            i++;
        }

        SetSquadSpeed(squadSpeedTemp); //set everyones speed to slowest man in squad
    }

    private void UpdatePlaceholders()
    {
        placeholders.Clear();

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
            placeholders.Add(placeholder.gameObject);
        }
    }

    void Update () {

        //TODO: Make player can select and use only mans from his\her own team.

        if (_GameRuleManager.GetTeam() == team)
        {
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

    public void DealDamage(GameObject entity)
    {
    
    }

    public void TakeDamage(float dmg)
    {
        squadHP -= dmg / armour;
    }

    public void SetSquadSpeed(float setSpeed) { //sets squad speed, public function for traps, potions etc.

        setSpeed = setSpeed * speedMultiplier;

        foreach (var member in squadMembers) //set everyones speed to given speed
        {
             SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();
             memberManager.SetMySpeed(setSpeed);
        }

        //set speed of squad manager
        aIController.speed = setSpeed * 2; //placeholders will go faster than mans for prevent bugs, to achive this its multipled with 2

    }

    //team will be set by player on spawn
    public void SetSquadTeam(int getTeam) {
        team = getTeam; //set squad's team

        foreach (var member in squadMembers) //set everyones team
        {
            SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();
            memberManager.SetTeam(team);
        }
    }

}
