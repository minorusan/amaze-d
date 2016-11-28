using UnityEngine;
using System.Collections;
using Core.Bioms;

namespace Core.Bioms
{
    public delegate void BiomCollisionHandler(BiomBase collidedWith);
    public class BiomCollisionDetector : MonoBehaviour
    {
        public event BiomCollisionHandler CollidedWithBiome;

        private void OnTriggerStay(Collider col)
        {    
            if (CollidedWithBiome != null)
            {
                CollidedWithBiome(col.gameObject.transform.parent.GetComponentInParent<BiomBase>());
            }
        }
    }
}

