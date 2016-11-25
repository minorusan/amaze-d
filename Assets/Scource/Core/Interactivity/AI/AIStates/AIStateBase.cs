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

    public abstract class AIStateBase
    {
        protected Dictionary<EAIState, Action> _transitions;
        protected readonly ArtificialIntelligence _masterBrain;
        protected AIStateCondition _currentCondition;
        protected EAIState _pendingState = EAIState.Empty;

        public EAIState State
        {
            get;
            private set;
        }

        public AIStateCondition CurrentStateCondition
        {
            get
            {
                return _currentCondition;
            }
        }

        public EAIState PendingState
        {
            get
            {
                return _pendingState;
            }
        }

        public AIStateBase(ArtificialIntelligence brains)
        {
            _masterBrain = brains;
            _transitions = new Dictionary<EAIState, Action>();
        }

        public virtual void OnEnter()
        {
            _currentCondition = AIStateCondition.Active;
            _masterBrain.MovableObject.CurrentPath.Nodes.Clear();
        }

        public abstract void OnLeave();

        public abstract void UpdateState();

        protected bool IsDestinationCellBusy()
        {
            return _masterBrain.MovableObject.CurrentPath.Nodes.Last().CurrentCellType == Core.Map.ECellType.Busy;
        }
    }
}

