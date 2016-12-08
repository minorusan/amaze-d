using UnityEngine;
using System.Collections;
using Core.Interactivity.Movement;
using UnityEngine.EventSystems;
using Gameplay;
using Core.Pathfinding;


public class PathfindinDemoScript : MonoBehaviour
{
	private Path _path;
	public MovableObject demoObject;
	public GameObject Destination;
	public GameObject Light;
	// Use this for initialization
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000))
			{
				var worldCoords = transform.TransformPoint (hit.point);
				var node = Game.Instance.CurrentMap.GetNodeByPosition (worldCoords);
				demoObject.CurrentPath.Nodes.Clear ();
				_path = Pathfinder.FindPathToDestination (demoObject.MyPosition.GridPosition, 
				                                          node.GridPosition, EPathfindingAlgorithm.AStar, false);
				Destination.SetActive (true);
				Destination.transform.position = node.Position;
				StartCoroutine (LightPath ());
			}
		}
	}

	private IEnumerator LightPath ()
	{
		foreach (var node in _path.Nodes)
		{
			var newLight = Instantiate (Light);
			newLight.transform.position = node.Position;
			newLight.SetActive (true);
			yield return new WaitForEndOfFrame ();
		}
		Invoke ("DestroyAll", 4f);
		StopAllCoroutines ();
	}

	private void DestroyAll ()
	{
		var spawned = GameObject.FindGameObjectsWithTag ("CELL");
		for (int i = 1; i < spawned.Length; i++)
		{
			Destroy (spawned [i]);
		}
		demoObject.BeginMovementByPath (_path);
	}


}
