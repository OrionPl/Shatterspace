using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksBuilding : MonoBehaviour {

    [SerializeField] private List<GameObject> placeholders; // TODO: remove serialize field.

    [SerializeField] private float constructionTime; //will be set by builder
    [SerializeField] private float manSpawnTime;
    [SerializeField] private float defence = 1f;

    [SerializeField] private Vector3 timerOffset;

    [SerializeField] private GameObject manType;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject emptySquad;
    [SerializeField] private GameObject UIButtons;

    [SerializeField] private int team; //will be set by builder

    private Slider uiConstructionTime;

    private bool constructed;
    private bool selected = false;
    private bool working;
    private int spawnedMans;

    private GameObject targetSquad;

    private void Start()
    {
        uiConstructionTime = GetComponentInChildren<Slider>();
        uiConstructionTime.maxValue = constructionTime;
    }

    // Update is called once per frame
    void Update () {

        uiConstructionTime.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + timerOffset);
        UIButtons.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        if (constructionTime <= 0f)
            Destroy(gameObject);

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

    private void StopWorking() {  //get ready for new squad
        working = false;
    }

    private void SetupTimer() { //start a timer
        InvokeRepeating("BuildTimer", 0f, 0.1f);
    }

    private void BuildTimer() { 
        uiConstructionTime.value = uiConstructionTime.value + 0.1f; //change ui bars value
        if (uiConstructionTime.value >= constructionTime) { //if its true
            constructed = true; //barrack is ready
            uiConstructionTime.gameObject.SetActive(false); //invisible health bar
            CancelInvoke("BuildTimer"); //stop building
        }
            
    }

    public void SpawnSquad() {
        if (selected) {
            if (!working) {
                working = true;
                targetSquad = Instantiate(emptySquad, spawnPoint.transform.position, spawnPoint.transform.rotation); //instantiate a squad in spawnpoint
                GameObject squadParent = null;  //declare an empty man parrent for new squad
                foreach (var go in targetSquad.GetComponentsInChildren<Transform>())
                {
                    if (go.name == "SquadMembers")
                    {
                        squadParent = go.gameObject;  //set squad parrent
                    }

                }

                foreach (var placeholder in placeholders)
                {
                    SpawnMans(3, placeholder, targetSquad, squadParent); //spawn 3 man
                }
                targetSquad.GetComponent<squadManager>().SetSquadTeam(team); //set squads team
                
                Invoke("UpdateSquad", manSpawnTime + 0.001f); //Added 0.001f for correction
                Invoke("StopWorking", manSpawnTime + 0.002f);
            }
        }
    }

    void SpawnMans(int count, GameObject placeholder, GameObject manager, GameObject squadParent) { //spawn men
        if (spawnedMans < placeholders.Count && spawnedMans < count) { 
            GameObject spawnedMan = Instantiate(manType, placeholder.transform.position, placeholder.transform.rotation); //spawn a man in placehodler of barrakcs
            spawnedMan.transform.SetParent(squadParent.transform); //set him parrent as empty squad
            SquadMemberManager manSettings = spawnedMan.GetComponent<SquadMemberManager>();
            manSettings.SetMyManager(manager); //sjhow him who is his squad manager
            manSettings.SetTeam(team); //set team of man
            manSettings.Setup(manSpawnTime); //start setting up
        }
    }

    void UpdateSquad() { //update squad when all men spawned
        targetSquad.GetComponent<squadManager>().Setup();
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

    public void GetDamage(float damage) {
        uiConstructionTime.value -= damage/defence;
        constructionTime -= damage/defence;
    }

    public bool GetConstructed() {
        return constructed;
    }

    public int GetTeam() {
        return team;
    }

    public void Select(bool input) {
        selected = input;
        uiConstructionTime.gameObject.SetActive(input);
        UIButtons.SetActive(input);
    }

    // Use this for initialization
    public void StartConst()
    {
        UpdatePlaceholders();
        SetupTimer();
    }

}
