using System;
using Core.Interactivity.AI;
using Core.Interactivity.AI.AIStates;


namespace Core.Interactivity.AI.Brains
{
	public class ZombieBrains:ArtificialIntelligence
	{
		public float SearchDistance;

		protected override void Start ()
		{
			base.Start ();
			_availiableStates.Add (EAIState.Wandering, new AIStateWandering (this, SearchDistance));
			_availiableStates.Add (EAIState.Alert, new AIStateAlert (this, SearchDistance * 2));
			_availiableStates.Add (EAIState.Attack, new AIStateAttack (this, 10));

			_currentState = _availiableStates [EAIState.Wandering];
			_currentState.OnEnter ();
		}
	}
}

