using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Interactivity.AI;
using Core.Bioms.BiomeComponents;


namespace Core.Bioms
{
	public class BiomSpawnsPool : MonoBehaviour, IBiomeComponent
	{
		#region PRIVATE

		private BiomSpawn[] _biomSpawns;
		private BiomBase _owner;

		#endregion

		#region IBiomeComponent

		public void SetActive (bool active)
		{
		}


		public void InitComponent (BiomBase owner)
		{
			_owner = owner;
			GetSpawns ();
		}

		public void UpdateComponent ()
		{
			if (_biomSpawns != null && _biomSpawns.Length > 0)
			{
				for (int i = 0; i < _biomSpawns.Length; i++)
				{
					if (_biomSpawns [i] != null && IsPowerConditionSatisfied (_biomSpawns [i]) && !_biomSpawns [i].isActiveAndEnabled)
					{
						_biomSpawns [i].gameObject.SetActive (true);
						_biomSpawns [i].transform.position = _biomSpawns [i].RandomizeLocation ? _owner.Shaper.GetRandomPosition (_owner.SpawnArea) : _biomSpawns [i].transform.position; 
					}

					if (_biomSpawns [i] != null && !IsPowerConditionSatisfied (_biomSpawns [i]))
					{
						_biomSpawns [i].DeactivateSpawn ();
					}
				}
			}
		}

		#endregion

		#region Internal

		private bool IsPowerConditionSatisfied (BiomSpawn _biomsSpawn)
		{
			return _biomsSpawn.RequiredPower <= _owner.BiomPower;
		}

		private void GetSpawns ()
		{
			_biomSpawns = GetComponentsInChildren<BiomSpawn> ();
			for (int i = 0; i < _biomSpawns.Length; i++)
			{
				_biomSpawns [i].gameObject.SetActive (false);
			}
		}

		#endregion
	}
}

