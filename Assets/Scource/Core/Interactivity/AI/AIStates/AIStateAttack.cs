using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using System.Linq;
using UnityEngine.EventSystems;


namespace Core.Interactivity.AI.AIStates
{
	public class AIStateAttack:AIStateBase
	{
		private Node _currentDestination;
		private Node _previousDestination;
		private Player _player;
		private bool _attacks;
		private int _leaveDistance;


		public AIStateAttack (ArtificialIntelligence brains, int leaveDistance) : base (brains)
		{
			_leaveDistance = leaveDistance;
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			_masterBrain.StatusText.text = "YAAARRRRR!11";
			_masterBrain.StatusText.color = Color.red;
			_masterBrain.MovableObject.DebugColor = Color.red;
			_player = Game.Instance.CurrentSession.Player;
			_player.PlayerPositionedChanged += GoToPlayer;
		}

		public override void OnLeave ()
		{
			base.OnLeave ();
			_player.PlayerPositionedChanged -= GoToPlayer;
		}

		private void GoToPlayer ()
		{
			var a = _player.AttackablePosition;
			if (a != null)
			{
				_masterBrain.MovableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (_masterBrain.MovableObject.MyPosition.GridPosition,
				                                                                                  a.GridPosition));
			}
		}

		public override void UpdateState ()
		{
			base.UpdateState ();
			if (PlayerIsSomewhereNear ())
			{
				if (_masterBrain.MovableObject.ReachedDestination || _masterBrain.MovableObject.CurrentPath.Nodes.Last ().CurrentCellType == Core.Map.ECellType.Busy)
				{
					var g = Game.Instance.CurrentMap.GetNeighbours (_player.MyPosition).All (p => p != _masterBrain.MovableObject.MyPosition);
					var a = _player.AttackablePosition;
					if (g && a != null)
					{
						_masterBrain.MovableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (_masterBrain.MovableObject.MyPosition.GridPosition,
						                                                                                  a.GridPosition));
					}
					else
					{
						_masterBrain.MovableObject.transform.LookAt (_player.transform);
						if (!_attacks)
						{
							_attacks = true;
							_masterBrain.MovableObject.SelfAnimator.SetBool ("Attack", true);
						}
					}

				}
				else
				if (_attacks)
				{
					_attacks = false;
					_masterBrain.MovableObject.SelfAnimator.SetBool ("Attack", false);
				}
			}
			else
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.Wandering;
			}
		}


		private bool PlayerIsSomewhereNear ()
		{
			return _masterBrain.MovableObject.CurrentPath.Nodes.Count < _leaveDistance;
		}

		private void FindNewpath ()
		{
			var possibleLocations = Game.Instance.CurrentMap.GetWalkableNeighbours (_masterBrain.MovableObject.MyPosition);
			if (possibleLocations.Count > 1)
			{

				var destination = possibleLocations [Random.Range (0, possibleLocations.Count - 1)].GridPosition;
				_masterBrain.MovableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (
					_masterBrain.MovableObject.MyPosition.GridPosition,
					destination));
			}
		}
	}
}

