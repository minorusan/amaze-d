using System;
using UnityEngine;
using Core.Interactivity.Movement;
using Core.Map;
using Gameplay;


namespace Core.Interactivity
{
	public class BonusManager
	{
		private BasicBonus[] _bonuses;

		public BonusManager ()
		{
			_bonuses = GameObject.FindObjectsOfType <BasicBonus> ();
		}

		public Node GetClosestCratePosition (Vector3 position)
		{
			float minDist = Mathf.Infinity;
			Node toReturn = null;
			Vector3 currentPos = position;

			foreach (var bonus in _bonuses)
			{
				if (bonus != null && bonus.CurrentState != EBonusState.Picked)
				{
					float dist = Vector3.Distance (bonus.transform.position, position);
					if (dist < minDist)
					{
						toReturn = Game.Instance.CurrentMap.GetNodeByPosition (bonus.transform.position);
						minDist = dist;
					}
				}

			}
			return toReturn;
		}
	}
}

