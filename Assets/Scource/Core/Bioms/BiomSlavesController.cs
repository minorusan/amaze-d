using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.AI.Brains;
using Gameplay;

namespace Core.Bioms
{
    #warning Create abstract base class

    public class BiomSlavesController
    {
        private BiomBase _ownerBiom;
        private SlaveBrains[] _registeredSlaves;

        public BiomSlavesController(BiomBase owner)
        {
            _ownerBiom = owner;
            _registeredSlaves = _ownerBiom.GetComponentsInChildren<SlaveBrains>();
            SetAllSlavesActive(false);
        }

        public void Spawn()
        {
            if (_ownerBiom.BiomPower >= 10)
            {
                for (int i = 0; i < _registeredSlaves.Length; i++)
                {
                    if (!_registeredSlaves[i].isActiveAndEnabled)
                    {
                        _registeredSlaves[i].gameObject.SetActive(true);
                        _registeredSlaves[i].transform.position = _ownerBiom.bonusDeliveryPoint.position;
                        _ownerBiom.BiomPower -= 10;
                        break;
                    }
                }
            }
        }

        private void SetAllSlavesActive(bool active)
        {
            for (int i = 0; i < _registeredSlaves.Length; i++)
            {
                _registeredSlaves[i].gameObject.SetActive(active);
            }
        }
    }
}

