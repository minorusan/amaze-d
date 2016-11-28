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
        public Health _target;
        public GameObject NoTarget;
        private Animator _selfAnimator;
        private Vector3 _prevPosition;
        private bool _initialised = false;

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
            _selfAnimator = GetComponent<Animator>();
            _initialised = true;
        }

        private void OnEnable()
        {
            if (_initialised)
            {
                StopAllCoroutines();
                StartCoroutine(GetPostion());
            }
           
        }

        public void Punch()
        {
            if (_target != null)
            {
                _selfAnimator.SetTrigger("Attack");
                _target.CurrentHealthAmount -= 20;
                if (_target.CurrentHealthAmount <= 0)
                {
                    _target = null;
                }
            }
            else
            {
                NoTarget.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (_target != null && col.gameObject == _target.gameObject)
            {
                _target = null;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            var artificialIntelligentce = col.GetComponent<Health>();
            if (artificialIntelligentce != null)
            {
                _target = artificialIntelligentce;
            }
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

