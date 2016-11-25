using UnityEngine;
using System;
using System.Collections;



namespace Core.Bioms
{
    public class BiomBase : MonoBehaviour
    {
        #region PRIVATE

        private float _timeToGrow;
        protected BiomShaper _shaper;
        private int _power = 1;
        private bool _canCollide = true;
        private BiomCollisionDetector _collisionDetector;
        private BiomSlavesController _slavesController;
        private BiomWarriorsController _warriorsController;
        public Material _material;


        #endregion

        [Header("Basic biome settings")]
        public Transform bonusDeliveryPoint;

        public bool ControlledByPlayer;

        public float ScaleCoeficient;
        public float Padding = 3.5f;
        public GameObject Plane;
        public GameObject Surface;
        public Collider SpawnArea;

        public float GrowthTime = 3;
        public float GrowthSpeed = 3;

        [Header("Biome-specific")]
        public string BiomSpawnsTag;
        public bool UsePerlinNoise;
        public float[,] CurrentMap;

        public EBiomType BiomType;

        public int BiomPower
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
                PerformTerrainGeneration();
            }
        }

        public BiomShaper Shaper
        {
            get
            {
                return _shaper;
            }
        }

        #region MONOBEHAVIOUR

        protected virtual void Awake()
        {
            _timeToGrow = GrowthTime;
        }

        protected virtual void Start()
        {
            InitShaper();

            _slavesController = new BiomSlavesController(this);
            _warriorsController = new BiomWarriorsController(this);
            _collisionDetector = GetComponentInChildren<BiomCollisionDetector>();
            _collisionDetector.CollidedWithBiome += OnCollidedWithBiome;
            StartCoroutine(DefineGrowProgress());
            StartCoroutine(UpdateShaper());
        }

        void OnCollidedWithBiome(BiomBase collidedWith)
        {
            if (_canCollide)
            {
                BiomPower += collidedWith.BiomPower > BiomPower ? -10 : 3;
                _canCollide = false;
                Invoke("EnableCollision", 2f);
                Debug.Log(this.name + " lost " + 10 + " biome power.");
            }
        }

        void EnableCollision()
        {
            _canCollide = true;
        }

        protected virtual void PerformTerrainGeneration()
        {
        }

        protected void Update()
        {
            if (!ControlledByPlayer)
            {
                _warriorsController.Spawn();
                _slavesController.Spawn();
            }
        }

        #endregion

        private void InitShaper()
        {
            var shaperData = new BiomShaperData();
            shaperData.GrowthSpeed = GrowthSpeed;
            shaperData.Owner = this;
            shaperData.Padding = Padding;
            shaperData.Plane = Plane;

            _shaper = new BiomShaper(shaperData);
        }

        public void UpdateBiomMap()
        {
            _shaper.UpdateMap();
        }

        private IEnumerator DefineGrowProgress()
        {
            while (true)
            {
                BiomPower++;

                yield return new WaitForSeconds(GrowthTime);
            }
        }

        private IEnumerator UpdateShaper()
        {
            while (true)
            {
                _shaper.UpdateShape();
                yield return new WaitForFixedUpdate();
            }
        }

        public void SpawnSlave()
        {
            _slavesController.Spawn();
        }

        public void SpawnWarrior()
        {
            _warriorsController.Spawn();
        }
    }
}
