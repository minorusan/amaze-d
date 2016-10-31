using System;
using Core.Interactivity.AI;
using Core.Interactivity.AI.AIStates;
using UnityEngine;
using Core.Bioms;
using System.Linq.Expressions;
using Gameplay;


namespace Core.Interactivity.AI.Brains
{
	public class SlaveBrains:ArtificialIntelligence
	{

		public bool CratePickedUp {
			get;
			private set;
		}

		public EBiomType OwnerBiome;

		protected override void Start ()
		{
			base.Start ();


			_availiableStates.Add (EAIState.PickCrate, new AIStatePickCrate (this));
			_availiableStates.Add (EAIState.CratePicked, new AIStateCratePicked (this));
		
			_currentState = _availiableStates [EAIState.PickCrate];
			_currentState.OnEnter ();
			Game.Instance.ReferenceStorage.RegisterSlave (this, OwnerBiome);
		}

		private void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "CRATE")
			{
				CratePickedUp = true;
			}
			else
			{
				var biom = col.gameObject.transform.parent.GetComponentInParent <BiomBase> ();
				if (biom != null && biom.BiomType == OwnerBiome)
				{
					CratePickedUp = false;
				}
			}
		}
	}
}

