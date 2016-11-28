using System;
using Gameplay;
using UnityEngine;
using Core.Interactivity.Movement;
using Core.Pathfinding;
using Core.Interactivity.AI.Brains;
using UnityEngine.EventSystems;
using System.Security.AccessControl;



namespace Core.Interactivity.AI.AIStates
{
    public class AIStateDetectSlave:AIStateBase
    {
        private WarriorBrains _ownerBrain;
        private SlaveBrains _targetSlave;

        public AIStateDetectSlave(ArtificialIntelligence brains)
            : base(brains)
        {
            _ownerBrain = (WarriorBrains)brains;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _masterBrain.StatusText.text = "Just gonna come closer to spot..";
            _masterBrain.StatusText.color = Color.yellow;
            _masterBrain.MovableObject.DebugColor = Color.yellow;
        }

        public override void UpdateState()
        {
            FindSlave();
            FindNewpath();
        }

        public override void OnLeave()
        {
            
        }

        private void FindSlave()
        {
            var slaves = Game.Instance.ReferenceStorage.GetSlavesOfBiome(_ownerBrain.EnemyBiome);

            if (slaves == null || slaves.Length < 1)
            {
                _currentCondition = AIStateCondition.Done;
                _pendingState = EAIState.Wandering;
                return;
            }
            var distanceTonearestSlave = float.MaxValue;

            foreach (var slave in slaves)
            {
                if (!slave.gameObject.activeInHierarchy || slave == _targetSlave && slave.MovableObject.MyPosition.BiomOwner != Core.Bioms.EBiomType.None)
                {
                    continue;
                }
                var currentDistance = Vector3.Distance(_masterBrain.MovableObject.MyPosition.Position, slave.MovableObject.MyPosition.Position);

                var newTargetHasMoreHealth = _targetSlave != null && _targetSlave.isActiveAndEnabled && slave.Health.CurrentHealthAmount > _targetSlave.Health.CurrentHealthAmount;
                if (newTargetHasMoreHealth)
                {
                    continue;
                }

                if (currentDistance < distanceTonearestSlave)
                {
                    distanceTonearestSlave = currentDistance;
                    _ownerBrain.SetTarget(slave);
                    _targetSlave = slave;
                }
            }
        }

        private void FindNewpath()
        {
            if (_targetSlave != null && _targetSlave.isActiveAndEnabled)
            {
                var randomNeighbour = Game.Instance.CurrentMap.GetWalkableNeighbours(_targetSlave.MovableObject.MyPosition)[0];
                var path = Pathfinder.FindPathToDestination(
                               _masterBrain.MovableObject.MyPosition.GridPosition,
                               randomNeighbour.GridPosition);

                _masterBrain.MovableObject.BeginMovementByPath(path);
            }
            else
            {
                _masterBrain.MovableObject.CurrentPath.Nodes.Clear();
            }
        }
    }
}

