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
        CancelInvoke("UpdateStatus");
        foreach (var placeholder in placeholders) {
            placeholder.GetComponent<PlaceHolderInfo>().Empty = true;
        }
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
        if (selected)
        {
            if (!working)
            {
                statusBar.gameObject.SetActive(true);
                statusBar.maxValue = manSpawnTime;
                InvokeRepeating("UpdateStatus", 0f, 0.01f);
                working = true;
                GameObject targetSquad = Instantiate(emptySquad, spawnPoint.transform.position, spawnPoint.transform.rotation);
                manSpawnTime = emptySquad.GetComponent<SquadManager>().SquadInfo.SpawnTime;
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
                    placeholder.GetComponent<PlaceHolderInfo>().Empty = false;
                }
                targetSquad.GetComponent<SquadManager>().SetSquadTeam(secondInfo.Team);
            }
        }
    }


    public void ReinforceSquad()
	{
        if (selected)
		{
            Debug.Log("Check point 1");
            if (!working)
            { 
                Debug.Log("Check point 2");
                GameObject squadCollider = null;
                foreach (var collider in Physics.OverlapSphere(transform.position, 4))
                {
                    if (collider.tag == "Squad")
                    {
                        Debug.Log("Check point 2.25");
                        squadCollider = collider.gameObject;
                    }
                    else if (collider.tag == "man")
                    {
                        Debug.Log("Check point 2.50");
                        squadCollider = collider.gameObject.GetComponent<SquadMemberManager>().mySquadManager.gameObject;
                    }
                }
                if (squadCollider != null)
                {
                    Debug.Log("Check point 3");
                    Debug.Log(squadCollider);
                    SquadManager _squadInfo = squadCollider.GetComponent<SquadManager>();
                    if (_squadInfo.Team == secondInfo.Team && _squadInfo.SquadMembers.Count < _squadInfo.SquadInfo.MaxCount)
                    {
                        List<GameObject> _placeholders = _squadInfo.Placeholders;
                        GameObject _squadParent = null;
                        GameObject _emptyPlaceholder = null;
                        statusBar.gameObject.SetActive(true);
                        manSpawnTime = _squadInfo.SquadInfo.ReinforceTime;
                        statusBar.maxValue = manSpawnTime;
                        foreach (GameObject _tempEmptyPlaceholder in placeholders)
                        {
                            if (_tempEmptyPlaceholder.GetComponent<PlaceHolderInfo>().Empty)
                            {
                                _emptyPlaceholder = _tempEmptyPlaceholder;
                                break;
                            }
                        }
                        foreach (var go in squadCollider.GetComponentsInChildren<Transform>())
                        {
                            if (go.name == "SquadMembers")
                            {
                                _squadParent = go.gameObject;
                            }

                        }
                        if (_emptyPlaceholder != null) {
                            SpawnMans(_emptyPlaceholder, squadCollider, _squadParent);
                            InvokeRepeating("UpdateStatus", 0f, 0.01f);
                            _emptyPlaceholder.GetComponent<PlaceHolderInfo>().Empty = false;
                        }
                    }
                }
            }
        }
    }
    
    public void Select(bool input)
	{
        selected = input;
        UIButtons.SetActive(input);
    }

}