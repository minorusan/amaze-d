using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Core.Interactivity.Movement
{
	public class Node
	{
		public Vector3 OwnerPosition;
	}

	public class Path
	{
		private readonly List<Node> _nodes;

		public List<Node> Nodes {
			get
			{
				return _nodes;
			}
		}

		public Path (List<Node> nodes)
		{
			_nodes = nodes;
		}
	}
}

