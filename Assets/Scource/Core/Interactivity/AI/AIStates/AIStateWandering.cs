using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;


namespace Core.Interactivity.AI.AIStates
{
	public class AIStateWandering:AIStateBase
	{
		private Node _currentDestination;
		private float _searchDistance;
		private float _previousMoveSpeed;
		private float _timeIdle = 3f;

		public AIStateWandering (ArtificialIntelligence brains, float searchDistance) : base (brains)
		{
			_searchDistance = searchDistance;
			_transitions.Add (EAIState.Alert, () =>
			Debug.Log ("Zombie::???????"));
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
		
			_masterBrain.StatusText.text = "ZZZZzz....";
			_masterBrain.StatusText.color = Color.green;
			_previousMoveSpeed = _masterBrain.MovableObject.MovementSpeed;
			_masterBrain.MovableObject.MovementSpeed *= 0.4f;
			_masterBrain.MovableObject.DebugColor = Color.green;
		}

		public override void OnLeave ()
		{
			base.OnLeave ();
			_masterBrain.MovableObject.MovementSpeed = _previousMoveSpeed;
		}


		public override void UpdateState ()
		{
			base.UpdateState ();
			if (!PlayerIsSomewhereNear ())
			{
				if (_currentDestination == null || _currentDestination == _masterBrain.MovableObject.MyPosition)
				{
					if (_timeIdle <= 0)
					{
						FindNewpath ();
						_timeIdle = 6f;
					}
					else
					{
						_timeIdle -= Time.deltaTime;
					}
				}	
			}
			else
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.Alert;
			}
		}

		private bool PlayerIsSomewhereNear ()
		{
			var playerPosition = Game.Instance.CurrentSession.Player.MyPosition.Position;
			var myPosition = _masterBrain.MovableObject.MyPosition.Position;
			var sub = playerPosition - myPosition;

			return Vector3.Distance (playerPosition, myPosition) <= _searchDistance;
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

