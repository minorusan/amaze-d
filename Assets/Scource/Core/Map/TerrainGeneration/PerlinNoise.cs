using UnityEngine;
using System.Collections;
using Core.Map.TerrainGeneration;
using System;


namespace Core.Map.TerrainGeneration
{
    public class PerlinNoise : IHeightMapAlgorithm
    {
        private float _power = 0.5f;

        public float[,] GetHeightMap(int resolution)
        {
            var heightMap = new float[resolution, resolution];
            for (int i = 0; i < resolution; i++)
            {  
                for (int j = 0; j < resolution; j++)
                {
                    heightMap[i, j] = Noise.Generate(i, j) / _power; 
                }
            }
            _power = Mathf.Clamp(_power - 0.005f, 0.01f, 0.9f);

            return heightMap;
        }
    }
}

