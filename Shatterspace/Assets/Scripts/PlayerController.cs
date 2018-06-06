using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody _rb;

    [SerializeField] private float cameraSpeed = 1;

    [SerializeField] private float maxCamHeight = 100;
    [SerializeField] private float minCamHeight = 20;
    [SerializeField] private float zoomSensitivity = 10;

    public List<GameObject> squads;

	void Start () {
        _rb = GetComponent<Rigidbody>();

        CheckForSquads();
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
