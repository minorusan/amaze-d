using UnityEngine;
using System.Collections;
using Gameplay;


public class Projectile : MonoBehaviour
{
	private Health _target;
	private Vector3 _startPosition;
	public GameObject Explosion;
	public float Speed;
	public int Damage;
	
	// Update is called once per frame
	public void SetTarget (Health target)
	{
		_target = target;
	}

	private void OnEnable ()
	{
		_startPosition = transform.position;
	}

	void Update ()
	{
		if (_target != null)
		{
			var distance = Vector3.Distance (_target.transform.position, transform.position);
			if (distance <= 1f)
			{
				var damage = (int)(Vector3.Distance (_target.transform.position, _startPosition) * Damage);
				damage = Mathf.Clamp (damage, 1, 50);
				Debug.Log ("Hit for " + damage);
				_target.CurrentHealthAmount -= damage;
				Explosion.transform.position = this.transform.position;
				Explosion.SetActive (true);
				gameObject.SetActive (false);
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, _target.transform.position, Speed);
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		Explosion.SetActive (true);
		Explosion.transform.position = this.transform.position;
		gameObject.SetActive (false);
	}
}
