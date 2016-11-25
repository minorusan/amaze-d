using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.AI.Brains;
using Gameplay;

namespace Core.Bioms
{
    #warning Create abstract base class

    public class BiomWarriorsController
    {
        private BiomBase _ownerBiom;
        private WarriorBrains[] _registeredWarriors;

        public BiomWarriorsController(BiomBase owner)
        {
            _ownerBiom = owner;
            _registeredWarriors = _ownerBiom.GetComponentsInChildren<WarriorBrains>();
            SetAllWarriorsActive(false);
        }

        public void Spawn()
        {
            if (_ownerBiom.BiomPower >= 20)
            {
                for (int i = 0; i < _registeredWarriors.Length; i++)
                {
                    if (!_registeredWarriors[i].isActiveAndEnabled)
                    {
                        _registeredWarriors[i].gameObject.SetActive(true);
                        _registeredWarriors[i].transform.position = _ownerBiom.bonusDeliveryPoint.position;
                        _ownerBiom.BiomPower -= 20;
                        break;
                    }
                }
            }
        }

        private void SetAllWarriorsActive(bool active)
        {
            for (int i = 0; i < _registeredWarriors.Length; i++)
            {
                _registeredWarriors[i].gameObject.SetActive(active);
            }
        }
    }
}
