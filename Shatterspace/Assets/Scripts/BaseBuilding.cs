using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts2;

public class BaseBuilding : MonoBehaviour {

    [SerializeField] private GameObject builderPrefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject UIButtons;

    private bool selected = false;
    private bool working;

    private Slider statusBar;

    private BuildingData mainInfo;
    private BuildingStandard secondInfo;

    // Use this for initialization
    void Start () {
        secondInfo = GetComponent<BuildingStandard>();
        mainInfo = secondInfo.main;
        statusBar = secondInfo.StatusBar;
    }

    // Update is called once per frame
    void Update()
    {
        UIButtons.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }

    private void LateUpdate()
    {
        Select(secondInfo.selected);
    }

    public void Select(bool input)
    {
        selected = input;
        UIButtons.SetActive(input);
    }

    public void SpawnBuilder() {
        if (!working) {
            GameObject lastSpawned = Instantiate(builderPrefab, spawnPoint.transform.position, Quaternion.identity);
            lastSpawned.GetComponent<Builder>().Team = secondInfo.Team;
            lastSpawned.name = "Builder";
        }
    }


}
