using System;
using UnityEngine;
using Core.Map;


namespace Gameplay
{
	public class Session
	{
		private readonly MapGenerator _currentMap;

		public MapGenerator CurrentMap {
			get
			{
				return _currentMap;
			}
		}

		public Session ()
		{
			_currentMap = GameObject.FindGameObjectWithTag (Tags.kMapTag).GetComponent <MapGenerator> ();
		}
	}
}

