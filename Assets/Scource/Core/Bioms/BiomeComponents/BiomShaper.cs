using System;
using UnityEngine;
using Gameplay;
using Core.Map;
using System.Runtime.InteropServices;
using Core.Bioms.BiomeComponents;


namespace Core.Bioms
{
    public struct BiomVertice
    {
        public Vector3 LocalPosition;
        public Vector3 WorldPosition;
    }

    public class BiomShaper : IBiomeComponent
    {
        #region PRIVATE

        private readonly GameObject _plane;
        private BiomBase _owner;

        private readonly MeshFilter _planeMeshFinter;
        private readonly MeshRenderer _surfaceFilter;
        private readonly MeshCollider _selfCollider;

        private float _selfMapLength;

        private MapGenerator _mapGeneratorInstance;
        private Vector3[] _requiredVertices;
        private Vector3[] _currentVertices;
        private Vector3 _newScale;

        #endregion

        public BiomVertice[] BiomVerticePositions
        {
            get
            {
                if (_requiredVertices != null)
                {
                    var positions = new BiomVertice[_requiredVertices.Length];

                    for (int i = 0; i < positions.Length; i++)
                    {
                        positions[i] = new BiomVertice();
                        positions[i].WorldPosition = _owner.Plane.transform.TransformPoint(_requiredVertices[i]);
                        positions[i].LocalPosition = _requiredVertices[i];
                    }
                    return positions;
                }
                return null;
            }
        }

        public float Padding
        {
            get;
            set;
        }

        public float GrowthSpeed
        {
            get;
            set;
        }

        public float MapLength
        {
            get
            {
                return _selfMapLength;
            }
        }

        public void InitComponent(BiomBase owner)
        {
            _owner = owner;
        }

        public BiomShaper(BiomShaperData data)
        {
            _plane = data.Plane;
            _owner = data.Owner;
            _surfaceFilter = data.Owner.Surface.GetComponent <MeshRenderer>();
            _selfCollider = _plane.GetComponent <MeshCollider>();
            _planeMeshFinter = _plane.GetComponent <MeshFilter>();
            _selfMapLength = Mathf.Sqrt(_planeMeshFinter.mesh.vertices.Length);
            _mapGeneratorInstance = GameObject.FindObjectOfType<MapGenerator>();

            Padding = data.Padding;
            GrowthSpeed = data.GrowthSpeed;
        }

        public Vector3 GetRandomPosition(Collider col)
        {
            var bounds = col.bounds;
            var center = bounds.center;

            var x = UnityEngine.Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
            var z = UnityEngine.Random.Range(center.z - bounds.extents.z, center.z + bounds.extents.z);

            return new Vector3(x, 0f, z);
        }

        public void GenerateTerrain()
        {
            ApplyNewHeightMap();
         
            _newScale = new Vector3(1f + _owner.BiomPower * 0.1f,
                1f,
                1f + _owner.BiomPower * 0.1f);
        }

        public void UpdateComponent()
        {
            if (_requiredVertices == null)
            {
                return;
            }

            _plane.transform.localScale = Vector3.MoveTowards(_plane.transform.localScale,
                _newScale,
                GrowthSpeed);
            
            if (_owner.BiomPower >= 1)
            {
                ApplyHeightsAsync();
            }

            UpdateMap();
            _selfMapLength = Mathf.Sqrt(_planeMeshFinter.mesh.vertices.Length);
        }

        public void UpdateMap()
        {
            _mapGeneratorInstance.UpdateBiomVerticeNode(BiomVerticePositions, _owner.BiomType, (int)_surfaceFilter.bounds.size.x); 
        }

        private void ApplyNewHeightMap()
        {
            Vector3[] vertices = _planeMeshFinter.mesh.vertices;
            _currentVertices = vertices;
            _requiredVertices = new Vector3[vertices.Length];
            vertices.CopyTo(_requiredVertices, 0);
            var iterator = 0;
            for (int i = 0; i < _selfMapLength; i++)
            {
                for (int j = 0; j < _selfMapLength; j++)
                {
                    if ((vertices[iterator].x < Padding && vertices[iterator].x > -Padding) &&
                        (vertices[iterator].z < Padding && vertices[iterator].z > -Padding))
                    {
                        _requiredVertices[iterator].y = _owner.CurrentMap[i, j];
                    }
                    iterator++;
                }
            }
        }

        private void ApplyHeightsAsync()
        {
            if (_requiredVertices == null)
            {
                return;
            }

            for (int i = 0; i < _requiredVertices.Length; i++)
            {
                _currentVertices[i] = Vector3.MoveTowards(_currentVertices[i], _requiredVertices[i], Time.deltaTime);
            }
            if (_currentVertices.Length == _planeMeshFinter.mesh.vertices.Length)
            {
                _planeMeshFinter.mesh.vertices = _currentVertices;
                _planeMeshFinter.mesh.RecalculateBounds(); 
                _planeMeshFinter.mesh.RecalculateNormals(); 
                _selfCollider.sharedMesh = _planeMeshFinter.mesh;
            }
        }

    }
}

