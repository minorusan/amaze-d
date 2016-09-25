using System;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Map
{
	public enum ECellType
	{
		Walkable = 0,
		Blocked = 1,
		Busy,
		Target,
		Player,
	}

	#if UNITY_EDITOR
	[CustomEditor (typeof(MapGenerator))]
	public class MapGeneratorEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			var mapGenerator = (MapGenerator)target;
			if (GUILayout.Button ("Generate map"))
			{
				mapGenerator.InstantiateCells ();
			}

			if (GUILayout.Button ("Generate obstacles"))
			{
				mapGenerator.GenerateObstacles ();
			}
		}
	}


	#endif
}

