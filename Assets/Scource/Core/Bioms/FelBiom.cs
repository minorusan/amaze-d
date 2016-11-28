using UnityEngine;
using System.Collections;
using Core.Map.TerrainGeneration;


namespace Core.Bioms
{
    public class FelBiom : BiomBase
    {
        private DiamondSquareTerrain _algorithm;

        protected override void Start()
        {
            _algorithm = new DiamondSquareTerrain();
            base.Start();
            BiomType = EBiomType.Fel;
        }

        protected override void PerformTerrainGeneration()
        {
            CurrentMap = _algorithm.GetHeightMap((int)_shaper.MapLength);
            _shaper.GenerateTerrain();
        }
    }
}
