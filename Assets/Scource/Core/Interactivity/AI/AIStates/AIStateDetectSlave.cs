using System;
using Gameplay;
using UnityEngine;
using Core.Interactivity.Movement;
using Core.Pathfinding;
using Core.Interactivity.AI.Brains;



namespace Core.Interactivity.AI.AIStates
{
	public class AIStateDetectSlave:AIStateBase
	{
		private enum AttackState
		{
			Searches,
			Attacks
		}

		private AttackState _currentState;
		private SlaveBrains _previous;
		private WarriorBrains _ownerBrain;
		private bool goingToBiome;
		private SlaveBrains _targetSlave;
		private float _distanceTonearestSlave;

		public AIStateDetectSlave (ArtificialIntelligence brains) : base (brains)
		{
			_ownerBrain = (WarriorBrains)brains;
		}

		public override void OnEnter ()
		{
			base.OnEnter ();

			_masterBrain.StatusText.text = "Just gonna come closer to spot..";
			_masterBrain.StatusText.color = Color.yellow;
			_masterBrain.MovableObject.DebugColor = Color.yellow;
			goingToBiome = true;
			FindPathToBiome ();
		}

		public override void UpdateState ()
		{
			base.UpdateState ();
			if (goingToBiome && _ownerBrain.MovableObject.CurrentPath.Empty)
			{
				FindPathToBiome ();
			}

			if (!goingToBiome)
			{
				FindSlave ();
				FindNewpath ();
				if (_ownerBrain.MovableObject.CurrentPath.Nodes.Count < 3)
				{
					_masterBrain.StatusText.text = "YYAAAAARRR";
					_masterBrain.StatusText.color = Color.red;
				}
			}

		
			if (!_ownerBrain.MovableObject.CurrentPath.Empty && _ownerBrain.MovableObject.CurrentPath.Nodes.Count < 30)
			{
				goingToBiome = false;
			}
		

		}

		private void FindSlave ()
		{
			var slaves = Game.Instance.ReferenceStorage.GetSlavesOfBiome (_ownerBrain.EnemyBiome);
			_distanceTonearestSlave = float.MaxValue;

			foreach (var slave in slaves)
			{
				if (!slave.gameObject.activeInHierarchy)
				{
					continue;
				}
				var currentDistance = Vector3.Distance (_masterBrain.MovableObject.MyPosition.Position, slave.MovableObject.MyPosition.Position);
				if (currentDistance < _distanceTonearestSlave)
				{
					_distanceTonearestSlave = currentDistance;
					_ownerBrain._target = slave;
				}
			}
		

		}

		private void FindPathToBiome ()
		{
			var biomNode = Game.Instance.CurrentMap.GetWalkableBiomNode (_ownerBrain.EnemyBiome);

			if (biomNode != null)
			{
				_masterBrain.MovableObject.BeginMovementByPath (Pathfinder.FindPathToDestination (
					_masterBrain.MovableObject.MyPosition.GridPosition,
					biomNode.GridPosition
				));
			}
		}

		private void FindNewpath ()
		{
			
			var path = Pathfinder.FindPathToDestination (
				           _masterBrain.MovableObject.MyPosition.GridPosition,
				           _ownerBrain._target.MovableObject.MyPosition.GridPosition
			           );
			_masterBrain.MovableObject.BeginMovementByPath (path);
		}
	}
}

