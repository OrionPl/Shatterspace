using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts2;

public class BarracksBuilding : MonoBehaviour {

    [SerializeField] private List<GameObject> placeholders; // TODO: remove serialize field.
    
    [SerializeField] private float manSpawnTime;
    [SerializeField] private float defence = 1f;

    [SerializeField] private GameObject manType;
	public GameObject ManType
	{
		set
		{
			manType = value;
		}
	}
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject emptySquad;
    [SerializeField] private GameObject UIButtons;


    
    private bool selected = false;
    private bool working;
    private int spawnedMans;


    private Slider statusBar;

    private BuildingData mainInfo;
    private BuildingStandard secondInfo;

    private void Start()
    {
        Invoke("LateStart", 0.001f);
    }

    void LateStart() {
        secondInfo = GetComponent<BuildingStandard>();
        mainInfo = secondInfo.main;
        statusBar = secondInfo.StatusBar;
        UpdatePlaceholders();
    }

    // Update is called once per frame
    void Update () {
        UIButtons.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

	}

    private void LateUpdate()
    {
        Select(secondInfo.selected);
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

    private void StopWorking()
    {
        working = false;
        statusBar.value = 0;
        statusBar.gameObject.SetActive(false);
    }


    private void SpawnMans(GameObject placeholder, GameObject manager, GameObject squadParent) {
        GameObject spawnedMan = Instantiate(manType, placeholder.transform.position, placeholder.transform.rotation);
        spawnedMan.transform.SetParent(squadParent.transform);
        SquadMemberManager manSettings = spawnedMan.GetComponent<SquadMemberManager>();
        manSettings.SetMyManager(manager);
        manSettings.Team = secondInfo.Team;
        manSettings.Setup(manSpawnTime);
    }

    private void UpdateStatus() {
        if (manSpawnTime <= statusBar.value)
        {
            statusBar.value += 0.01f;
        } else {
            StopWorking();
        }
    }

    public void SpawnSquad()
    {
        Debug.Log("Function status: pass");
        if (selected)
        {
            Debug.Log("Seleceted check: pass");
            if (!working)
            {
                Debug.Log("Working check: pass");
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
                    SpawnMans(placeholder, targetSquad, squadParent);
                }
                targetSquad.GetComponent<SquadManager>().SetSquadTeam(secondInfo.Team);
                statusBar.gameObject.SetActive(true);
                statusBar.maxValue = manSpawnTime;
                InvokeRepeating("UpdateStatus", 0f, 0.01f);
            }
        }
    }


    public void ReinforceSquad()
	{
        if (selected)
		{

        }
    }

    public void Select(bool input)
	{
        selected = true;
        UIButtons.SetActive(input);
    }
}