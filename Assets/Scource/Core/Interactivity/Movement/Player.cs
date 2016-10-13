using System;
using Gameplay;
using Core.Map;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Core.Interactivity.Movement
{
    public class Player:MonoBehaviour
    {
        public Action PlayerPositionedChanged;
        private Vector3 _prevPosition;

        private Node _myPosition;

        public List<Node> AttackablePositions
        {
            get
            {
                return Game.Instance.CurrentMap.GetNeighbours(MyPosition, Vector2.one).Where(n => n.CurrentCellType == ECellType.Walkable).ToList();
            }
        }

        public Node MyPosition
        {
            get
            {
                return _myPosition;
            }
            protected set
            {
                if (value != null)
                {
                    if (_myPosition != null)
                    {
                        _myPosition.CurrentCellType = ECellType.Walkable;
                        for (int i = 0; i < Game.Instance.CurrentMap.CurrentMap.Count; i++)
                        {
                            if (Game.Instance.CurrentMap.CurrentMap[i].CurrentCellType == ECellType.Target)
                            {
                                Game.Instance.CurrentMap.CurrentMap[i].CurrentCellType = ECellType.Walkable;
                            }
                        }
                    }
                    _myPosition = value;
                    value.CurrentCellType = ECellType.Player;
                    if (PlayerPositionedChanged != null)
                    {
                        PlayerPositionedChanged();
                    }

                }
            }
        }

        private void Start()
        {
            StartCoroutine(GetPostion());
        }

        private IEnumerator GetPostion()
        {
            while (true)
            {
                if (transform.position != _prevPosition)
                {
                    var newPosition = Game.Instance.CurrentMap.GetNodeByPosition(transform.position);
                    if (MyPosition == null || newPosition != null && newPosition != MyPosition)
                    {
                        MyPosition = newPosition;
                    }
                    _prevPosition = transform.position;
                }
                yield return new WaitForSeconds(3);
            }
        }
    }
}

