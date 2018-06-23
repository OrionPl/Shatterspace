using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts2;

public class ConstructionController : MonoBehaviour
{

    public float buildTime = 10;

    public GameObject building;
    private GameObject lastBuilding;
    [SerializeField] GameObject builder;

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

    private bool placed;
    public bool Placed
    {
        get
        {
            return placed;
        }

        set
        {
            placed = value;
        }
    }

    [SerializeField] private Slider timeSlider;

    private Vector3 sliderOffset = new Vector3(0, 3, 0);

    public void CustomStart()
    {
        timeSlider = GetComponentInChildren<Slider>();

        lastBuilding = Instantiate(building, transform.position, Quaternion.identity);

        buildTime = lastBuilding.GetComponent<BuildingStandard>().main.UpgradeTime;
        timeSlider.maxValue = buildTime;

        lastBuilding.SetActive(false);

        Placed = true;
    }

    void Update()
    {
        if (!hasBuilder && Placed) //check "hasBuilder" here for more optimization 
        {
            foreach (var collider in Physics.OverlapSphere(transform.position, 2))
            {
                if (collider.tag == "Builder" && !hasBuilder)
                {
                    builder = collider.gameObject;
                    collider.gameObject.GetComponent<Builder>().GoWork(gameObject);
                    BuilderStartBuilding();
                    hasBuilder = true;
                    break;
                }
            }

        }

        timeSlider.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + sliderOffset);
    }

    void Construct()
    {
        if (buildTime > 0f)
        {
            buildTime -= 0.1f;
            timeSlider.value += 0.1f;
        }
        else
        {
            lastBuilding.SetActive(true);
            lastBuilding.GetComponent<BuildingStandard>().Team = team;
            lastBuilding.GetComponent<BuildingStandard>().Select(false);
            builder.GetComponent<Builder>().Finish();
            Destroy(gameObject, 0.1f);
        }

    }

    void BuilderStartBuilding()
    {
        InvokeRepeating("Construct", 0f, 0.1f);
    }

}
