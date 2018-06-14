using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts2
{
	[System.Serializable, CreateAssetMenu(menuName = "Shatterspace/Building")]
	public class BuildingData : ScriptableObject
	{
		[SerializeField] private string buildingName;
		[SerializeField] private GameObject prefab;
		[SerializeField] private Faction faction = Faction.None;
		[SerializeField] private BuildingType type = BuildingType.None;
		[SerializeField] private float hp = 100f;
		[SerializeField] private float buildCost = 0f;
		[SerializeField] private float upgradeCost = 0f;
		[SerializeField] private float upgradeTime = 5f;
        [SerializeField] private UnityEngine.UI.Slider statusBar;
        [SerializeField] private UnityEngine.UI.Slider healthBar;

		public string BuildingName { get { return buildingName; } }
		public GameObject Prefab { get { return prefab; } }
		public Faction Faction { get { return faction; } }
		public BuildingType Type { get { return type; } }
		public float Hp { get { return hp; } }
		public float BuildCost { get { return buildCost; } }
		public float UpgradeCost { get { return upgradeCost; } }
		public float UpgradeTime { get { return upgradeTime; } }
        public UnityEngine.UI.Slider StatusBar { get { return statusBar; } }
        public UnityEngine.UI.Slider HealthBar { get { return healthBar; } }

        // TODO: add mesh references to SquadData, and extend parameters with sub-types if needed
    }
}
