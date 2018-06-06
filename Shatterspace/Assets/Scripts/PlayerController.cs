using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody _rb;

    [SerializeField] private float cameraSpeed = 1;

    [SerializeField] private float maxCamHeight = 100;
    [SerializeField] private float minCamHeight = 20;
    [SerializeField] private float zoomSensitivity = 10;
    [SerializeField] private float horizontalRotationSpeed = 1;
    [SerializeField] private float verticalRotationSpeed = 1;
    [SerializeField] private float minVerticalRotation = 90;
    [SerializeField] private float maxVerticalRotation = 10;

    public Vector3 startingRotation;

    public List<GameObject> squads;

    private GameManager _gameManager;


    private Quaternion camTargetRotation;

	void Start () {
        _rb = GetComponent<Rigidbody>();

        startingRotation = new Vector3(90, 0, 0);

        CheckForSquads();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        camTargetRotation = transform.rotation;
	}
	
	void Update () {
        MoveCamera();
	}

    private void MoveCamera()
    {
        _rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * cameraSpeed;

        float zoomPos = transform.position.y - Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        zoomPos = Mathf.Clamp(zoomPos, minCamHeight, maxCamHeight);
        transform.position = new Vector3(0, zoomPos, 0);

        //int rotHorizontal = 0;
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    rotHorizontal++;
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    rotHorizontal--;
        //}
        //_rb.rotation = Quaternion.AngleAxis(transform.rotation.y + rotHorizontal * horizontalRotationSpeed, Vector3.up);

        int rotVertical = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotVertical++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rotVertical--;
        }

        camTargetRotation *= Quaternion.Euler(-rotVertical * verticalRotationSpeed, 0, 0);

        camTargetRotation = _gameManager.ClampRotationAroundXAxis(camTargetRotation, minVerticalRotation, maxVerticalRotation);

        transform.rotation = camTargetRotation;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.Euler(startingRotation.x, startingRotation.y, startingRotation.z);
        }
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
}
