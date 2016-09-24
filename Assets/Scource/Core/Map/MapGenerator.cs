using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.Movement;


namespace Core.Map
{
	[Serializable]
	public struct IJ
	{
		public int I;
		public int J;
	}

	public class MapGenerator : MonoBehaviour
	{
		#region PRIVATE

		private List<Node> _currentCellsArray;
		private Node[,] _currentNodeMatrix;
		private List<Node> _cellsInGame;

		#endregion

		public Node[,] CurrentMapAsMatrix {
			get
			{
				return _currentNodeMatrix;
			}
		}

		public List<Node> CurrentMap {
			get
			{
				return _currentCellsArray;
			}
		}

		[Header ("Map settings")]
		public bool DrawDebug;

		[Range (0, 100)]
		public int RandomFillPercentage;
		public int SmoothIterations;
		public IJ MapDimentions;
		public Vector2 CellSize;

		public Transform StartPoint;

		#region Monobehaviour

		void Awake ()
		{
			_currentCellsArray = new List<Node> ();
			InstantiateCells ();
		}

		private void OnDrawGizmos ()
		{
			if (_currentCellsArray == null || !DrawDebug)
			{
				return;
			}

			foreach (var item in _currentCellsArray)
			{
				var gizmoColor = Color.white;

				switch (item.CurrentCellType)
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
				case ECellType.Target:
					{
						gizmoColor = Color.yellow;
						break;
					}
				default:
					break;
				}
				Gizmos.color = gizmoColor;
				Gizmos.DrawSphere (item.Position, 0.3f);
			}

		}


		#endregion

		#region MapGeneratorInit

		public void InstantiateCells ()
		{

			if (_currentCellsArray == null)
			{
				_currentCellsArray = new List<Node> ();
			}
			else
			{
				_currentCellsArray.Clear ();
			}

			_currentNodeMatrix = new Node[MapDimentions.I, MapDimentions.J];

			var currentPosition = StartPoint.position;
			for (int i = 0; i < MapDimentions.I; i++)
			{

				for (int j = 0; j < MapDimentions.J; j++)
				{
					var instantiated = new Node ();

					currentPosition = new Vector3 (currentPosition.x + CellSize.x, currentPosition.y, currentPosition.z);
					instantiated.Position = currentPosition;
					instantiated.GridPosition = new IJ (){ I = i, J = j };

					_currentNodeMatrix [i, j] = instantiated;
					_currentCellsArray.Add (instantiated);
				}
				currentPosition = new Vector3 (StartPoint.localPosition.x, currentPosition.y, currentPosition.z + CellSize.y);
			}
		}

		public void GenerateObstacles ()
		{
			ProceduralCaveGenerator.GenerateCaveFromNodes (ref _currentNodeMatrix, RandomFillPercentage, SmoothIterations);
		}

		#endregion

		#region MapGeneratorUtils

		public List<Node> GetNeighbours (Node node)
		{
			List<Node> neighbours = new List<Node> ();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
						continue;

					int checkX = node.GridPosition.I + y;
					int checkY = node.GridPosition.J + x;

					if (checkX >= 0 && checkX < MapDimentions.J && checkY >= 0 && checkY < MapDimentions.I)
					{
						neighbours.Add (_currentNodeMatrix [checkX, checkY]);
					}
				}
			}

			return neighbours;
		}

		public int GetDistance (Node nodeA, Node nodeB)
		{
			int dstX = Mathf.Abs (nodeA.GridPosition.J - nodeB.GridPosition.J);
			int dstY = Mathf.Abs (nodeA.GridPosition.I - nodeB.GridPosition.I);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}

		#endregion
	}

}

