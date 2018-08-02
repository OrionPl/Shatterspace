using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private bool multiplayer = false;
    
    void Start() {
        gameObject.tag = "GameManager";

        if (Cursor.lockState != CursorLockMode.Confined)
        {
            //Going directly from Locked to Confined does not work
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    
    void Update() {

    }

    public Quaternion ClampRotationAroundXAxis(Quaternion q, float min, float max)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, min, max);

        q.x = Mathf.Tan(Mathf.Deg2Rad * angleX);

        return q;
    }
}
