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
		private bool _didFindPath;

		public AIStateCratePicked (ArtificialIntelligence brains) : base (brains)
		{
			_ownerBrain = (SlaveBrains)brains;
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			_didFindPath = false;
			_masterBrain.StatusText.text = "Home!";
			_masterBrain.StatusText.color = Color.green;
			_masterBrain.MovableObject.DebugColor = Color.green;
			FindNewpath ();
		}

		public override void OnLeave ()
		{
			base.OnLeave ();

		}


		public override void UpdateState ()
		{
			base.UpdateState ();

			if (!_didFindPath || _ownerBrain.MovableObject.CurrentPath.Empty)
			{
				FindNewpath ();
			}

			if (!_ownerBrain.CratePickedUp)
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.PickCrate;

			}
		}



		private void FindNewpath ()
		{

			var biomNode = Game.Instance.CurrentMap.GetWalkableBiomNode (_ownerBrain.OwnerBiome);

			if (biomNode != null)
			{
				_didFindPath = true;
				_masterBrain.MovableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (
					_masterBrain.MovableObject.MyPosition.GridPosition,
					biomNode.GridPosition
				));
			}

		}
	}
}

