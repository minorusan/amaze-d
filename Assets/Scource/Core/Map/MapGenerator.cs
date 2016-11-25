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
        private Dictionary <EBiomType, HashSet<Node>> _walkableBiomNode;

        #endregion

        public bool DoneUpdatingVerticeNodes
        {
            get;
            set;
        }

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
			
                if (item.CurrentCellType == ECellType.Blocked)
                {
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawSphere(item.Position, 0.3f);
                }

            }
        }

        #endregion

        #region MapGeneratorInit

        public void InstantiateCells()
        {
            DoneUpdatingVerticeNodes = true;
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
            _walkableBiomNode = new Dictionary<EBiomType, HashSet<Node>>();
            _walkableBiomNode.Add(EBiomType.Fel, new HashSet<Node>());
            _walkableBiomNode.Add(EBiomType.Storm, new HashSet<Node>());
            DefineInwalkables();
            //StartCoroutine(GenerateObstacles());

            //GenerateEnemies ();
        }

        public Node GetWalkableBiomNode(EBiomType type)
        {
            var nodes = _walkableBiomNode[type];
            return nodes.LastOrDefault();
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
                if (neighbours[h] != null && Mathf.Abs(forNode.Position.y - neighbours[h].Position.y) < 2f)
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
                _walkableBiomNode[biomType].Clear();
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
                    if (node.CurrentCellType == ECellType.Walkable)
                    {
                        _walkableBiomNode[biomType].Add(node);
                    }
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

        #endregion

        #region MapGeneratorUtils

        public Node GetNodeByPosition(Vector3 position)
        {
            el.I = (Mathf.RoundToInt(position.x));
            el.J = Mathf.RoundToInt(position.z);

            return nodesMap[el];
        }

        IJ el = new IJ(0, 0);

        public Node GetBiomNodeByPosition(BiomVertice position, int biomSize)
        { 
            el.I = (Mathf.RoundToInt(position.WorldPosition.x));
            el.J = Mathf.RoundToInt(position.WorldPosition.z);
         
            return nodesMap[el];
        }

        Node[] _neighbours = new Node[8];

        public Node[] GetNeighbours(Node node, Vector2 inRange)
        {
            int iterator = 0;

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

