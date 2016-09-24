using UnityEngine;

using System.Linq;
using System.Collections;
using Gameplay;


namespace Core.Interactivity.Movement
{


	public class MovableObject : MonoBehaviour
	{
		private Path _currentPath = new Path ();
		private EMovableObjectState _currentState = EMovableObjectState.Standing;
		private Node _myPosition;

		public Node MyPosition {
			get
			{
				return _myPosition;
			}
			private set
			{
				_myPosition = value;
				transform.position = value.Position;
			}
		}


		public Animator SelfAnimator;

		public float MovementSpeed;

		// Use this for initialization
		void Start ()
		{
			if (_myPosition == null)
			{
				MyPosition = Enumerable.FirstOrDefault (Game.Instance.CurrentMap.CurrentMap, c => c.CurrentCellType == Core.Map.ECellType.Walkable);
			}
		}

		public void BeginMovementByPath (Path path)
		{
			
			_currentPath = path;
			ToggleWalkAnimation (EMovableObjectState.Walking);
		}

	
		// Update is called once per frame
		void Update ()
		{
			if (!_currentPath.Empty)
			{
				var rotation = Quaternion.LookRotation (_currentPath.Nodes [0].Position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 0.1f);

				MoveToTarget (_currentPath.Nodes [0].Position);

				DrawDebugPath ();
			}
		}

		private void MoveToTarget (Vector3 target)
		{
			transform.position = Vector3.MoveTowards (transform.position, target, MovementSpeed);
			CheckIfDestinationIsReached ();
		}

		private void CheckIfDestinationIsReached ()
		{
			if (_currentPath.Nodes [0].Position == this.transform.position)
			{
				_myPosition = _currentPath.Nodes [0];
				_currentPath.Nodes.Remove (_currentPath.Nodes [0]);
			}

			if (_currentPath.Nodes.Count <= 0)
			{
				ToggleWalkAnimation (EMovableObjectState.Standing);
			}

		}

		private void DrawDebugPath ()
		{
			if (_currentPath.Empty)
			{
				return;
			}
			var startDraw = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
			var endFirst = new Vector3 (_currentPath.Nodes [0].Position.x, _currentPath.Nodes [0].Position.y + 1, _currentPath.Nodes [0].Position.z);
			Debug.DrawLine (startDraw, endFirst);
			for (int i = 0; i < _currentPath.Nodes.Count - 1; i++)
			{
				var start = new Vector3 (_currentPath.Nodes [i].Position.x, _currentPath.Nodes [i].Position.y + 1, _currentPath.Nodes [i].Position.z);
				var end = new Vector3 (_currentPath.Nodes [i + 1].Position.x, _currentPath.Nodes [i + 1].Position.y + 1, _currentPath.Nodes [i + 1].Position.z);

				Debug.DrawLine (start, end);
			}
		}

		protected virtual void ToggleWalkAnimation (EMovableObjectState state)
		{
			switch (state)
			{
			case EMovableObjectState.Standing:
				{
					SelfAnimator.SetBool ("Walk", false);
					_currentState = EMovableObjectState.Standing;
					break;
				}
			case EMovableObjectState.Walking:
				{
					SelfAnimator.SetBool ("Walk", true);
					_currentState = EMovableObjectState.Walking;
					break;
				}
			default:
				break;
			}

		}
	}
}

