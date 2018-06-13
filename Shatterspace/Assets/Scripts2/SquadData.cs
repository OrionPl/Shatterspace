using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts2
{
	[System.Serializable, CreateAssetMenu(menuName = "Shatterspace/Squad")]
	public class SquadData : ScriptableObject
	{
		[SerializeField] private string squadName;
		[SerializeField] private GameObject unitPrefab;
		[SerializeField] private Faction faction = Faction.None;
		[SerializeField] private float unitHp = 100f;
		[SerializeField] private float unitArmor = 10f;
		[SerializeField] private float unitSpeed = 1f;
		[SerializeField] private float spawnCost = 0f;
		[SerializeField] private float reinforceCost = 0f;
		[SerializeField] private float spawnTime = 5f;
		[SerializeField] private float reinforceTime = 5f;
		[SerializeField] private int startCount = 3;
		[SerializeField] private int maxCount = 5;
		[SerializeField] private float baseDamage = 5f;
		[SerializeField] private float armorDamageModifier = 1f;

		public GameObject UnitPrefab { get { return unitPrefab; } }
		public Faction Faction { get { return faction; } }
		public float UnitHp { get { return unitHp; } }
		public float UnitArmor { get { return unitArmor; } }
		public float UnitSpeed { get { return unitSpeed; } }
		public float SpawnCost { get { return spawnCost; } }
		public float ReinforceCost { get { return reinforceCost; } }
		public float SpawnTime { get { return spawnTime; } }
		public float ReinforceTime { get { return reinforceTime; } }
		public float StartCount { get { return startCount; } }
		public float MaxCount { get { return maxCount; } }
		public float BaseDamage { get { return baseDamage; } }
		public float ArmorDamageModifier { get { return armorDamageModifier; } }

		// TODO: add mesh references to SquadData, and extend parameters with sub-types if needed
	}
}
