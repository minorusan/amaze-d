using UnityEngine;

using System.Linq;
using System.Collections;


namespace Core.Interactivity.Movement
{
	public class MovableObject : MonoBehaviour
	{
		private Path _currentPath;
		private bool _movesNow;

		public bool MovesNow {
			get
			{
				return _movesNow;
			}
		}

		public Animator SelfAnimator;

		public float MovementSpeed;

		// Use this for initialization
		void Start ()
		{

		}

		public void BeginMovementByPath (Path path)
		{
			_movesNow = false;
			_currentPath = path;
			ToggleWalkAnimation ();
		}

	
		// Update is called once per frame
		void Update ()
		{
			if (_currentPath != null && _currentPath.Nodes.Count > 0)
			{
				var rotation = Quaternion.LookRotation (_currentPath.Nodes [0].OwnerPosition - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 0.1f);

				MoveToTarget (_currentPath.Nodes [0].OwnerPosition);

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
			if (_currentPath.Nodes [0].OwnerPosition == this.transform.position)
			{
				_currentPath.Nodes.Remove (_currentPath.Nodes [0]);
			}

			if (_currentPath.Nodes.Count <= 0)
			{
				ToggleWalkAnimation ();
			}

		}

		private void DrawDebugPath ()
		{
			Debug.DrawLine (transform.position, _currentPath.Nodes [0].OwnerPosition);
			for (int i = 0; i < _currentPath.Nodes.Count - 1; i++)
			{
				Debug.DrawLine (_currentPath.Nodes [i].OwnerPosition, _currentPath.Nodes [i + 1].OwnerPosition);
			}
		}

		protected virtual void ToggleWalkAnimation ()
		{
			_movesNow = !_movesNow;
			SelfAnimator.SetBool ("Walk", _movesNow);
		}
	}
}

