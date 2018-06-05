using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour {

    [SerializeField] private List<GameObject> squadMembers;
    [SerializeField] private List<GameObject> placeholders;
    private Camera cam; //maincamera - scene camera

    private UnityEngine.AI.NavMeshAgent aIController;
    
    public float squadHP;
    public float armour;


    private float squadSpeed;

    void Start () {
        aIController = GetComponent<UnityEngine.AI.NavMeshAgent>();

        cam = Camera.main;

        UpdatePlaceholders();
        UpdateSquadMembers();
    }

    private void UpdateSquadMembers() //updates members and sets up manager
    {

        squadMembers.Clear();

        GameObject squadParent = null;
        float squadSpeedTemp = 100000; //declare a temp speed

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
            i++;
        }

        SetSquadSpeed(squadSpeedTemp);

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

    public void SetSquadSpeed(float setSpeed) { //sets squad speed, public function for traps, potions etc.

        foreach (var member in squadMembers) //set everyones speed to given speed
        {
             SquadMemberManager memberManager = member.GetComponent<SquadMemberManager>();
             memberManager.SetMySpeed(setSpeed);
        }

        //set speed of squad manager
        aIController.speed = setSpeed * 2; //placeholders will go faster than mans for prevent bugs, to achive this its multipled with 2

    }
}
