using System;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Map
{
    public class MapHelpers
    {
        public static float GetHeightWorldCoords(TerrainData terrainData, Vector2 point)
        {
            Vector3 scale = terrainData.heightmapScale;
            return (float)terrainData.GetHeight((int)(point.x / scale.x), (int)(point.y / scale.z));
        }
    }

    public enum ECellType
    {
        Walkable = 0,
        Blocked = 1,
        Busy,
        Player
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var mapGenerator = (MapGenerator)target;
            if (GUILayout.Button("Generate map"))
            {
                mapGenerator.InstantiateCells();
            }
        }
    }
    #endif
}

