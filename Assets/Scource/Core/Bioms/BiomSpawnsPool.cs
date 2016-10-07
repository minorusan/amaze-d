using UnityEngine;
using System.Collections;

namespace Core.Bioms
{
    public class BiomSpawnsPool : MonoBehaviour
    {
        private BiomSpawn[] _biomSpawns;

        public BiomBase Owner;

        // Use this for initialization
        void Start()
        {
            GetSpawns();
        }

        // Update is called once per frame
        void Update()
        {
            CheckSpawns();
        }

        private void CheckSpawns()
        {
            if (_biomSpawns != null && _biomSpawns.Length > 0)
            {
                for (int i = 0; i < _biomSpawns.Length; i++)
                {
                    if (IsPowerConditionSatisfied(_biomSpawns[i]) && !_biomSpawns[i].isActiveAndEnabled)
                    {
                        _biomSpawns[i].gameObject.SetActive(true);
                        _biomSpawns[i].InitialScale = new Vector3(_biomSpawns[i].InitialScale.x + (float)Owner.BiomPower / 1000,
                            _biomSpawns[i].InitialScale.y + (float)Owner.BiomPower / 1000,
                            _biomSpawns[i].InitialScale.z + (float)Owner.BiomPower / 1000);
                        _biomSpawns[i].transform.position = _biomSpawns[i].RandomizeLocation ? Owner.Shaper.GetRandomPosition(Owner.SpawnArea) : _biomSpawns[i].transform.position; 
                    }
                }
            }

        }

        private bool IsPowerConditionSatisfied(BiomSpawn _biomsSpawn)
        {
            return _biomsSpawn.RequiredPower <= Owner.BiomPower && _biomsSpawn.LimitPower >= Owner.BiomPower;
        }

        private void GetSpawns()
        {
            _biomSpawns = new BiomSpawn[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                
                _biomSpawns[i] = transform.GetChild(i).GetComponent<BiomSpawn>();
            }
        }
    }
}

