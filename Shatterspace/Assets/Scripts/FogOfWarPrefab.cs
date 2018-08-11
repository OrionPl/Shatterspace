using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapSettings;

public class FogOfWarPrefab : MonoBehaviour
{
    [Header("Only edit collider if you want to change radius, scrit will do all job for you.")]
    [SerializeField]private float lightColliderAngleRatio = 8;
    private Light explorerLight;
    private GameObject gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        explorerLight = GetComponentInChildren<Light>();

        MapOptions opts;
        opts = gameManager.GetComponent<MapOptions>();

        if (opts.MapData.ExplorerType != ExploringType.Light)
        {
            explorerLight.enabled = false;
        } else {
            explorerLight.spotAngle = GetComponent<CapsuleCollider>().radius * lightColliderAngleRatio;
        }

        this.enabled = false; //harakiri
    }
}