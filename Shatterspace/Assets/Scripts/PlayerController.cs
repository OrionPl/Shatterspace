﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts2;

public class PlayerController : MonoBehaviour
{

    private GameManager _gameManager;

    [SerializeField] private float cameraSpeed = 1;

    [SerializeField] private float maxCamHeight = 100;
    [SerializeField] private float minCamHeight = 20;
    [SerializeField] private float zoomSensitivity = 10;
    [SerializeField] private float horizontalRotationSpeed = 1;
    [SerializeField] private float verticalRotationSpeed = 1;
    [SerializeField] private float minVerticalRotation = 90;
    [SerializeField] private float maxVerticalRotation = 10;

    [SerializeField] public GameObject construction;

    [SerializeField] private GameObject[] buildingPrefabs;

    [SerializeField] private Vector3 maxPlaceAngle = new Vector3(90f, 0f, 90f);
    [SerializeField] private Vector3 minPlaceAngle = new Vector3(-90f, 0f, -90f);

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private LayerMask absorberCheckMask;

    [Header("0-3, 0 for Hack., 1 for SysA., 2 for Swarm,  3 for GCDI ")]
    public int teamID = 0;

    public List<GameObject> squads;
    private List<GameObject> selection;

    private GameObject newConstruction;

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

    void Update()
    {

        //TODO: Make player can select and use only mans from his\her own team.

        CheckClick();
        MoveCameraCheck();
        RotateCamera();
    }

    private void CheckClick()
    {
        // if we click anywhere on screen with right mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // send ray
            RaycastHit hit;
            Ray clickRay = cam.ScreenPointToRay(Input.mousePosition);

            // if raycast hit  to an object
            if (Physics.Raycast(clickRay, out hit, Mathf.Infinity, raycastMask))
            {
                if (hit.collider.transform.tag == "Building") //check for tags
                {
                    BuildingStandard tempBuild;
                    tempBuild = hit.collider.gameObject.GetComponent<BuildingStandard>();

                    foreach (var choosen in selection)
                    {  //clear selections if building type is different or there is no building on selection before
                        if (choosen.gameObject.tag != "Building")
                        {
                            CleanSelection();
                            break;
                        }
                        else if (choosen.GetComponent<BuildingStandard>().main.Type != tempBuild.main.Type)
                        {
                            CleanSelection();
                            break;
                        }
                    }

                    if (tempBuild.Team == teamID)
                    { //if its in my team and constructed
                        tempBuild.Select(true); //set barrack seleceted
                        selection.Add(tempBuild.gameObject); //add barracks to selection
                    }
                }
                else if (hit.collider.transform.tag == "Squad")
                {
                    SquadManager tempSquad = hit.collider.gameObject.GetComponent<SquadManager>();
                    if (tempSquad.Team == teamID) //if its ally, make squad controllable
                    {
                        selection.Add(tempSquad.gameObject);
                        tempSquad.Select(true);
                    }
                    else
                    {
                        Attack(tempSquad.gameObject); //attack him, he is an enemy
                    }
                }
                else
                {
                    CleanSelection();
                }
            }
        }
    }

    private void MoveCameraCheck()
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

        if(moveVertical != 0 || moveHorizontal != 0) {
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * cameraSpeed;
            MoveCameraHorizontal(moveHorizontal);
            MoveCameraVertical(moveVertical);
        }
    }

    private void RotateCamera()
    {
        float zoomPos = transform.position.y + Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        zoomPos = Mathf.Clamp(zoomPos, minCamHeight, maxCamHeight);
        transform.position = new Vector3(transform.position.x, zoomPos, transform.position.z);


        //TODO : Why always updating rotate things? Just create function and only update when key pressed.
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

    private bool CheckRotation(Vector3 limMax, Vector3 limMin, Quaternion rot)
    {
        if ((limMax.x > rot.x * 360f) && (limMax.z > rot.z * 360f))
        {
            if ((rot.x * 360f > limMin.x) && (rot.z * 360f > limMin.z))
            {
                return true;
            }
        }
        return false;
    }

    private void Attack(GameObject target)
    {

    }

    private void CleanSelection()
    {
        foreach (var something in selection)
        {
            if (something.GetComponent<SquadManager>() != null)
            {
                something.GetComponent<SquadManager>().Select(false);
            }
            else if (something.tag == "Building")
            {
                something.GetComponent<BuildingStandard>().Select(false);
            }

        }
        selection.Clear();
    }


    public void MoveCameraHorizontal(float power)
    {
        transform.parent.position += transform.parent.right * power * cameraSpeed;
    }

    public void MoveCameraVertical(float power)
    {
        transform.parent.position += transform.parent.forward * power * cameraSpeed;
    }

    public void MoveCamera() {

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

    public void BuildingPlacement(GameObject building) // TODO !IMPORTANT BUG  you can place builds two or more times if you click "Place Barrack". So check before player placing something.
    {
        Vector3 newBuildingPosition = new Vector3(0, 0, 0);
        newConstruction = Instantiate(construction, newBuildingPosition, Quaternion.identity);

        newConstruction.GetComponent<ConstructionController>().building = building;

        newConstruction.GetComponent<ConstructionController>().enabled = false;

        StartCoroutine("NewBuildingPositionSelection");
    }

    IEnumerator NewBuildingPositionSelection()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            yield return new WaitForEndOfFrame();

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask))
            {
                newConstruction.transform.position = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
                newConstruction.transform.up = hit.normal; //terrain-ready

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if ((CheckRotation(maxPlaceAngle, minPlaceAngle, newConstruction.transform.rotation)))
                    {
                        bool placeable = true;
                        foreach (var collider in Physics.OverlapSphere(newConstruction.transform.position, 4, raycastMask))
                        {
                            if (collider.gameObject.tag == "Builder" || hit.collider.gameObject == collider.gameObject)
                            {
                                placeable = true;
                            }
                            else
                            {
                                placeable = false;
                                break;
                            }
                        }

                        RaycastHit absorberCheckHit;
                        Ray absorberCheckRay = cam.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(absorberCheckRay, out absorberCheckHit, Mathf.Infinity, absorberCheckMask) && placeable)
                        {
                            placeable = true;
                        }
                        else if(placeable){
                            placeable = false;
                        }

                        if (placeable)
                        {
                            newConstruction.GetComponent<ConstructionController>().enabled = true;
                            newConstruction.GetComponent<ConstructionController>().CustomStart();
                            newConstruction = null;
                            break;
                        }
                    }

                }
            }
        }
    }

    public void BarrackPlacement()
    {
        BuildingPlacement(buildingPrefabs[0]);
    }
}