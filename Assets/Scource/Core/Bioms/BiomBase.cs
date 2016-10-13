using UnityEngine;



namespace Core.Bioms
{
    public class BiomBase : MonoBehaviour
    {
        #region PRIVATE

        private float _timeToGrow;
        protected BiomShaper _shaper;
        private int _power = 1;
        public Material _material;


        #endregion

        [Header("Basic biome settings")]
        public float ScaleCoeficient;
        public float Padding = 3.5f;
        public GameObject Plane;
        public Collider SpawnArea;


        public float GrowthTime = 3;
        public float GrowthSpeed = 3;

        [Header("Biome-specific")]
        public string BiomSpawnsTag;
        public bool UsePerlinNoise;
        public float[,] CurrentMap;


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

        }

        protected virtual void Update()
        {
            DefineGrowthProgress();
        }

        protected virtual void PerformTerrainGeneration()
        {
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



        private void DefineGrowthProgress()
        {
            if (_timeToGrow < 0)
            {
                BiomPower++;
                _timeToGrow = GrowthTime;
            }
            else
            {
                _timeToGrow -= Time.deltaTime;
                _shaper.UpdateShape();
            }
        }
    }
}
