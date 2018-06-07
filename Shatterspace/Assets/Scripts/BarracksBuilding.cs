using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksBuilding : MonoBehaviour {

    [SerializeField]private List<GameObject> placeholders; // TODO: remove serialize field.


    [SerializeField]private float manSpawnTime;

    [SerializeField]private GameObject manType;

    [SerializeField]private int team; //will be set by builder

	// Use this for initialization
	void Start () {
        UpdatePlaceholders();
    }
	
	// Update is called once per frame
	void Update () {

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

    public void SetManType(GameObject man) {
        manType = man;
    }

    public void SetTeam(int getTeam) {
        team = getTeam;
    }

}
