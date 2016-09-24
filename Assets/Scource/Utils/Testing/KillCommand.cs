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
		public MovableObject Dummy;

		public int x, y;

		public void GoRapeHim ()
		{
			var passable = Enumerable.Where (Game.Instance.CurrentMap.CurrentMap, c => c.CurrentCellType == ECellType.Walkable).ToList ();
			var target = passable [Random.Range (0, passable.Count () - 1)];
			Dummy.BeginMovementByPath (Pathfinder.FindPathToDestination (Dummy.MyPosition.GridPosition, new IJ {
				I = target.GridPosition.I,
				J = target.GridPosition.J
			}, 
			                                                             Game.Instance.CurrentMap.CurrentMapAsMatrix));
		}

	}

}
