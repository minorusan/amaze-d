using System;
using Core.Bioms;
using Core.Interactivity.AI;
using UnityEngine;
using System.ComponentModel.Design.Serialization;
using Gameplay;

namespace Core.Bioms.BiomeComponents
{
    public class BiomeNPCController:MonoBehaviour, IBiomeComponent
    {
        private BiomBase _ownerBiom;
        protected ArtificialIntelligence[] _registeredNPCs;

        public int Cost;

        public void InitComponent(BiomBase owner)
        {
            _ownerBiom = owner;
        }

        public void UpdateComponent()
        {
            if (!_ownerBiom.ControlledByPlayer)
            {
                Spawn();
            }
        }

        protected virtual void Awake()
        {
            SetAllSlavesActive(false);
        }

        public void Spawn()
        {
            if (_ownerBiom.BiomPower >= Cost)
            {
                for (int i = 0; i < _registeredNPCs.Length; i++)
                {
                    if (!_registeredNPCs[i].isActiveAndEnabled)
                    {
                        var node = Game.Instance.CurrentMap.GetWalkableBiomNode(_ownerBiom.BiomType);
                        if (node != null)
                        {
                            _registeredNPCs[i].AllowsUpdates = true;
                            _registeredNPCs[i].gameObject.SetActive(true);
                            _registeredNPCs[i].transform.position = node.Position;
                            _ownerBiom.BiomPower -= Cost;
                        }
                        break;
                    }
                }
            }
        }

        private void SetAllSlavesActive(bool active)
        {
            for (int i = 0; i < _registeredNPCs.Length; i++)
            {
                _registeredNPCs[i].gameObject.SetActive(active);
            }
        }
    }
}

