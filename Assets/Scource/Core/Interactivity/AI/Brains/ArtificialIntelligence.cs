using UnityEngine;
using System;
using System.Collections;
using Core.Interactivity.AI.AIStates;
using System.Collections.Generic;
using Core.Interactivity.Movement;
using Gameplay;
using UnityEngine.UI;
using System.Linq;


namespace Core.Interactivity.AI
{
    [RequireComponent(typeof(MovableObject))]
    [RequireComponent(typeof(Health))]
    public abstract class ArtificialIntelligence : MonoBehaviour
    {
        #region Protected

        public EAIState BaseState;
        protected AIStateBase _currentState;
        protected Dictionary<EAIState, AIStateBase> _availiableStates = new Dictionary<EAIState, AIStateBase>();
        protected MovableObject _movableObject;
        protected Health _health;

        #endregion

        public bool AllowsUpdates = false;

        public MovableObject MovableObject
        {
            get
            {
                return _movableObject;
            }
        }

        public Health Health
        {
            get
            {
                return _health;
            }
        }

        public Text StatusText;

        #region Monobehaviour

        private void Awake()
        {
            _movableObject = GetComponent <MovableObject>();
            _health = GetComponent<Health>();
            InitStates();
        }

        protected virtual void Start()
        {
            _movableObject.MyPosition = Game.Instance.CurrentMap.GetNodeByPosition(transform.position);
            _health.OnWillDie += OnBrainWillDie;
            _health.OnDied += OnBrainDied;
        }

        protected virtual void OnBrainWillDie()
        {
            
        }

        protected virtual void OnBrainDied()
        {
            Debug.Log(this.name + " died");
        }

        protected virtual void OnEnable()
        {
            Debug.Assert(BaseState >= 0, this.name + " did not have a base state ID. CRASH LOUDLY");
           
            _currentState = _availiableStates.Count >= 1 ? _availiableStates[BaseState] : _availiableStates[_availiableStates.Keys.First()];
            _currentState.OnEnter();
        }

        private void Update()
        {
            if (_currentState == null || !AllowsUpdates)
            {
                return;
            }

            if (_currentState.CurrentStateCondition == AIStateCondition.Done)
            {
                MoveToState(_currentState.PendingState);
            }
            else
            {
                _currentState.UpdateState();
            }
        }

        #endregion

        protected abstract void InitStates();

        public void MoveToState(EAIState pendingState)
        {
            if (_availiableStates[pendingState] != null)
            {
                _currentState.OnLeave();
                _currentState = _availiableStates[pendingState];
                _currentState.OnEnter();
            }
        }
    }
}

