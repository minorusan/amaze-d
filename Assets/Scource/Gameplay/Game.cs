using UnityEngine;
using System.Collections;
using Utils;
using Core.Map;
using Core.Gameplay;


namespace Gameplay
{
	public class Game : MonoSingleton<Game>
	{
		private Session _currentSession;

		public ReferenceStorage ReferenceStorage {
			get
			{
				return _currentSession.References;
			}
		}

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

