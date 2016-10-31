using System;
using Core.Interactivity.AI;
using Core.Bioms;
using Core.Interactivity.AI.AIStates;
using System.Diagnostics;
using UnityEngine;


namespace Core.Interactivity.AI.Brains
{
	public class WarriorBrains:ArtificialIntelligence
	{
		public EBiomType EnemyBiome;
		public SlaveBrains _target;

		public void Attack ()
		{
			if (_target != null && Vector3.Distance (_target.MovableObject.MyPosition.Position, transform.position) < 2f)
			{
				MovableObject.SelfAnimator.SetTrigger ("Attack");
			}

		}

		protected override void Start ()
		{
			base.Start ();

			_availiableStates.Add (EAIState.DetectSlave, new AIStateDetectSlave (this));

			_currentState = _availiableStates [EAIState.DetectSlave];
			_currentState.OnEnter ();
		}

		private void Update ()
		{
			Attack ();
		}

		public void Punch ()
		{
			_target.Health.CurrentHealthAmount -= 10;
			if (_target.Health.CurrentHealthAmount <= 0)
			{
				transform.localScale *= 1.1f;
				UnityEngine.Debug.Log ("KILL");
			}
		}
	}
}

