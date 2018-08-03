using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts2;

public class ConstructionController : MonoBehaviour
{
    public bool hasBuilder = false;
    public float buildTime = 10;

    public GameObject building;

    [SerializeField] GameObject builder;

    [SerializeField] private Slider timeSlider;

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

    private Bounds boundingBox;
    public Bounds BoundingBox
    {
        get
        {
            return boundingBox;
        }

        set
        {
            boundingBox = value;
        }
    }

    private bool buildStatus = false;

    private GameObject lastBuilding;

    private Vector3 sliderOffset = new Vector3(0, 3, 0);

    public void CustomStart()
    {
        timeSlider = GetComponentInChildren<Slider>();

        lastBuilding = Instantiate(building, transform.position, transform.rotation);

        buildTime = lastBuilding.GetComponent<BuildingStandard>().main.UpgradeTime;
        timeSlider.maxValue = buildTime;
        boundingBox = lastBuilding.GetComponent<Collider>().bounds;

        lastBuilding.SetActive(false);

        Placed = true;
    }

    void Update()
    {
        FindnSetBuilders();

        timeSlider.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + sliderOffset);
    }

    private void FindnSetBuilders()
    {
        if (Placed && !buildStatus) //check "hasBuilder" here for more optimization 
        {
            foreach (var collider in Physics.OverlapSphere(transform.position, 7))
            {
                if (collider.tag == "Builder")
                {
                    if (collider.gameObject.GetComponent<Builder>().Building == gameObject)
                    {
                        collider.gameObject.GetComponent<Builder>().GoWork(gameObject);
                        builder = collider.gameObject;
                        BuilderStartBuilding();
                        hasBuilder = true;
                        break;
                    }
                }
            }

        }
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

    public void BuilderStartBuilding()
    {
        if (!buildStatus)
        {
            InvokeRepeating("Construct", 0f, 0.1f);
            buildStatus = true;
        }
    }

}
