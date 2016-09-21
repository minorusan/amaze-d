using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


namespace Core.Map
{
	[Serializable]
	public struct IJ
	{
		public int I;
		public int J;
	}





	public class MapGenerator : MonoBehaviour
	{
		#region PRIVATE

		private List<GameObject> _currentCellsArray;

		#endregion

		[Header ("Dimensions")]
		public IJ MapDimentions;
		public Vector2 CellSize;

		public Transform StartPoint;
		public GameObject CellPrefab;

		
		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{

		}

		public void InstantiateCells ()
		{
			if (_currentCellsArray == null)
			{
				_currentCellsArray = new List<GameObject> ();
			}
			else
			{
				foreach (var cell in _currentCellsArray)
				{
					DestroyImmediate (cell);
				}
			}

			var currentPosition = StartPoint.localPosition;
			for (int i = 0; i < MapDimentions.I; i++)
			{
				
				for (int j = 0; j < MapDimentions.J; j++)
				{
					var instantiated = Instantiate (CellPrefab);
					instantiated.transform.SetParent (this.transform);
					instantiated.transform.localPosition = currentPosition;
					currentPosition = new Vector3 (currentPosition.x + CellSize.x, currentPosition.y, currentPosition.z);

					_currentCellsArray.Add (instantiated);
				}
				currentPosition = new Vector3 (StartPoint.localPosition.x, currentPosition.y, currentPosition.z + CellSize.y);
			}
		}

	
	}

}

