using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour {
    PlayerController playerControlScript;

    [Header("Set UI Area (Select one of them please)")]
    [SerializeField] bool left;
    [SerializeField] bool right;
    [SerializeField] bool up;
    [SerializeField] bool down;

    // Use this for initialization
    void Start () {
        playerControlScript = GameObject.Find("Player").GetComponentInChildren<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartMovement()
    {
        if (left)
        {
            InvokeRepeating("GoLeft", 0f, 0.02f);
        }
        else if (right)
        {
            InvokeRepeating("GoRight", 0f, 0.02f);
        }
        else if (up)
        {
            InvokeRepeating("GoUp", 0f, 0.02f);
        }
        if (down)
        {
            InvokeRepeating("GoDown", 0f, 0.02f);
        }
    }

    public void StopMovement() {
    }

    private void GoLeft()
    {
        playerControlScript.MoveCameraHorizontal(-1f);
    }
    private void GoRight()
    {
        playerControlScript.MoveCameraHorizontal(+1f);
    }
    private void GoUp()
    {
        playerControlScript.MoveCameraVertical(1f);
    }
    private void GoDown()
    {
        playerControlScript.MoveCameraVertical(-1f);
    }
}
