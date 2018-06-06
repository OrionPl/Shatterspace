using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
	void Start () {
		
	}
	
	void Update () {
		
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
