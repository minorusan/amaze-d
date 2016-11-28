using UnityEngine;
using System.Collections;
using Core.Map.TerrainGeneration;


namespace Core.Bioms
{
    public class StormBiom : BiomBase
    {
        private PerlinNoise _algorithm;

        protected override void Start()
        {
            
            _algorithm = new PerlinNoise();
            //MeshHelper.Subdivide9(Plane.GetComponent<MeshFilter>().sharedMesh);
            base.Start();
            BiomType = EBiomType.Storm;
        }

        protected override void PerformTerrainGeneration()
        {
            CurrentMap = _algorithm.GetHeightMap((int)_shaper.MapLength);
            _shaper.GenerateTerrain();
        }
    }
}
