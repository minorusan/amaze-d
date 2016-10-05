using UnityEngine;
using System.Collections;
using Gameplay;


namespace Core.Bioms
{
	public class BiomSpawn:MonoBehaviour
	{
		private Vector3 _initialScale;
		private bool _grows;
		private bool _deactivating;

		public BiomBase Owner;
		public int StartIteration = 3;
		public int EndIteration = 30;

		private void Start ()
		{
			_initialScale = transform.localScale;
			transform.localScale = Vector3.zero;
			_grows = true;
			_deactivating = false;
		}

		private void Update ()
		{
			if (_grows)
			{
				transform.localScale = Vector3.MoveTowards (transform.localScale, _initialScale, Time.deltaTime);
				if (transform.localScale == _initialScale)
				{
					_grows = false;
				}
			}

			if (_deactivating)
			{
				transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.zero, Time.deltaTime);
				if (transform.localScale.x <= 0.1f)
				{
					_deactivating = false;
					gameObject.SetActive (false);
				}
			}

		}

		public void DeactivateSpawn ()
		{
			if (_deactivating == false)
			{
				var col = GetComponent<Collider> ();
				if (col != null)
				{
					col.enabled = false;
				}

				_deactivating = true;
			}
		}
	}


}
