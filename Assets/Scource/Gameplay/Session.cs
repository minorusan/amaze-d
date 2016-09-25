using System;
using UnityEngine;
using Core.Map;
using Core.Interactivity.Movement;


namespace Gameplay
{
	public class Session
	{
		private readonly MapGenerator _currentMap;
		private readonly Player _player;

		public MapGenerator CurrentMap {
			get
			{
				return _currentMap;
			}
		}

		public Player Player {
			get
			{
				return _player;
			}
		}

		public Session ()
		{
			_currentMap = GameObject.FindGameObjectWithTag (Tags.kMapTag).GetComponent <MapGenerator> ();
			_player = GameObject.FindGameObjectWithTag (Tags.kPlayerTag).GetComponent <Player> ();
		}
	}
}

