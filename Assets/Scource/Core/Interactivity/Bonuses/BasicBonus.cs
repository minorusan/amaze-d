using UnityEngine;
using System.Collections;
using Core.Bioms;
using System;
using Core.Interactivity.AI.Brains;
using Core.Interactivity.AI;


namespace Core.Interactivity
{
    public enum EBonusState
    {
        Idle,
        Picked
    }

    public class BasicBonus : MonoBehaviour
    {
        private EBonusState _currentState;
        private Transform _rootTransform;
        private EBiomType targetOwner;

        public int BonusValue = 20;

        public static event EventHandler CrateBeingPicked;

        public EBonusState CurrentState
        {
            get
            {
                return _currentState;
            }
        }

        #region Monobehaviour

        private void Start()
        {
            _currentState = EBonusState.Idle;
            _rootTransform = transform.parent;
        }

        private void OnTriggerEnter(Collider col)
        {
            switch (_currentState)
            {
                case EBonusState.Idle:
                    {
                        var brains = col.gameObject.GetComponent<SlaveBrains>();
                        targetOwner = brains.OwnerBiome;
                        if (brains != null && brains.CratePickedUp == false)
                        {
                            transform.SetParent(col.gameObject.transform);
                            transform.localPosition = new Vector3(0, 3f, 0);
                            CrateBeingPicked(this, EventArgs.Empty);
                        }

                        break;
                    }
                case EBonusState.Picked:
                    {
                        if (col.gameObject.layer == 9)
                        {
                            var biom = col.transform.parent.gameObject.GetComponentInParent <BiomBase>();
                            if (biom.BiomType == targetOwner)
                            {
                                biom.BiomPower += BonusValue;
                                transform.SetParent(_rootTransform); 
                                gameObject.SetActive(false);
                            }
                        }

                        break;
                    }
                default:
                    break;
            }
        }

        #endregion

        public void SetPicked(bool picked)
        {
            _currentState = picked ? EBonusState.Picked : EBonusState.Idle;
            if (!picked)
            {
                transform.SetParent(_rootTransform);  
            }
        }
    }
}
