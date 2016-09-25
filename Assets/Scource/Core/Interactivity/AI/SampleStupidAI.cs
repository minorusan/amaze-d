using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using System.Linq;
using System;
using Core.Map;


namespace Core.Interactivity.AI
{
	public class SampleStupidAI : MonoBehaviour
	{
		private bool _attacks;
		private MovableObject _movableObject;
		private Player _player;

		private void Start ()
		{
			_movableObject = GetComponent <MovableObject> ();
			_player = Game.Instance.CurrentSession.Player;
			_player.PlayerPositionedChanged += () =>
			{
				var a = _player.AttackablePosition;
				if (a != null)
				{
					_movableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (_movableObject.MyPosition.GridPosition,
					                                                                      a.GridPosition));
				}
			

			};
		}

		private void LateUpdate ()
		{

			if (_movableObject.ReachedDestination || _movableObject.CurrentPath.Nodes.Last ().CurrentCellType == Core.Map.ECellType.Busy)
			{
				var g = Game.Instance.CurrentMap.GetNeighbours (_player.MyPosition).All (p => p != _movableObject.MyPosition);
				var a = _player.AttackablePosition;
				if (g && a != null)
				{
					_movableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (_movableObject.MyPosition.GridPosition,
					                                                                      a.GridPosition));
				}
				else
				{
					_movableObject.transform.LookAt (_player.transform);
					if (!_attacks)
					{
						_attacks = true;
						_movableObject.SelfAnimator.SetBool ("Attack", true);
					}
				}

			}
			else
			if (_attacks)
			{
				_attacks = false;
				_movableObject.SelfAnimator.SetBool ("Attack", false);
			}
		}
			
	}
}

