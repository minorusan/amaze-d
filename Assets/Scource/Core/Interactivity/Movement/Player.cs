using System;
using Gameplay;
using Core.Map;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace Core.Interactivity.Movement
{
	public class Player:MonoBehaviour
	{
		public Health _target;
		public Projectile Projectile;

		public Action PlayerPositionedChanged;
		private Text _errorText;

		public GameObject NoTarget;
		private Animator _selfAnimator;
		private Vector3 _prevPosition;
		private bool _initialised = false;

		private Node _myPosition;

		public List<Node> AttackablePositions {
			get
			{
				return Game.Instance.CurrentMap.GetNeighbours (MyPosition, Vector2.one).Where (n => n.CurrentCellType == ECellType.Walkable).ToList ();
			}
		}

		public Node MyPosition {
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
						PlayerPositionedChanged ();
					}
				}
			}
		}

		private void Start ()
		{
			StartCoroutine (GetPostion ());
			_selfAnimator = GetComponent<Animator> ();
			_initialised = true;
			_errorText = NoTarget.GetComponent <Text> ();
		}

		private void OnEnable ()
		{
			if (_initialised)
			{
				StopAllCoroutines ();
				StartCoroutine (GetPostion ());
			}
		}

		public void Cast ()
		{
			if (IsTargetInRange (25f))
			{
				if (!this.Projectile.isActiveAndEnabled)
				{
					_selfAnimator.SetTrigger ("Attack");
					transform.LookAt (_target.transform);
					this.Projectile.transform.position = transform.position;
					this.Projectile.SetTarget (_target);
					this.Projectile.gameObject.SetActive (true);
				}
				else
				{
					_errorText.text = "I cannot do it yet";
					NoTarget.SetActive (true);
				}
			}
			else
			{
				_errorText.text = "I need to get closer";
				NoTarget.SetActive (true);
			}
		}


		public void Punch ()
		{
			if (IsTargetInRange (2f))
			{
				if (_target != null)
				{
					_selfAnimator.SetTrigger ("Attack");
					transform.LookAt (_target.transform);
					_target.CurrentHealthAmount -= 5;
					if (_target.CurrentHealthAmount <= 0)
					{
						_target = null;
					}
				}
				else
				{
					_errorText.text = "I have to target something first";
					NoTarget.SetActive (true);
				}
			}
			else
			{
				_errorText.text = "Need to get closer";
				NoTarget.SetActive (true);
			}
		}

		private void OnTriggerExit (Collider col)
		{
			
			if (IsTargetInRange (2f) && col.gameObject == _target.gameObject)
			{
				_target = null;
			}
		}

		private bool IsTargetInRange (float range)
		{
			return _target != null && _target.gameObject.activeInHierarchy && Vector3.Distance (_target.transform.position, transform.position) < range;
		}


		public void SetTarget (GameObject target)
		{
			var targetHealth = target.GetComponent<Health> ();
			if (targetHealth != null)
			{
				_target = targetHealth;
			}
		}

		private void OnTriggerEnter (Collider col)
		{
			var artificialIntelligentce = col.GetComponent<Health> ();
			if (artificialIntelligentce != null)
			{
				_target = artificialIntelligentce;
			}
		}

		private IEnumerator GetPostion ()
		{
			while (true)
			{
				if (transform.position != _prevPosition)
				{
					var newPosition = Game.Instance.CurrentMap.GetNodeByPosition (transform.position);
					if (MyPosition == null || newPosition != null && newPosition != MyPosition)
					{
						MyPosition = newPosition;
					}
					_prevPosition = transform.position;
				}
				yield return new WaitForSeconds (3);
			}
		}
	}
}

