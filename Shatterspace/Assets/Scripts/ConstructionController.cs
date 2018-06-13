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

    public int team;

    public bool builderBuilding = false;
    public bool hasBuilder = false;

    public float startedBuilding;

    [SerializeField] private Faction faction = Faction.None;
    public Faction Faction { get { return faction; } }

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
        foreach (var collider in Physics.OverlapSphere(transform.position, 2))
        {
            if (collider.tag == "Builder")
            {
                hasBuilder = true;
                builder = collider.gameObject;
            }
        }

        timeSlider.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + sliderOffset);
    }

    IEnumerator Construct()
    {
        while (buildTime > 0)
        {
            if (builderBuilding)
            {
                buildTime -= 0.1f;
                timeSlider.value += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }

        lastBuilding = Instantiate(building, transform.position, Quaternion.identity);

        lastBuilding.GetComponent<BuildInfo>().main.Faction = Faction;

        Destroy(gameObject, 0.1f);
    }
}
