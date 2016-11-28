using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.AI.Brains;
using Gameplay;
using System.Configuration;

namespace Core.Bioms.BiomeComponents
{
    public class BiomWarriorsController:BiomeNPCController
    {
        protected override void Awake()
        {
            _registeredNPCs = GetComponentsInChildren<WarriorBrains>();
            base.Awake();
        }
    }
}
