using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts2;
using UnityEngine.UI;

public class BuildingStandard : MonoBehaviour {


    public BuildingData main;

    [SerializeField] private Slider statusBar;
    public Slider StatusBar
    {
        get
        {
            return statusBar;
        }
    }

    [SerializeField] private Slider healthBar;
    public Slider HealthBar
    {
        get
        {
            return healthBar;
        }
    }


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

    public bool selected;

    // Use this for initialization
    void Start () {
        Invoke("LateStart", 0.001f);

    }

    void LateStart()
    {
        health = main.Hp;
        healthBar.maxValue = main.Hp;
        healthBar.value = main.Hp;
        selected = false;
    }

    // Update is called once per frame
    void Update () {
    }

    public void Select(bool input) {
        healthBar.gameObject.SetActive(input);
        selected = input;
    }
}
