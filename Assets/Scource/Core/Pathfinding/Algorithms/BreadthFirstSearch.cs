using System;
using Core.Interactivity.Movement;
using Core.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Gameplay;

namespace AssemblyCSharp
{
    public class BreadthFirstSearch : IPathFinder
    {
        private Queue _searchQueue;
        private Node _root;

        public BreadthFirstSearch(Node rootNode)
        {
            _searchQueue = new Queue();
        }

        public Path Search(Node data)
        {
            Node _current = _root;
            _searchQueue.Enqueue(_root);

            while (_searchQueue.Count != 0)
            {
                _current = (Node)_searchQueue.Dequeue();
                if (_current == data)
                {
                    return new Path(new List<Node>((Node[])_searchQueue.ToArray()));
                }
                else
                {
                    _searchQueue.Enqueue(_current.Left);
                    _searchQueue.Enqueue(_current.Right);
                }
            }
            return new Path();
        }

        #region IPathFinder implementation

        public Path FindPathToDestination(Core.Map.IJ currentNodeIndex, Core.Map.IJ targetNodeIndex)
        {
            _root = Game.Instance.CurrentMap.CurrentMapAsMatrix[currentNodeIndex.I, currentNodeIndex.J];
            var target = Game.Instance.CurrentMap.CurrentMapAsMatrix[targetNodeIndex.I, targetNodeIndex.J];
            return Search(target);
        }

        public EPathfindingAlgorithm Algorithm
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

