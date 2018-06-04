using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squadManager : MonoBehaviour {

    [SerializeField] private GameObject[] mans;
    [SerializeField] private GameObject[] placeholder;

    public float squadHP;
    public float armour;

    void Start () {
        mans = GameObject.FindGameObjectsWithTag("man"); //find mans

        for (int i = 0; i < mans.Length; i++) //setup every man
        {
            mans[i].GetComponent<goWhereIClick>().setPlaceholder(placeholder[i]); //set default positin
            mans[i].GetComponent<goWhereIClick>().goPosition(); //send him to position
        }

        transform.parent = mans[0].transform.parent; //set parent for following team
    }
	
	void Update () {

    }

    public void DealDamage(GameObject entity)
    {

    }

    public void TakeDamage(float dmg)
    {
        squadHP -= dmg / armour;
    }
}
