using System;
using Core.Interactivity.Movement;
using Core.Map;


namespace Core.Pathfinding
{
	internal interface IPathFinder
	{
		EPathfindingAlgorithm Algorithm {
			get;
		}

		Path FindPathToDestination (IJ currentNodeIndex, IJ targetNodeIndex, Node[,] map);
	}

	public enum EPathfindingAlgorithm
	{
		Deikstra,
		AStar,
		DepthFirstSearch
	}
}

