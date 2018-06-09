using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private GameManager _gameManager;

    [SerializeField] private float cameraSpeed = 1;

    [SerializeField] private float maxCamHeight = 100;
    [SerializeField] private float minCamHeight = 20;
    [SerializeField] private float zoomSensitivity = 10;
    [SerializeField] private float horizontalRotationSpeed = 1;
    [SerializeField] private float verticalRotationSpeed = 1;
    [SerializeField] private float minVerticalRotation = 90;
    [SerializeField] private float maxVerticalRotation = 10;

    [SerializeField] private GameObject[] buildingPrefabs;

    [Header("0-3, 0 for Hack., 1 for SysA., 2 for Swarm,  3 for GCDI ")]
    public int teamID = 0;

    public List<GameObject> squads;

    private List<GameObject> selection;
    private Camera cam;

    private Rigidbody _rb;
    private Rigidbody _parentRb;

    private Quaternion startingCamRotation;
    private Quaternion startingPlayerRotation;
    private Quaternion camTargetRotation;
    private Quaternion playerTargetRotation;

    void Start()
    {
        Invoke("LateStart", 0.001f); // TODO: It's temp fix for "One man  bug".
    }

    void LateStart()
    {
        selection = new List<GameObject>();
        _rb = GetComponent<Rigidbody>();
        _parentRb = GetComponentInParent<Rigidbody>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        cam = Camera.main;

        startingCamRotation = transform.rotation;
        startingPlayerRotation = transform.parent.rotation;
        
        camTargetRotation = transform.rotation;
        playerTargetRotation = transform.parent.rotation;

        CheckForSquads();
    }

    void Update () {

        //TODO: Make player can select and use only mans from his\her own team.

        CheckClick();
        MoveCamera();
        RotateCamera();
	}

    private void CheckClick() {
        // if we click anywhere on screen with right mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // send ray
            RaycastHit hit;
            Ray clickRay = cam.ScreenPointToRay(Input.mousePosition);

            // if raycast hit  to an object
            if (Physics.Raycast(clickRay, out hit))
            {
                if (hit.collider.transform.tag == "barrack") //check for tags
                {
                    OpenBarrackUI();
                    selection.Clear();
                    hit.collider.gameObject.GetComponent<BarracksBuilding>().Select(true);
                    selection.Add(hit.collider.gameObject);
                }
                else if (hit.collider.transform.tag == "Squad")
                {
                    squadManager tempSquad = hit.collider.gameObject.GetComponent<squadManager>();
                    if (tempSquad.GetSquadTeam() == teamID) //if its ally, make squad controllable
                    {
                        selection.Add(tempSquad.gameObject);
                        tempSquad.Select(true);
                    }
                    else
                    {
                        Attack(tempSquad.gameObject); //attack him, he is an enemy
                    }
                }
                else {
                    CleanSelection();
                }
            }
        }
    }

    private void MoveCamera()
    {
        int moveHorizontal = 0;
        int moveVertical = 0;

        if (Input.GetKey(KeyCode.A))
            moveHorizontal--;
        if (Input.GetKey(KeyCode.D))
            moveHorizontal++;
        if (Input.GetKey(KeyCode.W))
            moveVertical++;
        if (Input.GetKey(KeyCode.S))
            moveVertical--;

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * cameraSpeed;
        
        transform.parent.position += transform.parent.forward * moveVertical * cameraSpeed;
        transform.parent.position += transform.parent.right * moveHorizontal * cameraSpeed;
    }

    private void RotateCamera()
    {
        float zoomPos = transform.position.y + Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        zoomPos = Mathf.Clamp(zoomPos, minCamHeight, maxCamHeight);
        transform.position = new Vector3(transform.position.x, zoomPos, transform.position.z);
        
        int rotHorizontal = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotHorizontal++;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotHorizontal--;
        }

        int rotVertical = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotVertical++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rotVertical--;
        }

        playerTargetRotation *= Quaternion.Euler(0, -rotHorizontal * horizontalRotationSpeed, 0);

        camTargetRotation *= Quaternion.Euler(-rotVertical * verticalRotationSpeed, 0, 0);

        camTargetRotation = _gameManager.ClampRotationAroundXAxis(camTargetRotation, minVerticalRotation, maxVerticalRotation);



        if (Input.GetKeyDown(KeyCode.R))
        {
            camTargetRotation = startingCamRotation;
            playerTargetRotation = startingPlayerRotation;
        }

        transform.localRotation = camTargetRotation;
        transform.parent.rotation = playerTargetRotation;
    }

    void Attack(GameObject target) {

    }

    void CleanSelection() {
        foreach (var something in selection)
        {
            if (something.GetComponent<squadManager>() != null) {
                something.GetComponent<squadManager>().Select(false);
            }else if(something.GetComponent<BarracksBuilding>() != null) {
                something.GetComponent<BarracksBuilding>().Select(false);
            }
                
        }
        selection.Clear();
    }

    public void CheckForSquads()
    {
        squads.Clear();
        foreach (var squad in GetComponentsInChildren<Transform>())
        {
            if (squad.tag == "Squad")
                squads.Add(squad.gameObject);
        }
    }
    
    public void SetTeam(int newTeam)
    { 
        teamID = newTeam;
    }

    public void OpenBarrackUI()
    {
        
    }

    public void BuildingPlacement(GameObject building)
    {
        Vector3 newBuildingPosition = new Vector3(0, 0, 0);
        newBuilding = Instantiate(building, newBuildingPosition, Quaternion.identity);
        StartCoroutine("NewBuildingPositionSelection");
    }

    private GameObject newBuilding;

    IEnumerator NewBuildingPositionSelection()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            yield return new WaitForEndOfFrame();

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                newBuilding.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if(newBuilding.tag == "barrack")
                    newBuilding.GetComponent<BarracksBuilding>().StartConst();

                newBuilding = null;
                break;
            }
        }
    }

    public void BarrackPlacement()
    {
        BuildingPlacement(buildingPrefabs[0]);
    }
}
