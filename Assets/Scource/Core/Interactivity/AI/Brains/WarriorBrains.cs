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
		private SlaveBrains _target;

		public float Damage = 5;
		public EBiomType EnemyBiome;

		#region Monobehavior

		private void LateUpdate ()
		{
			Attack ();
		}

		#endregion

		#region WarriorBrains

		public void SetTarget (ArtificialIntelligence target)
		{
			_target = (SlaveBrains)target;
		}

		public void Attack ()
		{
			if (_target != null && _target.isActiveAndEnabled && Vector3.Distance (_target.MovableObject.MyPosition.Position, transform.position) < 2f)
			{
				_movableObject.SelfAnimator.SetTrigger ("Attack");
			}
		}

		public void Punch ()
		{
			if (_target != null)
			{
				_target.Health.CurrentHealthAmount -= (int)Damage;
				if (_target.Health.CurrentHealthAmount < 0)
				{
					Damage += 1;
					transform.localScale *= 1.2f;

					_target = null;
				}
			}
		}

		#endregion

		#region ArtificialIntelligence

		protected override void InitStates ()
		{
			_availiableStates.Add (EAIState.DetectSlave, new AIStateDetectSlave (this));
			_availiableStates.Add (EAIState.Wandering, new AIStateWandering (this));
			BaseState = EAIState.DetectSlave;
		}

		#endregion
	}
}

