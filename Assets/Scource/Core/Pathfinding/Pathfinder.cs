using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.Movement;
using Core.Map;
using Core.Pathfinding.Algorithms;
using Gameplay;


namespace Core.Pathfinding
{
    public class Pathfinder
    {

        private static Dictionary<EPathfindingAlgorithm, IPathFinder> _currentAlgorithms;

        static Pathfinder()
        {
            _currentAlgorithms = new Dictionary<EPathfindingAlgorithm, IPathFinder>();
            _currentAlgorithms.Add(EPathfindingAlgorithm.AStar, new AStar());
        }


        public static Path FindPathToDestination(IJ currentNodeIndex, IJ targetNodeIndex,
                                                 EPathfindingAlgorithm algorithm = EPathfindingAlgorithm.AStar)
        {
            var pathfinderToUse = _currentAlgorithms[algorithm];
            return pathfinderToUse.FindPathToDestination(currentNodeIndex, targetNodeIndex);
        }


    }
}

