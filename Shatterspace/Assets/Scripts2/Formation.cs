using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Formation : ScriptableObject
{
	[SerializeField] private List<Vector3> positions;
	public List<Vector3> Positions { get { return positions; } }

	/// <summary>
	/// Handy in situations where the movement is based around a central spot.
	/// </summary>
	/// <returns>The centre of mass, the mean vector combining all others.</returns>
	public Vector3 CalculateCentreOfMass()
	{
		Vector3 result = Vector3.zero;

		for (int i = 0; i < positions.Count; i++)
		{
			result += positions[i];
		}

		return result / positions.Count;
	}

}
