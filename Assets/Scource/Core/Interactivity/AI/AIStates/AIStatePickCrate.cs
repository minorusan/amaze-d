using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using Core.Interactivity.AI.Brains;
using System;


namespace Core.Interactivity.AI.AIStates
{
    public class AIStatePickCrate:AIStateBase
    {
        private SlaveBrains _ownerBrain;
        private Node _currentTargetNode;

        public AIStatePickCrate(ArtificialIntelligence brains)
            : base(brains)
        {
            _ownerBrain = (SlaveBrains)brains;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            BasicBonus.CrateBeingPicked += OnCreatePicked;
            _masterBrain.StatusText.text = "Cratecratecratecrate!";
            _masterBrain.StatusText.color = Color.green;
            _masterBrain.MovableObject.DebugColor = Color.green;
        }

        void OnCreatePicked(object sender, EventArgs args)
        {
            if (_ownerBrain.CratePickedUp == false)
            {
                var senderPosition = Game.Instance.CurrentMap.GetNodeByPosition((sender as BasicBonus).transform.position);
                if (_currentTargetNode != null && senderPosition == _currentTargetNode)
                {
                    FindNewpath();
                }
            }
        }

        public override void OnLeave()
        {
            BasicBonus.CrateBeingPicked -= OnCreatePicked;
        }

        public override void UpdateState()
        {
            if (_ownerBrain.CratePickedUp)
            {
                _currentCondition = AIStateCondition.Done;
                _pendingState = EAIState.CratePicked;
                return;
            }

            if (_ownerBrain.MovableObject.CurrentPath.Empty)
            {
                FindNewpath();
            }
        }

        private void FindNewpath()
        {
            var crateNode = Game.Instance.CurrentSession.BonusManager.GetClosestCratePosition(_masterBrain.MovableObject.MyPosition.Position);

            if (crateNode != null)
            {
                _currentTargetNode = crateNode;
                _masterBrain.MovableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(
                        _masterBrain.MovableObject.MyPosition.GridPosition,
                        crateNode.GridPosition
                    ));
            }
        }
    }
}

