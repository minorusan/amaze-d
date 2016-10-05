using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;

namespace Core.Map.TerrainGeneration
{
    public class TerrainGenerator
    {
         

        public static void GenerateTerrainForMap(Node[,] mapRepresentation)
        {
            Terrain terrain = GameObject.FindObjectOfType<Terrain>();

            float[,] heights = new float[mapRepresentation.GetLength(0), mapRepresentation.GetLength(1)];

            for (int i = 0; i < mapRepresentation.GetLength(0); i++)
            {
                for (int j = 0; j < mapRepresentation.GetLength(1); j++)
                {
                    heights[i, j] = mapRepresentation[i, j].CurrentCellType == ECellType.Blocked ? Random.Range(0.2f, 1f) : 0.1f;
                }
            }

            terrain.terrainData.size = new Vector3(mapRepresentation.GetLength(0), 10f, mapRepresentation.GetLength(0)); 
            terrain.terrainData.heightmapResolution = mapRepresentation.GetLength(0); 
            terrain.terrainData.SetHeights(0, 0, heights);
        }
    }
}

