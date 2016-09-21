using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.Movement;


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
		private List<BasicCell> _cellsInGame;

		#endregion

		[Header ("Dimensions")]
		public IJ MapDimentions;
		public Vector2 CellSize;

		public Transform StartPoint;
		public GameObject CellPrefab;

		
		// Use this for initialization
		void Start ()
		{
			var t = GameObject.FindGameObjectsWithTag ("CELL");
			_cellsInGame = new List<BasicCell> ();
			for (int i = 0; i < t.Length; i++)
			{
				_cellsInGame.Add (t [i].GetComponent <BasicCell> ());
			}

			foreach (var item in _cellsInGame)
			{
				item.InitWithNode (new Node ());
			}
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
			Debug.LogWarning ("MapGenerator::TEMP NODE CONFIGURATION! SEE LINE 76");
			for (int i = 0; i < MapDimentions.I; i++)
			{
				
				for (int j = 0; j < MapDimentions.J; j++)
				{
					var instantiated = Instantiate (CellPrefab);
					instantiated.transform.SetParent (this.transform);
					instantiated.transform.localPosition = currentPosition;
					currentPosition = new Vector3 (currentPosition.x + CellSize.x, currentPosition.y, currentPosition.z);

					instantiated.GetComponent <BasicCell> ().InitWithNode (new Node ());


					_currentCellsArray.Add (instantiated);
				}
				currentPosition = new Vector3 (StartPoint.localPosition.x, currentPosition.y, currentPosition.z + CellSize.y);
			}


		}

	

	
	}

}

