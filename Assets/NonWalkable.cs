using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using System;
using Gameplay;

namespace Utils
{
    public class NonWalkable : MonoBehaviour
    {
        private Node MyPosition;
        // Use this for initialization
        void Start()
        {
            MyPosition = Game.Instance.CurrentMap.GetNodeByPosition(transform.position);
            var nodes = Game.Instance.CurrentMap.CurrentMap;
            var mesh = GetComponent<MeshFilter>().mesh;

           

            foreach (var item in nodes)
            {
                if (item.CurrentCellType != Core.Map.ECellType.Blocked && mesh.bounds.Contains(transform.InverseTransformPoint(item.Position)))
                {
                    item.CurrentCellType = Core.Map.ECellType.Blocked;
                }
            }
        }

    }
}

