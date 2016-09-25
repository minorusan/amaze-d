using System;
using Gameplay;
using Core.Map;
using UnityEngine;
using System.Linq;


namespace Core.Interactivity.Movement
{
	public class Player:MonoBehaviour
	{
		public Action PlayerPositionedChanged;

		private Node _myPosition;

		public Node AttackablePosition {
			get
			{
				return Game.Instance.CurrentMap.GetNeighbours (MyPosition).FirstOrDefault (n => n.CurrentCellType == ECellType.Walkable);
			}
		}

		public Node MyPosition {
			get
			{
				return _myPosition;
			}
			protected set
			{
				if (value != null)
				{
					if (_myPosition != null)
					{
						_myPosition.CurrentCellType = ECellType.Walkable;
						for (int i = 0; i < Game.Instance.CurrentMap.CurrentMap.Count; i++)
						{
							if (Game.Instance.CurrentMap.CurrentMap [i].CurrentCellType == ECellType.Target)
							{
								Game.Instance.CurrentMap.CurrentMap [i].CurrentCellType = ECellType.Walkable;
							}
						}
					}
					_myPosition = value;
					value.CurrentCellType = ECellType.Player;
					if (PlayerPositionedChanged != null)
					{
						PlayerPositionedChanged ();
					}

				}
			}
		}

		private void Update ()
		{
			var newPosition = Game.Instance.CurrentMap.GetNodeByPosition (transform.position);
			if (MyPosition == null || newPosition != null && newPosition != MyPosition)
			{
				MyPosition = newPosition;
			}

		}
	}
}

