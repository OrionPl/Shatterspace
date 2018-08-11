using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapSettings
{
    [System.Serializable, CreateAssetMenu(menuName = "Shatterspace/MapInfo")]
    public class MapInfo : ScriptableObject
    {
        [SerializeField] private string mapName;
        [SerializeField] private int maxPlayers;
        [SerializeField] private ExploringType explorerType;
        [SerializeField] private GameObject[] fogPrefabs;
        [SerializeField] private Vector3[] fogPositions;

        public string MapName { get { return MapName; } }
        public int MaxPlayers { get { return maxPlayers; } }
        public ExploringType ExplorerType { get { return explorerType; } }
        public GameObject[] FogPrefabs { get { return fogPrefabs; } }
        public Vector3[] FogPositions { get { return fogPositions; } }

        //Make an option for gamemode
    }
}
