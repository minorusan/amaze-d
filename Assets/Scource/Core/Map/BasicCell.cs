using UnityEngine;


namespace Core.Map
{
	public class BasicCell : MonoBehaviour
	{
		public ECellType CellType;

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

