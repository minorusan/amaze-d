using System;
using System.Collections.Generic;
using Core.Interactivity.Movement;
using System.Linq;
using Core.Map;
using Gameplay;
using UnityEngine;


namespace Core.Pathfinding.Algorithms
{
    public class AStar:IPathFinder
    {
        private List<ECellType> _ignoredNodeTypes;

        #region IPathFinder implementation

        public EPathfindingAlgorithm Algorithm
        {
            get
            {
                return EPathfindingAlgorithm.AStar;
            }
        }

        public AStar()
        {
            _ignoredNodeTypes = new List<ECellType>{ ECellType.Blocked, ECellType.Busy };
        }

        public Path FindPathToDestination(IJ currentNodeIndex, IJ targetNodeIndex)
        {
            var map = Game.Instance.CurrentMap.CurrentMapAsMatrix;
            Node startNode = map[currentNodeIndex.I, currentNodeIndex.J];
            Node targetNode = map[targetNodeIndex.I, targetNodeIndex.J];



            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            var currentMap = Game.Instance.CurrentMap;

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost)
                    {
                        if (openSet[i].HCost < node.HCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (Node neighbour in currentMap.GetNeighbours(node, Vector2.one))
                {
                    var ignored = _ignoredNodeTypes.Any(p => p == neighbour.CurrentCellType);
                    if (ignored || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.GCost + currentMap.GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = currentMap.GetDistance(neighbour, targetNode);
                        neighbour.Parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
            return new Path();
        }

        #endregion

        private Path RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();

            return new Path(path);

        }
    }
}

