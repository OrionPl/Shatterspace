using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts2;

public class ConstructionController : MonoBehaviour {

    public float buildTime = 10;

    public GameObject building;
    public GameObject lastBuilding;
    public GameObject builder;

    public bool hasBuilder = false;

    [SerializeField] private int team;
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

    [SerializeField] private Slider timeSlider;

    private Vector3 sliderOffset = new Vector3(0, 3, 0);

    public void CustomStart()
    {
        timeSlider = GetComponentInChildren<Slider>();
        gameObject.tag = "Construction";

        lastBuilding = Instantiate(building, transform.position, Quaternion.identity);

        buildTime = lastBuilding.GetComponent<BuildingStandard>().main.UpgradeTime;
        timeSlider.maxValue = buildTime;

        lastBuilding.SetActive(false);
    }

    void Update()
    {
        if(!hasBuilder) //check "hasBuilder" here for more optimization 
        {
            foreach (var collider in Physics.OverlapSphere(transform.position, 2))
            {
                if (collider.tag == "Builder" && !hasBuilder)
                {
                    hasBuilder = true;
                    builder = collider.gameObject;
                    builder.GetComponent<Builder>().Construct(this);
                }
            }

        }

        timeSlider.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + sliderOffset);
    }

    void Construct() {
        if (buildTime > 0f) {
            buildTime -= 0.1f;
            timeSlider.value += 0.1f;
        }
        else {
            lastBuilding.SetActive(true);
            lastBuilding.GetComponent<BuildingStandard>().Team = team;
            lastBuilding.GetComponent<BuildingStandard>().Select(false);
            builder.GetComponent<Builder>().Finish();
            Destroy(gameObject, 0.1f);
        }

    }

    public void BuilderStartBuilding() {
        InvokeRepeating("Construct", 0f, 0.1f);  //Coroutines not working correctly.
    }
}
