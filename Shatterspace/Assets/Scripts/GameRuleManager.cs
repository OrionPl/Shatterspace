using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour {

    [Header("After some code player will be able to change team from UI")]
    [Header("0-3, 0 for Hack., 1 for SysA., 2 for Swarm,  3 for GCDI ")]
    [SerializeField] private int team;
    [SerializeField] private bool multiplayer = false;

    private GameObject player;

    // Use this for initialization
    void Start() {
        if (!multiplayer) {
            player = GameObject.Find("Player");

        }
        else {
            //check if its local with a loop.
            player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update() {

    }


    public int GetTeam(){
        return team;
    }

    public Quaternion ClampRotationAroundXAxis(Quaternion q, float min, float max)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;
        Debug.Log(q);
        float angleX = Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, min, max);

        q.x = Mathf.Tan(Mathf.Deg2Rad * angleX);

        return q;
    }
}
