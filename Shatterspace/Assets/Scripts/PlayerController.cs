using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody _rb;

    [SerializeField]
    private float cameraSpeed = 1;

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
    }

    public void CheckForSquads()
    {
        squads.Clear();
        foreach (var squad in GetComponentsInChildren<GameObject>())
        {
            if (squad.tag == "Squad")
                squads.Add(squad);
        }
    }
}
