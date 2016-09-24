using UnityEngine;
using System.Collections;
using Utils;
using Core.Map;


namespace Gameplay
{
	public class Game : MonoSingleton<Game>
	{
		private Session _currentSession;

		public Session CurrentSession {
			get
			{
				return _currentSession;
			}
		}

		public MapGenerator CurrentMap {
			get
			{
				return _currentSession.CurrentMap;
			}
		}

		// Use this for initialization
		void Start ()
		{
			_currentSession = new Session ();
		}

		// Update is called once per frame
		void Update ()
		{

		}
	}
}

