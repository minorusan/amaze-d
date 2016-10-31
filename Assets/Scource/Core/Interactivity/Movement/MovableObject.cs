using UnityEngine;

using System.Linq;
using System.Collections;
using Gameplay;
using Core.Map;
using Core.Pathfinding;


namespace Core.Interactivity.Movement
{


	public class MovableObject : MonoBehaviour
	{
		private Path _currentPath = new Path ();
		private EMovableObjectState _currentState = EMovableObjectState.Standing;
		private Node _myPosition;

		public bool RandomStart;
		public Color DebugColor;

		public Node MyPosition {
			get
			{
				return Game.Instance.CurrentMap.GetNodeByPosition (transform.position);
			}
			set
			{
				if (value != null)
				{
					if (_myPosition != null)
					{
						_myPosition.CurrentCellType = ECellType.Walkable;
					}

					_myPosition = value;

					value.CurrentCellType = ECellType.Busy;
					transform.position = value.Position;
				}
			}
		}

		public bool ReachedDestination {
			get
			{
				return _currentPath.Empty;
			}
		}

		public Path CurrentPath {
			get
			{
				return _currentPath;
			}
		}

		public Animator SelfAnimator;

		public float MovementSpeed;

		// Use this for initialization
		void Start ()
		{
			if (_myPosition == null && RandomStart)
			{
				var passable = Enumerable.Where (Game.Instance.CurrentMap.CurrentMap, c => c.CurrentCellType == ECellType.Walkable).ToList ();
				var target = passable [Random.Range (0, passable.Count () - 1)];
				MyPosition = target;
			}


		}

		public void BeginMovementByPath (Path path)
		{
			
			_currentPath = path;
			ToggleWalkAnimation (EMovableObjectState.Walking);
		}

	
		// Update is called once per frame
		protected void Update ()
		{
			SelfAnimator.SetFloat ("Speed", MovementSpeed * 120);
			if (!_currentPath.Empty)
			{
				var rotation = Quaternion.LookRotation (_currentPath.Nodes [0].Position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 0.1f);

				if (_currentPath.Nodes [0].CurrentCellType == ECellType.Busy)
				{
					BeginMovementByPath (Pathfinder.FindPathToDestination (MyPosition.GridPosition, _currentPath.Nodes.Last ().GridPosition));
					if (!_currentPath.Empty)
					{
						MoveToTarget (_currentPath.Nodes [0].Position);
					}
					else
					{
						ToggleWalkAnimation (EMovableObjectState.Standing);
					}
				}
				else
				{
					MoveToTarget (_currentPath.Nodes [0].Position);
				}
			


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
			if (Vector3.Distance (_currentPath.Nodes [0].Position, this.transform.position) < 0.1f)//(_currentPath.Nodes [0].Position == this.transform.position)
			{
				MyPosition = _currentPath.Nodes [0];

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
			Debug.DrawLine (startDraw, endFirst, DebugColor);
			for (int i = 0; i < _currentPath.Nodes.Count - 1; i++)
			{
				var start = new Vector3 (_currentPath.Nodes [i].Position.x, _currentPath.Nodes [i].Position.y + 1, _currentPath.Nodes [i].Position.z);
				var end = new Vector3 (_currentPath.Nodes [i + 1].Position.x, _currentPath.Nodes [i + 1].Position.y + 1, _currentPath.Nodes [i + 1].Position.z);

				Debug.DrawLine (start, end, DebugColor);
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

