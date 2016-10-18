using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using Core.Bioms;


namespace Core.Interactivity.Movement
{
	public class Node
	{
		public int GCost;
		public int HCost;
		public bool IsIgnored = false;
		public bool IsPositionDirty = false;

		public EBiomType BiomOwner = EBiomType.None;

		public int FCost {
			get
			{
				return GCost + HCost;
			}
		}



		public Node Parent;

		public ECellType CurrentCellType;
		public Vector3 Position;
		public IJ GridPosition;
	}

	public enum EMovableObjectState
	{
		Walking,
		Standing
	}

	public class Path
	{
		public bool Empty {
			get
			{
				return _nodes.Count <= 0;
			}

		}

		public Path ()
		{
			_nodes = new List<Node> ();
		}

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

