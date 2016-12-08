using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Core.Bioms.BiomeComponents;
using Core.Interactivity.AI.Brains;
using System.Collections.Generic;
using Gameplay;



namespace Core.Bioms
{
	public abstract class BiomBase : MonoBehaviour
	{
		#region PRIVATE

		//Backing fields & private
		private int _power = 1;
		private bool _canCollide = true;

		//Components
		private List<IBiomeComponent> _biomeComponents;
		protected BiomShaper _shaper;
		private BiomCollisionDetector _collisionDetector;
		private BiomSlavesController _slavesController;
		private BiomWarriorsController _warriorsController;

		#endregion

		[Header ("Basic biome settings")]
		public bool ControlledByPlayer;

		public float Padding = 3.5f;
		public GameObject Plane;
		public GameObject Surface;
		public Collider SpawnArea;
		public Collider BonusDeliveryArea;

		public float GrowthRate = 3;
		public float GrowthCoeficient = 3;

		#region Properties

		public EBiomType BiomType {
			get;
			protected set;
		}

		public int BiomPower {
			get
			{
				return _power;
			}
			set
			{
				_power = value;
				PerformTerrainGeneration ();
			}
		}

		public BiomShaper Shaper {
			get
			{
				return _shaper;
			}
		}

		public float[,] CurrentMap {
			get;
			protected set;
		}

		#endregion

		#region MONOBEHAVIOUR

		protected virtual void Awake ()
		{  
			Init ();
		}

		protected virtual void Start ()
		{
			_collisionDetector = GetComponentInChildren<BiomCollisionDetector> ();
			_collisionDetector.CollidedWithBiome += OnCollidedWithBiome;
			InvokeRepeating ("IncreasePower", 0.0f, GrowthRate);
		}

		private void Update ()
		{
			
			if (ControlledByPlayer && !Game.Instance.CurrentSession.Player.isActiveAndEnabled && BiomPower > 20)
			{
				Game.Instance.CurrentSession.Player.gameObject.SetActive (true);
				BiomPower -= 20;
				Game.Instance.CurrentSession.Player.transform.position = new Vector3 (transform.position.x, 10f, transform.position.z);
			}

			for (int i = 0; i < _biomeComponents.Count; i++)
			{
				_biomeComponents [i].UpdateComponent ();
			}
		}

		#endregion

		#region Public

		public void SpawnSlave ()
		{
			_slavesController.Spawn ();
		}

		public void SpawnWarrior ()
		{
			_warriorsController.Spawn ();
		}

		#endregion

		#region Init

		private void Init ()
		{
			InitComponents ();
			InitShaper ();
		}

		private void InitShaper ()
		{
			var shaperData = new BiomShaperData ();
			shaperData.GrowthSpeed = GrowthCoeficient;
			shaperData.Owner = this;
			shaperData.Padding = Padding;
			shaperData.Plane = Plane;

			_shaper = new BiomShaper (shaperData);
			_biomeComponents.Add (_shaper);
		}

		private void InitComponents ()
		{
			_biomeComponents = GetComponentsInChildren<IBiomeComponent> ().ToList ();
			for (int i = 0; i < _biomeComponents.Count; i++)
			{
				_biomeComponents [i].InitComponent (this);
			}
			_slavesController = GetComponentInChildren<BiomSlavesController> ();
			_warriorsController = GetComponentInChildren<BiomWarriorsController> ();
		}

		#endregion

		#region Collision

		private void OnCollidedWithBiome (BiomBase collidedWith)
		{
			BonusDeliveryArea.enabled = false;
			_shaper.ResetScale ();
			if (_canCollide && collidedWith.BiomPower > BiomPower)
			{
				
				BiomPower -= 20;
				_canCollide = false;

				Invoke ("EnableCollision", 2f);
			}
		}

		private void EnableCollision ()
		{
			_canCollide = true;
			BonusDeliveryArea.enabled = true;
		}

		#endregion

		#region Internal

		protected abstract void PerformTerrainGeneration ();

		private void IncreasePower ()
		{
			BiomPower++;
		}

		#endregion
	}
}
