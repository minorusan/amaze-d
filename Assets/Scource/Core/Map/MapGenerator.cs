using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.Movement;
using System.Linq;
using System.Security.Policy;
using Core.Bioms;
using UnityEngine.Networking.Types;
using System.Xml.Linq;


namespace Core.Map
{
    [Serializable]



    public class IJ
    {
        public int I;
        public int J;

        public IJ(int i, int j)
        {
            I = i;
            J = j;
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return I * 1000 + J;
        }

    }

    public class MapGenerator : MonoBehaviour
    {
        #region PRIVATE

        private List<Node> _currentCellsArray;
        private Node[,] _currentNodeMatrix;
        private List<Node> _cellsInGame;
        private TerrainData _terrainData;
        private HashSet<Node> _dirtyNodes;

        #endregion

        public Node[,] CurrentMapAsMatrix
        {
            get
            {
                return _currentNodeMatrix;
            }
        }



        public List<Node> CurrentMap
        {
            get
            {
                return _currentCellsArray;
            }
        }

        private Dictionary<IJ, Node> nodesMap = new Dictionary<IJ, Node>();

        public GameObject Obstacle;
        public GameObject Enemy;
        public Transform EnemiesRoot;

        [Header("Map settings")]
        public bool DrawDebug;

        [Range(0, 100)]
        public int EnemiesOccupation;

        [Range(0, 100)]
        public int RandomFillPercentage;
        public int SmoothIterations;
        public IJ MapDimentions;
        public Vector2 CellSize;

        public Transform StartPoint;

        #region Monobehaviour

        void Awake()
        {
            _currentCellsArray = new List<Node>();
            _terrainData = FindObjectOfType<Terrain>().terrainData;
            InstantiateCells();
         
        }

        private void OnDrawGizmos()
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
                            gizmoColor = Color.blue;
                            break;
                        }
                    case ECellType.Busy:
                        {
                            gizmoColor = Color.yellow;
                            break;
                        }
                    case ECellType.Player:
                        {
                            gizmoColor = Color.white;
                            break;
                        }
                    default:
                        break;
                }
                if (item.BiomOwner == EBiomType.Fel && item.CurrentCellType == ECellType.Walkable)
                {
                    gizmoColor = Color.green;
                }

                Gizmos.color = gizmoColor;
                if (item.BiomOwner == EBiomType.Fel)
                {
                    Gizmos.DrawSphere(item.Position, 0.3f);
                }

            }

        }


        #endregion

        #region MapGeneratorInit

        public void InstantiateCells()
        {

            if (_currentCellsArray == null)
            {
                _currentCellsArray = new List<Node>();
            }
            else
            {
                _currentCellsArray.Clear();
            }

            _currentNodeMatrix = new Node[MapDimentions.I, MapDimentions.J];

            var currentPosition = StartPoint.position;
            for (int i = 0; i < MapDimentions.I; i++)
            {

                for (int j = 0; j < MapDimentions.J; j++)
                {
                    var instantiated = new Node();

                    currentPosition = new Vector3(currentPosition.x + CellSize.x, currentPosition.y, currentPosition.z);
                    instantiated.Position = currentPosition;
                   
                    instantiated.GridPosition = new IJ(i, j);

                    var flooredKey = new IJ(Mathf.RoundToInt(currentPosition.x), Mathf.RoundToInt(currentPosition.z));

                    nodesMap.Add(flooredKey, instantiated);
                    _currentNodeMatrix[i, j] = instantiated;
                    _currentCellsArray.Add(instantiated);
                }
                currentPosition = new Vector3(StartPoint.localPosition.x, currentPosition.y, currentPosition.z + CellSize.y);
            }

            DefineInwalkables();
            //GenerateObstacles ();
            //GenerateEnemies ();
        }

        private void DefineInwalkables()
        {
         
            foreach (var item in CurrentMap)
            {
                var yValue = MapHelpers.GetHeightWorldCoords(_terrainData, new Vector2(item.Position.x, item.Position.z));
                item.Position = new Vector3(item.Position.x, yValue, item.Position.z);
            }


            DetectUnwalkableCells(CurrentMapAsMatrix.GetLength(0));
        }

        private void DetectUnwalkableCells(Node forNode)
        {
            var neighbours = GetNeighbours(forNode, Vector2.one);

            int iterator = 0;
            for (int h = 0; h < neighbours.Length; h++)
            {
                if (neighbours[h] != null && Mathf.Abs(forNode.Position.y - neighbours[h].Position.y) < (neighbours[h].BiomOwner != Core.Bioms.EBiomType.None ? 3f : 0.7f))
                {
                    iterator++;
                }
            }

            if (iterator < neighbours.Length)
            {
                forNode.CurrentCellType = ECellType.Blocked;
            }
        }

        private void DetectUnwalkableCells(int inRange)
        {
            for (int i = 0; i < inRange; i++)
            {
                for (int j = 0; j < inRange; j++)
                {
                    DetectUnwalkableCells(CurrentMapAsMatrix[i, j]);
                }
            }
        }

        public void UpdateBiomVerticeNode(BiomVertice[] worldBiomVerticePosions, EBiomType biomType, int biomSize)
        {
            if (worldBiomVerticePosions != null)
            {
                for (int i = 0; i < worldBiomVerticePosions.Length; i++)
                {
                    var node = GetBiomNodeByPosition(worldBiomVerticePosions[i], biomSize);
                    if (node == null)
                    {
                        continue;
                    }
                    node.Position = new Vector3(node.Position.x, worldBiomVerticePosions[i].WorldPosition.y, node.Position.z);
       
                    node.BiomOwner = biomType;
                    DetectUnwalkableCells(node);
                }

            }
        }

        public void GenerateEnemies()
        {
            foreach (var item in CurrentMap.Where (cel=>cel.CurrentCellType == ECellType.Walkable))
            {
                if (UnityEngine.Random.Range(0, 100) < EnemiesOccupation)
                {
                    var z = Instantiate(Enemy);
                    z.GetComponent <MovableObject>().MyPosition = item;
                    z.transform.SetParent(EnemiesRoot);
                    z.transform.position = item.Position;
                }
            }
        }

        public void GenerateObstacles()
        {
            ProceduralCaveGenerator.GenerateCaveFromNodes(ref _currentNodeMatrix, RandomFillPercentage, SmoothIterations);
            foreach (var item in CurrentMap.Where (c=>c.CurrentCellType == ECellType.Blocked))
            {
                //var t = Instantiate (Obstacle);
                //t.transform.SetParent (transform);
                //t.transform.position = item.Position;
            }
        }

        #endregion

        #region MapGeneratorUtils

        public Node GetNodeByPosition(Vector3 position)
        {
            float minDist = Mathf.Infinity;
            Node toReturn = null;
            Vector3 currentPos = transform.position;

            foreach (var node in CurrentMap)
            {
                if (node != null)
                {
                    float dist = Vector3.Distance(node.Position, position);
                    if (dist < minDist)
                    {
                        toReturn = node;
                        minDist = dist;
                    }
                }
            
            }
            return toReturn;
        }

        IJ el = new IJ(0, 0);

        public Node GetBiomNodeByPosition(BiomVertice position, int biomSize)
        { 
            el.I = (Mathf.RoundToInt(position.WorldPosition.x));
            el.J = Mathf.RoundToInt(position.WorldPosition.z);
         
            return nodesMap[el];
        }

       
      

        public Node[] GetNeighbours(Node node, Vector2 inRange)
        {
            int iterator = 0;
            Node[] _neighbours = new Node[(int)inRange.x * 8];
            for (int x = (int)-inRange.x; x <= inRange.x; x++)
            {
                for (int y = (int)-inRange.y; y <= inRange.y; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.GridPosition.I + y;
                    int checkY = node.GridPosition.J + x;

                    if (checkX >= 0 && checkX < MapDimentions.J && checkY >= 0 && checkY < MapDimentions.I)
                    {
                        _neighbours[iterator] = _currentNodeMatrix[checkX, checkY];
                        iterator++;
                    }
                }
            }

            return _neighbours;
        }

        public List<Node> GetWalkableNeighbours(Node node)
        {
            return GetNeighbours(node, Vector2.one).Where(n => n.CurrentCellType == ECellType.Walkable).ToList();
        }

        public int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.GridPosition.J - nodeB.GridPosition.J);
            int dstY = Mathf.Abs(nodeA.GridPosition.I - nodeB.GridPosition.I);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        public int GetManhattanDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.GridPosition.J - nodeB.GridPosition.J);
            int dstY = Mathf.Abs(nodeA.GridPosition.I - nodeB.GridPosition.I);
            return dstX + dstY;
			
        }


        #endregion
    }

}

