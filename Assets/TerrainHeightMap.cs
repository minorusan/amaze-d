using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Core.Map.TerrainGeneration;
using System.Runtime.Remoting.Messaging;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


[RequireComponent (typeof(Terrain))]
public class TerrainHeightMap : MonoBehaviour
{
	private TerrainData _data;
	private float _rock = 3f;
	private int _grain = 3;

	[Header ("Diamond square")]
	public InputField GrainValue;
	public InputField RockValue;
	// Use this for initialization
	void Start ()
	{
		_data = GetComponent <Terrain> ().terrainData;
	}

	public void ApplyDiamondSquare ()
	{
		_rock = (float)Convert.ToDouble (RockValue.text.Length > 0 ? RockValue.text : _rock.ToString ());
		_grain = Convert.ToInt32 (GrainValue.text.Length > 0 ? GrainValue.text : _grain.ToString ());
		var algorithm = new DiamondSquareTerrain (_grain, _rock);
		var hightMap = algorithm.GetHeightMap (200);
		_data.SetHeights (0, 0, hightMap);
	}



	// Update is called once per frame
	public void Back ()
	{
		SceneManager.LoadSceneAsync ("MainMenu");
	}
}
