using System;
using Core.Interactivity.AI;
using Core.Interactivity.AI.AIStates;
using Core.Bioms;
using UnityEngine;


namespace Core.Interactivity.AI.Brains
{
    public class ZombieBrains:ArtificialIntelligence
    {
        public float SearchDistance;

        protected override void InitStates()
        {
            // _availiableStates.Add(EAIState.Wandering, new AIStateWandering(this, SearchDistance));
            _availiableStates.Add(EAIState.Alert, new AIStateAlert(this, SearchDistance * 2));
            _availiableStates.Add(EAIState.Attack, new AIStateAttack(this, 10));
        }
    }
}

