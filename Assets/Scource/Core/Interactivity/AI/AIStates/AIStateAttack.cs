using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace Core.Interactivity.AI.AIStates
{
    public class AIStateAttack:AIStateBase
    {
        private Node _currentDestination;
        private Node _previousDestination;
        private Player _player;
        private bool _attacks;
        private int _leaveDistance;


        public AIStateAttack(ArtificialIntelligence brains, int leaveDistance = 60)
            : base(brains)
        {
            _leaveDistance = leaveDistance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _masterBrain.StatusText.text = "YAAARRRRR!11";
            _masterBrain.StatusText.color = Color.red;
            _masterBrain.MovableObject.DebugColor = Color.red;

            _player = Game.Instance.CurrentSession.Player;
            _player.PlayerPositionedChanged += MoveToPlayer;
        }

        public override void OnLeave()
        {
            _player.PlayerPositionedChanged -= MoveToPlayer;
        }

        public override void UpdateState()
        {
            if (IsPlayerReachable())
            {
                GoGetHim();
            }
            else
            {
                _currentCondition = AIStateCondition.Done;
                _pendingState = EAIState.Alert;
            }
        }

        #region PRIVATE

        private void MoveToPlayer()
        {
            var suitableAttackPosition = GetNearestAttackablePosition(_player.AttackablePositions); 
            if (suitableAttackPosition != null)
            {
                _masterBrain.MovableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(_masterBrain.MovableObject.MyPosition.GridPosition,
                        suitableAttackPosition.GridPosition));
            }
        }

        private void GoGetHim()
        {
            var distance = Vector3.Distance(_player.transform.position, _masterBrain.transform.position);
            if (_masterBrain.MovableObject.ReachedDestination && distance < 2f)
            {
                _masterBrain.transform.LookAt(_player.transform.position);
                _masterBrain.MovableObject.SelfAnimator.SetTrigger("Attack");
                _masterBrain.MovableObject.SelfAnimator.SetBool("Walk", true);
            }
        }


        private Node GetNearestAttackablePosition(List<Node> positions)
        {
            float distance = float.MaxValue;
            Node nodeToReturn = new Node();
            foreach (var node in positions)
            {
                var currentDistance = Vector3.Distance(_masterBrain.MovableObject.MyPosition.Position, node.Position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    nodeToReturn = node;
                }
            }
            return nodeToReturn;
        }

        private bool IsPlayerReachable()
        {
            return _player.isActiveAndEnabled;
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

        #endregion
    }
}