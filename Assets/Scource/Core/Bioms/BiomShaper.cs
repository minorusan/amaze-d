using System;
using UnityEngine;
using Gameplay;
using Core.Map;


namespace Core.Bioms
{
	public class BiomShaper
	{
		private readonly GameObject _plane;
		private readonly BiomBase _owner;
		private readonly MeshFilter _planeMeshFinter;
		private readonly MeshRenderer _surfaceFilter;
		private readonly MeshCollider _selfCollider;


		private int _whenToGrowSubDivision = 100;
		private float _selfMapLength;
		private Terrain terrain;

		private Vector3 _newScale;
		private MapGenerator _mapGeneratorInstance;
		private Vector3[] _requiredVertices;
		private Vector3[] _currentVertices;


		public Vector3[] BiomVerticePositions {
			get
			{
				if (_requiredVertices != null)
				{
					var positions = new Vector3[_requiredVertices.Length];

					for (int i = 0; i < positions.Length; i++)
					{
						positions [i] = _owner.Plane.transform.position + _owner.Plane.transform.TransformPoint (_requiredVertices [i]);
					}
					return positions;
				}
				return null;
			}
		}

		public float Padding {
			get;
			set;
		}

		public float GrowthSpeed {
			get;
			set;
		}

		public float MapLength {
			get
			{
				return _selfMapLength;
			}
		}


		public BiomShaper (BiomShaperData data)
		{
			_plane = data.Plane;
			_owner = data.Owner;
			_surfaceFilter = data.Owner.Surface.GetComponent <MeshRenderer> ();
			terrain = GameObject.FindObjectOfType<Terrain> ();
			_selfCollider = _plane.GetComponent <MeshCollider> ();
			_planeMeshFinter = _plane.GetComponent <MeshFilter> ();
			_selfMapLength = Mathf.Sqrt (_planeMeshFinter.mesh.vertices.Length);
			_mapGeneratorInstance = GameObject.FindObjectOfType<MapGenerator> ();

			Padding = data.Padding;
			GrowthSpeed = data.GrowthSpeed;
		}

		public Vector3 GetRandomPosition (Collider col)
		{
			var bounds = col.bounds;
			var center = bounds.center;

			var x = UnityEngine.Random.Range (center.x - bounds.extents.x, center.x + bounds.extents.x);
			var z = UnityEngine.Random.Range (center.z - bounds.extents.z, center.z + bounds.extents.z);

			return new Vector3 (x, 0f, z);
         
		}

		public void GenerateTerrain ()
		{
			ApplyNewHeightMap ();
         
			_newScale = new Vector3 (1f + (float)_owner.BiomPower / 10,
			                         1f,
			                         1f + (float)_owner.BiomPower / 10);
		}

		public void UpdateShape ()
		{
			if (_requiredVertices == null)
			{
				return;
			}
			if (_owner.BiomPower % _whenToGrowSubDivision == 0)
			{
				MeshHelper.Subdivide4 (_planeMeshFinter.mesh);
				_whenToGrowSubDivision *= 18;
				Debug.Log ("Biom::Subdivided!");
			}

			_plane.transform.localScale = Vector3.MoveTowards (_plane.transform.localScale,
			                                                   _newScale,
			                                                   GrowthSpeed);
			if (_owner.BiomPower >= 1)
			{
				ApplyHeightsAsync ();
			}


		}

		public void UpdateMap ()
		{
			_mapGeneratorInstance.UpdateBiomVerticeNode (BiomVerticePositions, EBiomType.Fel, (int)_surfaceFilter.bounds.size.x);
		}

		private void ApplyNewHeightMap ()
		{
			Vector3[] vertices = _planeMeshFinter.mesh.vertices;
			_currentVertices = vertices;
			_requiredVertices = new Vector3[vertices.Length];
			vertices.CopyTo (_requiredVertices, 0);
			var iterator = 0;
			Vector2 startPoint = new Vector2 (0f, 0f);
			for (int i = 0; i < _selfMapLength; i++)
			{
				for (int j = 0; j < _selfMapLength; j++)
				{
					if ((vertices [iterator].x < Padding && vertices [iterator].x > -Padding) &&
					    (vertices [iterator].z < Padding && vertices [iterator].z > -Padding))
					{
						float x = startPoint.x + vertices [i].x * 1f; 
						float z = startPoint.y + vertices [i].z * 1f; 

						_requiredVertices [iterator].y = _owner.UsePerlinNoise ? (Mathf.PerlinNoise (x, z) - 0.5f) * 3f :
							_owner.CurrentMap [i, j];
					}
					iterator++;
				}
			}
           
		}

		private void ApplyHeightsAsync ()
		{
			if (_requiredVertices == null)
			{
				return;
			}

			for (int i = 0; i < _requiredVertices.Length; i++)
			{
				_currentVertices [i] = Vector3.MoveTowards (_currentVertices [i], _requiredVertices [i], Time.deltaTime);
			}
			if (_currentVertices.Length == _planeMeshFinter.mesh.vertices.Length)
			{
				_planeMeshFinter.mesh.vertices = _currentVertices;
				_planeMeshFinter.mesh.RecalculateBounds (); 
				_planeMeshFinter.mesh.RecalculateNormals (); 
				_selfCollider.sharedMesh = _planeMeshFinter.mesh;
			}
		


		}

	}
}

