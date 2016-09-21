using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using Core.Map;

using System.Collections.Generic;


namespace Utils.Testing
{
	public class KillCommand : MonoBehaviour
	{
		public MovableObject Dummy;
		public BasicCell[] Path;

		public void GoRapeHim ()
		{
			var nodesList = new List<Node> ();
			for (int i = 0; i < Path.Length; i++)
			{
				nodesList.Add (Path [i].AssociatedNode);
			}

			Dummy.BeginMovementByPath (new Path (nodesList));
		}

	}

}
