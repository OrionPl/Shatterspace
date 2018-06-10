using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksBuilding : MonoBehaviour {

    [SerializeField] private List<GameObject> placeholders; // TODO: remove serialize field.
    
    [SerializeField] private float manSpawnTime;
    [SerializeField] private float defence = 1f;

    [SerializeField] private GameObject manType;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject emptySquad;
    [SerializeField] private GameObject UIButtons;

    [SerializeField] private int team; //will be set by builder
    
    private bool selected = false;
    private bool working;
    private int spawnedMans;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        UIButtons.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
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

    private void StopWorking() {
        working = false;
    }

    public void SpawnSquad() {
        if (selected) {
            if (!working) {
                working = true;
                GameObject targetSquad = Instantiate(emptySquad, spawnPoint.transform.position, spawnPoint.transform.rotation);
                GameObject squadParent = null;
                foreach (var go in targetSquad.GetComponentsInChildren<Transform>())
                {
                    if (go.name == "SquadMembers")
                    {
                        squadParent = go.gameObject;
                    }

                }

                foreach (var placeholder in placeholders)
                {
                    SpawnMans(3, placeholder, targetSquad, squadParent);
                }
                targetSquad.GetComponent<squadManager>().SetSquadTeam(team);
                Invoke("StopWorking", manSpawnTime);
            }
        }
    }

    void SpawnMans(int count, GameObject placeholder, GameObject manager, GameObject squadParent) {
        if (spawnedMans < placeholders.Count && spawnedMans < count) {
            GameObject spawnedMan = Instantiate(manType, placeholder.transform.position, placeholder.transform.rotation);
            spawnedMan.transform.SetParent(squadParent.transform);
            SquadMemberManager manSettings = spawnedMan.GetComponent<SquadMemberManager>();
            manSettings.SetMyManager(manager);
            manSettings.SetTeam(team);
            manSettings.Setup(manSpawnTime);
        }
    }

    public void ReinforceSquad() {
        if (selected) {

        }
    }

    public void SetManType(GameObject man) {
        manType = man;
    }

    public void SetTeam(int getTeam) {
        team = getTeam;
    }

    public int GetTeam() {
        return team;
    }

    public void Select(bool input) {
        selected = input;
        UIButtons.SetActive(input);
    }

    // Use this for initialization
    public void StartConst()
    {
        UpdatePlaceholders();
    }
}