using UnityEngine;
using System.Collections;
using Core.Map.TerrainGeneration;
using Core.Map;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Collections.Generic;


namespace Core.Bioms
{
	public class BiomBase : MonoBehaviour
	{
		#region PROTECTED

		protected MeshFilter _selfMeshFiler;
		protected float _selfMapLength;
		protected float[,] _currentMap;

		#endregion

		#region PRIVATE

		private float _timeToGrow;
		private BiomSpawn[] _biomSpawns;
		private Vector3[] _requiredVertices;
		private Vector3[] _currentVertices;
		private int _whenToGrowSubDivision = 24;
		private int iterations;
		private MeshCollider _selfCollider;
		private Vector3 _newScale;

		#endregion

		[Header ("Basic biome settings")]
		public float ScaleCoeficient;
		public float Padding = 3.5f;

		public float GrowthTime = 3;
		public float GrowthSpeed = 3;

		[Header ("Biome-specific")]
		public string BiomSpawnsTag;



		public int Iterations {
			get
			{
				return iterations;
			}
		}

		#region MONOBEHAVIOUR

		protected virtual void Awake ()
		{
			GetSpawns ();
		}

		protected virtual void Start ()
		{
			
			_selfMeshFiler = GetComponent<MeshFilter> (); 

			_selfCollider = GetComponent <MeshCollider> ();
			_selfMapLength = Mathf.Sqrt (_selfMeshFiler.mesh.vertices.Length);
			_timeToGrow = GrowthTime;
			GenerateTerrain ();

		}

		protected virtual void Update ()
		{
			DefineGrowthProgress ();
			CheckSpawns ();
		}

		#endregion

		protected virtual void GenerateTerrain ()
		{
			ApplyNewHeightMap ();
			_newScale = new Vector3 (transform.localScale.x + ScaleCoeficient,
			                         1f,
			                         transform.localScale.z + ScaleCoeficient);
			_timeToGrow = GrowthTime;
		}

		private void ApplyNewHeightMap ()
		{
			Vector3[] vertices = _selfMeshFiler.mesh.vertices;
			_currentVertices = vertices;
	
			_requiredVertices = new Vector3[vertices.Length];
			vertices.CopyTo (_requiredVertices, 0);
			var iterator = 0;
			for (int i = 0; i < _selfMapLength; i++)
			{
				for (int j = 0; j < _selfMapLength; j++)
				{
					if ((vertices [iterator].x < Padding && vertices [iterator].x > -Padding) &&
					    (vertices [iterator].z < Padding && vertices [iterator].z > -Padding))
					{
						_requiredVertices [iterator].y = _currentMap [i, j];
					}
					iterator++;
				}
			}
		}

		private void ApplyHeightsAsync ()
		{
			for (int i = 0; i < _requiredVertices.Length; i++)
			{
				_currentVertices [i] = Vector3.MoveTowards (_currentVertices [i], _requiredVertices [i], Time.deltaTime);
			}
			if (_currentVertices.Length == _selfMeshFiler.mesh.vertices.Length)
			{
				_selfMeshFiler.mesh.vertices = _currentVertices;
				_selfMeshFiler.mesh.RecalculateBounds (); 
				_selfMeshFiler.mesh.RecalculateNormals (); 
				_selfCollider.sharedMesh = _selfMeshFiler.mesh;
			}
		}

		private void GetSpawns ()
		{
			if (!string.IsNullOrEmpty (BiomSpawnsTag))
			{
				var spawnsGOs = GameObject.FindGameObjectsWithTag (BiomSpawnsTag);
				_biomSpawns = new BiomSpawn[spawnsGOs.Length];

				for (int i = 0; i < _biomSpawns.Length; i++)
				{
					_biomSpawns [i] = spawnsGOs [i].GetComponent <BiomSpawn> ();
					_biomSpawns [i].gameObject.SetActive (false);
				}
			}

		}

		private void DefineGrowthProgress ()
		{
			if (_timeToGrow < 0)
			{
				GenerateTerrain ();
				iterations++;
				if (iterations % _whenToGrowSubDivision == 0)
				{
					MeshHelper.Subdivide4 (_selfMeshFiler.mesh);
					_whenToGrowSubDivision *= 5;
					Debug.Log ("Biom::Subdivided!");
				}
			}
			else
			{
				_timeToGrow -= Time.deltaTime;
				transform.localScale = Vector3.MoveTowards (transform.localScale,
				                                            _newScale,
				                                            Time.deltaTime * GrowthSpeed);
				if (iterations >= 1)
				{
					ApplyHeightsAsync ();
				}
			}
		}

		private void CheckSpawns ()
		{
			if (_biomSpawns != null && _biomSpawns.Length > 0)
			{
				for (int i = 0; i < _biomSpawns.Length; i++)
				{
					if (_biomSpawns [i].StartIteration <= Iterations && _biomSpawns [i].EndIteration >= Iterations)
					{
						_biomSpawns [i].gameObject.SetActive (true);
					}

					if (_biomSpawns [i].EndIteration <= Iterations && _biomSpawns [i].isActiveAndEnabled)
					{
						_biomSpawns [i].DeactivateSpawn ();
					}
				}
			}

		}
	}
}
