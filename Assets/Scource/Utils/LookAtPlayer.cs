using UnityEngine;
using System.Collections;
using Gameplay;


namespace Utils
{
	public class LookAtPlayer : MonoBehaviour
	{
		void Update ()
		{
			transform.LookAt (Game.Instance.CurrentSession.Player.transform);
		}
	}
}

