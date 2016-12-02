using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Core.Interactivity.Movement;
using Gameplay;


public class Targeter : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
	private Player _player;
	public GameObject TargetPrefab;

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		SetTarget ();
	}

	#endregion

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		SetTarget ();
	}

	private void SetTarget ()
	{
		TargetPrefab.SetActive (true);
		TargetPrefab.transform.parent = transform;
		TargetPrefab.transform.localPosition = Vector3.zero;

		if (_player == null)
		{
			_player = Game.Instance.CurrentSession.Player;
		}
		_player.SetTarget (this.gameObject);
	}

	#endregion

}
