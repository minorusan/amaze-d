using UnityEngine;
using Core.Interactivity.Movement;


namespace Core.Map
{
	public class BasicCell : MonoBehaviour
	{
		private Node _associatedNode;

		public Node AssociatedNode {
			get
			{
				return _associatedNode;
			}
			private set
			{
				_associatedNode = value;
				value.OwnerPosition = transform.position;
			}
		}

		public ECellType CellType;

		public void InitWithNode (Node value)
		{
			if (AssociatedNode == null)
			{
				AssociatedNode = value;
			}
		}



		private void OnDrawGizmos ()
		{
			var gizmoColor = Color.white;

			switch (CellType)
			{
			case ECellType.Blocked:
				{
					gizmoColor = Color.red;
					break;
				}
			case ECellType.Walkable:
				{
					gizmoColor = Color.green;
					break;
				}
			default:
				break;
			}
			Gizmos.color = gizmoColor;
			Gizmos.DrawSphere (transform.position, 0.3f);
		}
	}
}

