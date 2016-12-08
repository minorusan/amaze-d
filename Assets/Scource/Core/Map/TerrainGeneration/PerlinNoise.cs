using UnityEngine;
using System.Collections;
using Core.Map.TerrainGeneration;
using System;


namespace Core.Map.TerrainGeneration
{
	public class PerlinNoise : IHeightMapAlgorithm
	{
		private float _power = 0.5f;

		public float[,] GetHeightMap (int resolution)
		{
			var heightMap = new float[resolution, resolution];
			var v2SampleStart = new Vector2 (UnityEngine.Random.Range (0.0f, 100.0f), UnityEngine.Random.Range (0.0f, 100.0f));
			for (int i = 0; i < resolution; i++)
			{  
				for (int j = 0; j < resolution; j++)
				{
					float xCoord = v2SampleStart.x + i;
					float yCoord = v2SampleStart.y + j;
					heightMap [i, j] = Noise.Generate (xCoord, yCoord) / _power; 
				}
			}
			_power = Mathf.Clamp (_power - 0.001f, 0.01f, 0.3f);

			return heightMap;
		}
	}
}

