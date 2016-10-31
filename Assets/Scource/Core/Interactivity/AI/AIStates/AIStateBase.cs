using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace Core.Interactivity.AI.AIStates
{
	public enum AIStateCondition
	{
		Done,
		Active
	}

	public class AIStateBase
	{
		protected Dictionary<EAIState, Action> _transitions;
		protected readonly ArtificialIntelligence _masterBrain;
		protected AIStateCondition _currentCondition;
		protected EAIState _pendingState = EAIState.Empty;

		public EAIState State {
			get;
			private set;
		}

		public AIStateBase (ArtificialIntelligence brains)
		{
			_masterBrain = brains;
			_transitions = new Dictionary<EAIState, Action> ();
		}

		public virtual void OnEnter ()
		{
			_currentCondition = AIStateCondition.Active;

		}

		public virtual void OnLeave ()
		{

		}

		public virtual void UpdateState ()
		{
			if (_currentCondition == AIStateCondition.Done)
			{
				Debug.Assert (_pendingState != EAIState.Empty, "BRAINS::ERROR");
				_masterBrain.MoveToState (_pendingState);
			}
		}

		protected bool IsDestinationCellBusy ()
		{
			return _masterBrain.MovableObject.CurrentPath.Nodes.Last ().CurrentCellType == Core.Map.ECellType.Busy;
		}
	}
}

