using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Interactivity.AI.Brains;
using Gameplay;
using AssemblyCSharp;

namespace Core.Bioms.BiomeComponents
{
    public class BiomSlavesController:BiomeNPCController
    {
        protected override void Awake()
        {
            _registeredNPCs = GetComponentsInChildren<SlaveBrains>();
            base.Awake();
        }
    }
}

