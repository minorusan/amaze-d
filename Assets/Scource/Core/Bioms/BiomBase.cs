using UnityEngine;
using System.Collections;
using Core.Map.TerrainGeneration;
using Core.Map;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Collections.Generic;


namespace Core.Bioms
{
    public class BiomBase : MonoBehaviour
    {
        #region PRIVATE

        private float _timeToGrow;
        protected BiomShaper _shaper;


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
        public float[,] CurrentMap;


        public int BiomPower
        {
            get;
            set;
        }

        public BiomShaper Shaper
        {
            get
            {
                return _shaper;
            }
        }

        public void AddBiomPower(int powerToAdd)
        {
            BiomPower += powerToAdd;
        }

        #region MONOBEHAVIOUR

        protected virtual void Awake()
        {
            InitShaper();
        }

        protected virtual void Start()
        {
            _timeToGrow = GrowthTime;
            PerformTerrainGeneration();
        }

        protected virtual void Update()
        {
            DefineGrowthProgress();
        }

        protected virtual void PerformTerrainGeneration()
        {
            BiomPower++;
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
                
                PerformTerrainGeneration();
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
