using UnityEngine;
using System.Collections;
using Gameplay;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Bioms
{
    #if UNITY_EDITOR

    [CustomEditor(typeof(BiomSpawn))]
    [CanEditMultipleObjects]
    public class BiomSpawnEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            var scriptTarget = target as BiomSpawn;
            scriptTarget.Infinite = EditorGUILayout.Toggle("Infinite", scriptTarget.Infinite);
            if (!scriptTarget.Infinite)
            {
                scriptTarget.LifeSpan = EditorGUILayout.IntField("Life Span", scriptTarget.LifeSpan);
            }
            scriptTarget.RequiredPower = EditorGUILayout.IntField("Required Power", scriptTarget.RequiredPower);
            scriptTarget.RandomizeLocation = EditorGUILayout.Toggle("Randomized location", scriptTarget.RandomizeLocation);
        }
    }

    #endif

    public class BiomSpawn:MonoBehaviour
    {
        enum BiomSpawnState
        {
            Grows,
            Exists,
            Dies
        }

        #region PRIVATE

        private Vector3 InitialScale;
        private BiomSpawnState _currentState;
        private float _remainingLifeTime;

        #endregion

        public bool Infinite;
        public bool RandomizeLocation;
        public int RequiredPower = 3;
        public int LifeSpan = 30;

        #region Monobehaviour

        private void Awake()
        {
            InitialScale = transform.localScale;
        }

        private void OnEnable()
        {
            _remainingLifeTime = LifeSpan;

            _currentState = BiomSpawnState.Grows;
            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case BiomSpawnState.Grows:
                    {
                        Grow();
                        break;
                    }
                case BiomSpawnState.Exists:
                    {
                        Exist();
                        break;
                    }
                case BiomSpawnState.Dies:
                    {
                        Dies();
                        break;
                    }
                default:
                    break;
            }
        }

        #endregion

        #region INTERNAL METHODS

        private void Dies()
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime);
            if (transform.localScale.x <= 0.1f)
            {
                gameObject.SetActive(false);
            }
        }

        private void Exist()
        {
            if (_remainingLifeTime > 0 || Infinite)
            {
                _remainingLifeTime -= Time.deltaTime;
            }
            else
            {
                _currentState = BiomSpawnState.Dies;
                DeactivateSpawn();
            }
        }

        private void Grow()
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, InitialScale, Time.deltaTime);
            if (transform.localScale == InitialScale)
            {
                _currentState = BiomSpawnState.Exists;
            }
        }

        public void DeactivateSpawn()
        {
            _currentState = BiomSpawnState.Dies;
            var col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }

        #endregion
    }
}
