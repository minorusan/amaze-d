using System;
using UnityEngine;
using Core.Map;
using Core.Interactivity.Movement;
using Core.Interactivity;
using Core.Bioms;
using Core.Gameplay;


namespace Gameplay
{
    public class Session
    {
        private readonly MapGenerator _currentMap;
        private readonly ReferenceStorage _references;
        private readonly BonusManager _bm;
        private readonly Player _player;

        public BonusManager BonusManager
        {
            get
            {
                return _bm;
            }
        }

        public MapGenerator CurrentMap
        {
            get
            {
                return _currentMap;
            }
        }

        public Player Player
        {
            get
            {
                return _player;
            }
        }

        public ReferenceStorage References
        {
            get
            {
                return _references;
            }
        }

        public Session()
        {
            _currentMap = GameObject.FindGameObjectWithTag(Tags.kMapTag).GetComponent <MapGenerator>();
            _player = GameObject.FindGameObjectWithTag(Tags.kPlayerTag).GetComponent <Player>();
            _bm = new BonusManager();
            _references = new ReferenceStorage();
        }

        public GameObject GetBiomOfType(EBiomType type)
        {
            var biomTag = "";
            switch (type)
            {
                case EBiomType.Fel:
                    {
                        biomTag = "FEL";
                        break;
                    }
                default:
                    break;
            }
            return GameObject.FindGameObjectWithTag(biomTag);
        }
    }
}

