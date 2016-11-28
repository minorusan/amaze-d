using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using UnityEngine.EventSystems;


namespace Core.Interactivity.AI.AIStates
{
    public class AIStateAlert:AIStateBase
    {
        private Node _currentDestination;
        private Node _previousDestination;
        private float _previousMoveSpeed;
        private float _searchDistance;


        public AIStateAlert(ArtificialIntelligence brains, float searchDistance = 40f)
            : base(brains)
        {
            _searchDistance = searchDistance;
		
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _masterBrain.StatusText.text = "??????";
            _masterBrain.StatusText.color = Color.yellow;
            _masterBrain.MovableObject.DebugColor = Color.yellow;
            _previousMoveSpeed = _masterBrain.MovableObject.MovementSpeed;
            _masterBrain.MovableObject.MovementSpeed *= 0.6f;
        }

        public override void OnLeave()
        {
            _masterBrain.MovableObject.MovementSpeed = _previousMoveSpeed;
        }


        public override void UpdateState()
        {
            if (PlayerIsSomewhereNear())
            {
                if (!PlayerIsVisible())
                {
                    if (_masterBrain.MovableObject.ReachedDestination)
                    {
                        FindNewpath();
                    }	
                }
                else
                {
                    _currentCondition = AIStateCondition.Done;
                    _pendingState = EAIState.Attack;
                }
            }
        }

        private bool PlayerIsVisible()
        {
            var playerPosition = Game.Instance.CurrentSession.Player.MyPosition.Position;

            var myPosition = _masterBrain.MovableObject.MyPosition.Position;
            var raycastPosition = new Vector3(myPosition.x, myPosition.y + 1f, myPosition.z);
            var hitPosition = new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z);
            RaycastHit info;
            Physics.Raycast(raycastPosition, hitPosition - raycastPosition, out info, _searchDistance * 2);

            if (info.transform != null && info.transform.gameObject.tag == "Player")
            {
                return true;
            }
            return false;
        }

        private bool PlayerIsSomewhereNear()
        {
            var playerPosition = Game.Instance.CurrentSession.Player.MyPosition.Position;
            var myPosition = _masterBrain.MovableObject.MyPosition.Position;

            return Vector3.Distance(playerPosition, myPosition) <= _searchDistance;
        }

        private void FindNewpath()
        {
            var possibleLocations = Game.Instance.CurrentMap.GetWalkableNeighbours(_masterBrain.MovableObject.MyPosition);
            if (possibleLocations.Count > 1)
            {
				
                var destination = possibleLocations[Random.Range(0, possibleLocations.Count - 1)].GridPosition;
                _masterBrain.MovableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(
                        _masterBrain.MovableObject.MyPosition.GridPosition,
                        destination));
            }
        }
    }
}

