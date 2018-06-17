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

    public bool builderBuilding = false;
    public bool hasBuilder = false;

    public float startedBuilding;

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

    void Start()
    {
        timeSlider = GetComponentInChildren<Slider>();
        gameObject.tag = "Construction";
        timeSlider.maxValue = buildTime;
        StartCoroutine("Construct");
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

    IEnumerator Construct()
    {

        lastBuilding = Instantiate(building, transform.position, Quaternion.identity);
        buildTime = lastBuilding.GetComponent<BuildingStandard>().main.UpgradeTime;
        lastBuilding.SetActive(false);
        

        while (buildTime > 0)
        {
            if (builderBuilding)
            {
                buildTime -= 0.1f;
                timeSlider.value += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }

        lastBuilding.SetActive(true);
        lastBuilding.GetComponent<BuildingStandard>().Team = team;
        lastBuilding.GetComponent<BuildingStandard>().Select(false);
        builder.GetComponent<Builder>().Finish();
        Destroy(gameObject, 0.1f);
    }
}
