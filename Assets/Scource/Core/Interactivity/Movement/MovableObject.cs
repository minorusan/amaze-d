using UnityEngine;

using System.Linq;
using System.Collections;
using Gameplay;
using Core.Map;
using Core.Pathfinding;


namespace Core.Interactivity.Movement
{
    [RequireComponent(typeof(Animator))]
    public class MovableObject : MonoBehaviour
    {
        #region PRIVATE

        private Path _currentPath = new Path();
        private EMovableObjectState _currentState = EMovableObjectState.Standing;
        private Node _myPosition;
        private Animator _animator;

        #endregion

        public Color DebugColor;
        public ECellType CurrentCell;
        public ECellType TargetCell;
        public float MovementSpeed;

        #region Properties

        public Node MyPosition
        {
            get
            {
                return Game.Instance.CurrentMap.GetNodeByPosition(transform.position);
            }
            set
            {
                Debug.Assert(value != null, this.name + " attempted to obtain null position.");
                _myPosition = value;
                _myPosition.CurrentCellType = ECellType.Busy;
            }
        }

        public bool ReachedDestination
        {
            get
            {
                return _currentPath.Empty;
            }
        }

        public Path CurrentPath
        {
            get
            {
                return _currentPath;
            }
        }

        public Animator SelfAnimator
        {
            get
            {
                return _animator;
            }
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
           
            SelfAnimator.SetFloat("Speed", MovementSpeed * 100);
            if (!_currentPath.Empty)
            {
                TargetCell = _currentPath.Nodes.LastOrDefault().CurrentCellType;
                CurrentCell = MyPosition.CurrentCellType;
                var rotation = Quaternion.LookRotation(_currentPath.Nodes[0].Position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);

                if (_currentPath.Nodes[0].CurrentCellType == ECellType.Busy)
                {
                    BeginMovementByPath(Pathfinder.FindPathToDestination(MyPosition.GridPosition, _currentPath.Nodes.Last().GridPosition));
                    if (!_currentPath.Empty)
                    {
                        MoveToTarget(_currentPath.Nodes[0].Position);
                    }
                    else
                    {
                        ToggleAnimationState(EMovableObjectState.Standing);
                    }
                }
                else
                {
                    MoveToTarget(_currentPath.Nodes[0].Position);
                }

                DrawDebugPath();
            }
        }

        private void OnDisable()
        {
            _currentPath.Nodes.Clear();
        }

        #endregion

        public void BeginMovementByPath(Path path)
        {
            _currentPath.Nodes.Clear();
            _currentPath = path;
            ToggleAnimationState(EMovableObjectState.Walking);
        }

        #region Internal

        private void MoveToTarget(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, MovementSpeed);
            CheckIfDestinationIsReached();
        }

        private void CheckIfDestinationIsReached()
        {
            if (Vector3.Distance(_currentPath.Nodes[0].Position, this.transform.position) < 0.1f)
            {
                MyPosition = _currentPath.Nodes[0];
                _currentPath.Nodes.Remove(_currentPath.Nodes[0]);
            }

            if (_currentPath.Empty)
            {
                ToggleAnimationState(EMovableObjectState.Standing);
            }
        }

        private void DrawDebugPath()
        {
            if (_currentPath.Empty)
            {
                return;
            }

            var startDraw = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            var endFirst = new Vector3(_currentPath.Nodes[0].Position.x, _currentPath.Nodes[0].Position.y + 1, _currentPath.Nodes[0].Position.z);
            Debug.DrawLine(startDraw, endFirst, DebugColor);
            for (int i = 0; i < _currentPath.Nodes.Count - 1; i++)
            {
                var start = new Vector3(_currentPath.Nodes[i].Position.x, _currentPath.Nodes[i].Position.y + 1, _currentPath.Nodes[i].Position.z);
                var end = new Vector3(_currentPath.Nodes[i + 1].Position.x, _currentPath.Nodes[i + 1].Position.y + 1, _currentPath.Nodes[i + 1].Position.z);

                Debug.DrawLine(start, end, DebugColor);
            }
        }

        protected virtual void ToggleAnimationState(EMovableObjectState state)
        {
            switch (state)
            {
                case EMovableObjectState.Standing:
                    {
                        SelfAnimator.SetBool("Walk", false);
                        _currentState = EMovableObjectState.Standing;
                        break;
                    }
                case EMovableObjectState.Walking:
                    {
                        SelfAnimator.SetBool("Walk", true);
                        _currentState = EMovableObjectState.Walking;
                        break;
                    }
                default:
                    break;
            }

        }

        #endregion
    }
}

