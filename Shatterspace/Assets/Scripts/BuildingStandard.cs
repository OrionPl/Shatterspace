using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts2;
using UnityEngine.UI;

public class BuildingStandard : MonoBehaviour {


    public BuildingData main;

    [SerializeField] private Vector3 healthBarOffset;
    [SerializeField] private Vector3 statusBarOffset;

    [SerializeField] private int team; //will be set by builder
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

    private float health;

    private bool selected;
    public bool Selected
    {
        get
        {
            return selected;
        }
    }

    // Use this for initialization
    void Start () {
        health = main.Hp;
        main.HealthBar.maxValue = main.Hp;
        main.HealthBar.value = main.Hp;
    }
	
	// Update is called once per frame
	void Update () {
        main.HealthBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + healthBarOffset);
        main.StatusBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + statusBarOffset);
    }

    public void Select(bool input) {
        main.HealthBar.gameObject.SetActive(input);
        selected = input;
    }
}
