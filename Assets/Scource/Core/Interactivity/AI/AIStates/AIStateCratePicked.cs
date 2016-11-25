using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using Core.Interactivity.AI.Brains;


namespace Core.Interactivity.AI.AIStates
{
    public class AIStateCratePicked:AIStateBase
    {
        private SlaveBrains _ownerBrain;

        public AIStateCratePicked(ArtificialIntelligence brains)
            : base(brains)
        {
            _ownerBrain = (SlaveBrains)brains;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _masterBrain.StatusText.text = "Home!";
            _masterBrain.StatusText.color = Color.green;
            _masterBrain.MovableObject.DebugColor = Color.green;
            FindNewpath();
        }

        public override void OnLeave()
        {
           
        }

        public override void UpdateState()
        {
            if (!_ownerBrain.CratePickedUp)
            {
                _currentCondition = AIStateCondition.Done;
                _pendingState = EAIState.PickCrate;
                return;
            }
                
            FindNewpath();
        }

        private void FindNewpath()
        {
            var biomNode = Game.Instance.CurrentMap.GetWalkableBiomNode(_ownerBrain.OwnerBiome);

            var walkableNighbour = Game.Instance.CurrentMap.GetWalkableNeighbours(biomNode)[0];
            if (walkableNighbour != null)
            {
                _masterBrain.MovableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(
                        _masterBrain.MovableObject.MyPosition.GridPosition,
                        walkableNighbour.GridPosition
                    ));
            }

        }
    }
}

