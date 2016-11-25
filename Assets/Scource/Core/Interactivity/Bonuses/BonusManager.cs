using System;
using UnityEngine;
using Core.Interactivity.Movement;
using Core.Map;
using Gameplay;


namespace Core.Interactivity
{
    public class BonusManager
    {
        private BasicBonus[] _bonuses;

        public BonusManager()
        {
            _bonuses = GameObject.FindObjectsOfType <BasicBonus>();
        }

        public Node GetClosestCratePosition(Vector3 position, Node excludingNode = null)
        {
            float minDist = Mathf.Infinity;
            Node toReturn = null;

            foreach (var bonus in _bonuses)
            {
                if (bonus != null && bonus.CurrentState != EBonusState.Picked && bonus.isActiveAndEnabled)
                {
                    float dist = Vector3.Distance(bonus.transform.position, position);
                    if (dist < minDist)
                    {
                        var candidateNode = Game.Instance.CurrentMap.GetNodeByPosition(bonus.transform.position);
                        bool nodeOnBiome = candidateNode.BiomOwner != Core.Bioms.EBiomType.None;
                        if ((excludingNode != null && candidateNode == excludingNode) && nodeOnBiome)
                        {
                            continue;
                        }
                        toReturn = candidateNode;
                        minDist = dist;
                    }
                }
            }
            return toReturn;
        }
    }
}

