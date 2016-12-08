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

		Path FindPathToDestination (IJ currentNodeIndex, IJ targetNodeIndex, bool optimised = true);
	}

	public enum EPathfindingAlgorithm
	{
		Deikstra,
		AStar,
		DepthFirstSearch,
		BreadthFirst
	}
}

