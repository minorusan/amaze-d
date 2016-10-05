using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using Core.Interactivity.Movement;


namespace Core.Map.TerrainGeneration
{
	public enum ETerrainGenerationAlgrorithm
	{
		DiamondSquare
	}

	[RequireComponent (typeof(Terrain))]
	public class TerrainController : MonoBehaviour
	{
		private float[,] _heightMap;
		private Terrain _selfTerrain;


		private float[,] hights;
		private float _maxHeight = 0.2f;

		public int Dimension = 500;


		// Use this for initialization
		void Start ()
		{
			_selfTerrain = GetComponent <Terrain> ();
			_heightMap = new float[Dimension, Dimension];
		}



		public void ApplyHeightMap (float[,] heightMap, IJ offsets, int resolutionCoef)
		{
			_selfTerrain.terrainData.size = new Vector3 (Dimension, _maxHeight, Dimension);
			_selfTerrain.terrainData.heightmapResolution = Dimension;
			for (int i = 0; i < heightMap.GetLength (0); i++)
			{
				for (int j = 0; j < heightMap.GetLength (0); j++)
				{
					_heightMap [i + offsets.I, j + offsets.J] = heightMap [i, j];
				}
			}


			_selfTerrain.terrainData.SetHeightsDelayLOD (0, 0, _heightMap);
			_maxHeight += 0.1f;
		}
	}
}

