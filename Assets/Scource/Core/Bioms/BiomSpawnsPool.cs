using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using Gameplay;
using Core.Interactivity.AI;


namespace Core.Bioms
{
    public class BiomSpawnsPool : MonoBehaviour
    {
        private BiomSpawn[] _biomSpawns;

        public BiomBase Owner;

        void Start()
        {
            GetSpawns();
        }

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
                    if (_biomSpawns[i] != null && IsPowerConditionSatisfied(_biomSpawns[i]) && !_biomSpawns[i].isActiveAndEnabled)
                    {
                        _biomSpawns[i].gameObject.SetActive(true);
                       
                        _biomSpawns[i].InitialScale = new Vector3(_biomSpawns[i].InitialScale.x + (float)Owner.BiomPower / 1000,
                            _biomSpawns[i].InitialScale.y + (float)Owner.BiomPower / 1000,
                            _biomSpawns[i].InitialScale.z + (float)Owner.BiomPower / 1000);
                        
                        var node = new Node();
                        _biomSpawns[i].transform.position = _biomSpawns[i].RandomizeLocation ? Owner.Shaper.GetRandomPosition(Owner.SpawnArea) : _biomSpawns[i].transform.position; 
                        node = Game.Instance.CurrentMap.GetNodeByPosition(_biomSpawns[i].transform.position);
                    }

                    if (_biomSpawns[i] != null && !IsPowerConditionSatisfied(_biomSpawns[i]))
                    {
                        _biomSpawns[i].DeactivateSpawn();
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
            _biomSpawns = GetComponentsInChildren<BiomSpawn>();
            for (int i = 0; i < _biomSpawns.Length; i++)
            {
                _biomSpawns[i].gameObject.SetActive(false);
            }
        }
    }
}

