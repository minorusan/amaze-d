using UnityEngine;
using System.Collections;
using Core.Bioms;


namespace Core.Interactivity
{
	public enum EBonusState
	{
		Idle,
		Picked
	}

	public class BasicBonus : MonoBehaviour
	{
		private EBonusState _currentState;
		// Use this for initialization
		void Start ()
		{
			_currentState = EBonusState.Idle;
		}

		// Update is called once per frame
		void Update ()
		{

		}

		private void OnTriggerEnter (Collider col)
		{
			switch (_currentState)
			{
			case EBonusState.Idle:
				{
					transform.SetParent (col.gameObject.transform);
					transform.localPosition = new Vector3 (0, 2f, 0);
					_currentState = EBonusState.Picked;
					break;
				}
			case EBonusState.Picked:
				{
					if (col.gameObject.layer == 9)
					{
						col.transform.parent.gameObject.GetComponentInParent <BiomBase> ().BiomPower += 10;
						gameObject.SetActive (false);
					}

					break;
				}
			default:
				break;
			}

		}

	}

}
