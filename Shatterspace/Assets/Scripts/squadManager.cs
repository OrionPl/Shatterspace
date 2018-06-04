using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squadManager : MonoBehaviour {

    [SerializeField] private GameObject[] mans;
    [SerializeField] private GameObject[] placeholder;

    // Use this for initialization
    void Start () {
        mans = GameObject.FindGameObjectsWithTag("man"); //find mans

        for (int i = 0; i < mans.Length; i++) //setup every man
        {
            mans[i].GetComponent<goWhereIClick>().setPlaceholder(placeholder[i]); //set default positin
            mans[i].GetComponent<goWhereIClick>().goPosition(); //send him to position
        }

        transform.parent = mans[0].transform.parent; //set parent for following team
    }
	
	// Update is called once per frame
	void Update () {
    }
}
