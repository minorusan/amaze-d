using System;
using Core.Interactivity.AI;
using Core.Interactivity.AI.AIStates;
using UnityEngine;
using Core.Bioms;
using System.Linq.Expressions;
using Gameplay;
using Core.Interactivity.Movement;
using Core.Gameplay;


namespace Core.Interactivity.AI.Brains
{
    public class SlaveBrains:ArtificialIntelligence
    {
        private BasicBonus _crate;
        private ReferenceStorage _referenceStorage;

        public GameObject BonusDeliveryPoint;
        public EBiomType OwnerBiome;

        public bool CratePickedUp
        {
            get;
            private set;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_referenceStorage == null && Game.Instance.CurrentSession != null)
            {
                _referenceStorage = Game.Instance.ReferenceStorage;
            }

            if (_referenceStorage != null)
            {
                _referenceStorage.RegisterSlave(this, OwnerBiome);
            }
           
            CratePickedUp = false;
        }

        private void OnDisable()
        {
            if (_referenceStorage != null)
            {
                _referenceStorage.UnregisterSlave(this, OwnerBiome);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (CratePickedUp == false && col.gameObject.tag == "CRATE")
            {
                _crate = col.gameObject.GetComponent<BasicBonus>();
                if (_crate.CurrentState != EBonusState.Picked)
                {
                    CratePickedUp = true; 
                    _crate.SetPicked(true);
                    return;
                }
            }

            if (CratePickedUp == true && col.gameObject.layer == 9)
            {
                var biom = col.gameObject.transform.parent.GetComponentInParent <BiomBase>();
                if (biom != null && biom.BiomType == OwnerBiome)
                {
                    CratePickedUp = false;
                }
            }
        }

        protected override void OnBrainWillDie()
        {
            base.OnBrainWillDie();
            if (_crate != null)
            {
                _crate.SetPicked(false);
            }
        }

        protected override void InitStates()
        {
            _availiableStates.Add(EAIState.PickCrate, new AIStatePickCrate(this));
            _availiableStates.Add(EAIState.CratePicked, new AIStateCratePicked(this));

            BaseState = EAIState.PickCrate;
        }
    }
}

