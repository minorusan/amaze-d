using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using Core.Map;

using System.Collections.Generic;
using System.Linq;
using Core.Pathfinding;
using Gameplay;


namespace Utils.Testing
{
	public class KillCommand : MonoBehaviour
	{
		public MovableObject[] Dummis;

		public int x, y;

		public void GoRapeHim ()
		{
			for (int i = 0; i < Dummis.Length; i++)
			{
				var passable = Enumerable.Where (Game.Instance.CurrentMap.CurrentMap, c => c.CurrentCellType == ECellType.Walkable).ToList ();
				var target = passable [Random.Range (0, passable.Count () - 1)];
				Dummis [i].BeginMovementByPath (Pathfinder.FindPathToDestination (Dummis [i].MyPosition.GridPosition, new IJ {
					I = target.GridPosition.I,
					J = target.GridPosition.J
				}));
			}
		}

			                                                            

	}

}
