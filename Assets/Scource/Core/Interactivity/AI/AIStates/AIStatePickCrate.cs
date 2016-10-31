using UnityEngine;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Pathfinding;
using Core.Interactivity.AI.Brains;


namespace Core.Interactivity.AI.AIStates
{
	public class AIStatePickCrate:AIStateBase
	{
		private SlaveBrains _ownerBrain;

		public AIStatePickCrate (ArtificialIntelligence brains) : base (brains)
		{
			_ownerBrain = (SlaveBrains)brains;
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			BasicBonus.CrateBeingPicked += OnCreatePicked;
			_masterBrain.StatusText.text = "Cratecratecratecrate!";
			_masterBrain.StatusText.color = Color.green;
			_masterBrain.MovableObject.DebugColor = Color.green;
			FindNewpath ();
		}

		void OnCreatePicked ()
		{
			FindNewpath ();
		}

		public override void OnLeave ()
		{
			base.OnLeave ();

		}


		public override void UpdateState ()
		{
			base.UpdateState ();
			if (_ownerBrain.MovableObject.CurrentPath.Empty)
			{
				FindNewpath ();
			}

			if (_ownerBrain.CratePickedUp)
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.CratePicked;
				BasicBonus.CrateBeingPicked -= OnCreatePicked;
			}

		}



		private void FindNewpath ()
		{
			var crateNode = Game.Instance.CurrentSession.BonusManager.GetClosestCratePosition (_masterBrain.MovableObject.MyPosition.Position);
			if (crateNode == null)
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.Wandering;
			}
		
			_masterBrain.MovableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (
				_masterBrain.MovableObject.MyPosition.GridPosition,
				crateNode.GridPosition
			));
		}
	}
}

