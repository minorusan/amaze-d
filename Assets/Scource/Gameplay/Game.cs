using UnityEngine;
using System.Collections;
using Utils;
using Core.Map;
using Core.Gameplay;
using Core.Bioms;
using UnityEngine.SceneManagement;


namespace Gameplay
{
	public class Game : MonoSingleton<Game>
	{
		private Session _currentSession;

		public BiomBase[] bioms;
		public GameObject WinPopup;

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

		void Start ()
		{
			_currentSession = new Session ();
			Time.timeScale = 1;
		}

		void Update ()
		{
			if (bioms.Length > 1)
			{
				if (bioms [0].BiomPower < 0 || bioms [1].BiomPower < 0)
				{
					Time.timeScale = 0.001f;
					WinPopup.SetActive (true);
				}
			}

		}

		public void Back ()
		{
			SceneManager.LoadSceneAsync ("MainMenu");
		}

		public void Restart ()
		{
			SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
		}
	}
}

