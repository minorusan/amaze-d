using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using Core.Interactivity.AI.Brains;


namespace Core.Interactivity.AI.AIStates
{
    public class AIStateWandering:AIStateBase
    {
        private Node _currentDestination;
        private WarriorBrains _ownerBrain;
        private float _previousMoveSpeed;
        private float _timeIdle = 3f;

        public AIStateWandering(ArtificialIntelligence brains)
            : base(brains)
        {
            _transitions.Add(EAIState.Alert, () =>
			Debug.Log("Zombie::???????"));

            _ownerBrain = (WarriorBrains)brains;
        }

        public override void OnEnter()
        {
            base.OnEnter();
		
            _masterBrain.StatusText.text = "ZZZZzz....";
            _masterBrain.StatusText.color = Color.green;
            _previousMoveSpeed = _masterBrain.MovableObject.MovementSpeed;
            _masterBrain.MovableObject.MovementSpeed *= 0.4f;
            _masterBrain.MovableObject.DebugColor = Color.green;
        }

        public override void OnLeave()
        {
            _masterBrain.MovableObject.MovementSpeed = _previousMoveSpeed;
        }

        public override void UpdateState()
        {
            if (_timeIdle <= 0)
            {
                FindNewpath();
                _timeIdle = 6f;
            }
            else
            {
                _timeIdle -= Time.deltaTime;
                CheckSwitchState();
            }
        }

        private void CheckSwitchState()
        {
            if (Game.Instance.ReferenceStorage.GetSlavesOfBiome(_ownerBrain.EnemyBiome).Length > 0)
            {
                _currentCondition = AIStateCondition.Done;
                _pendingState = EAIState.DetectSlave; 
            }
        }

        private void FindNewpath()
        {
            var possibleLocations = Game.Instance.CurrentMap.GetWalkableNeighbours(_masterBrain.MovableObject.MyPosition);
            if (possibleLocations.Count > 1)
            {
                var destination = possibleLocations[Random.Range(0, possibleLocations.Count - 1)];
                _currentDestination = destination;
                _masterBrain.MovableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(
                        _masterBrain.MovableObject.MyPosition.GridPosition,
                        destination.GridPosition));
            }
        }
    }
}

