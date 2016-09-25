using UnityEngine;
using System.Collections;
using Core.Interactivity.AI.AIStates;
using System.Collections.Generic;
using Core.Interactivity.Movement;


namespace Core.Interactivity.AI
{
	[RequireComponent (typeof(MovableObject))]
	public class ArtificialIntelligence : MonoBehaviour
	{
		protected AIStateBase _currentState;
		protected Dictionary<EAIState, AIStateBase> _availiableStates;
		public MovableObject MovableObject;

		public TextMesh StatusText;

		// Use this for initialization
		protected virtual void Start ()
		{
			MovableObject = GetComponent <MovableObject> ();
			_availiableStates = new Dictionary<EAIState, AIStateBase> ();
		}

		// Update is called once per frame
		protected virtual void LateUpdate ()
		{
			if (_currentState != null)
			{
				_currentState.UpdateState ();
			}
		}

		public void MoveToState (EAIState pendingState)
		{
			if (_availiableStates [pendingState] != null)
			{
				_currentState.OnLeave ();
				_currentState = _availiableStates [pendingState];
				_currentState.OnEnter ();
			}
		}
	}
}

