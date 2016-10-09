using UnityEngine;
using System.Collections;
using Gameplay;


namespace Core.Bioms
{
	public enum BiomSpawnState
	{
		Grows,
		Exists,
		Dies
	}

	public class BiomSpawn:MonoBehaviour
	{
		public Vector3 InitialScale;
		private BiomSpawnState _currentState;
		private float _remainingLifeTime;

    

		[Tooltip ("Stop spawining after that amount of biom power")]

		public bool Disposable;
		public bool Infinite;
		public bool RandomizeLocation;
		public int RequiredPower = 3;
		public int LimitPower = 80;
		public float LifeSpan = 30;

		private void Awake ()
		{
			InitialScale = transform.localScale;
		}

		private void OnEnable ()
		{
			_remainingLifeTime = LifeSpan;
			_currentState = BiomSpawnState.Grows;
			transform.localScale = Vector3.zero;
		}

		private void Update ()
		{
			switch (_currentState)
			{
			case BiomSpawnState.Grows:
				{
					Grow ();
					break;
				}
			case BiomSpawnState.Exists:
				{
					Exist ();
					break;
				}
			case BiomSpawnState.Dies:
				{
					Dies ();
					break;
				}
			default:
				break;
			}
		}

		#region PRIVATE

		private void Dies ()
		{
			transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.zero, Time.deltaTime);
			if (transform.localScale.x <= 0.1f)
			{
				gameObject.SetActive (false);
				if (Disposable)
				{
					Destroy (this.gameObject);
				}

			}
		}

		private void Exist ()
		{
			if (_remainingLifeTime > 0 || Infinite)
			{
				_remainingLifeTime -= Time.deltaTime;
			}
			else
			{
				_currentState = BiomSpawnState.Dies;
				DeactivateSpawn ();
			}
		}

		private void Grow ()
		{
			transform.localScale = Vector3.MoveTowards (transform.localScale, InitialScale, Time.deltaTime);
			if (transform.localScale == InitialScale)
			{
				_currentState = BiomSpawnState.Exists;
			}
		}

		public void DeactivateSpawn ()
		{
			var col = GetComponent<Collider> ();
			if (col != null)
			{
				col.enabled = false;
			}
		}

		#endregion

	}


}
