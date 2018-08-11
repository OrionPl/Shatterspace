using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapSettings;

public class MapOptions : MonoBehaviour {

    [SerializeField] MapInfo mapData; //For getting this info from main menu, make an object and make it "Don't destroy while changing scenes". Use it as ship.
    public MapInfo MapData
    {
        get
        {
            return mapData;
        }
    }

    [SerializeField] private GameObject mapLight;

    // Use this for initialization
    void Start () {
        SetExploringType();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetExploringType() { //Use "else if" for future "ExploringTypes"
        if (MapData.ExplorerType == ExploringType.Fog)
        {
            for (int i = 0; i < MapData.FogPositions.Length; i++)
            {
                Instantiate(MapData.FogPrefabs[i], MapData.FogPositions[i], Quaternion.identity);
            }
            mapLight.gameObject.SetActive(true);
        }
        else {

        }
    }

}
