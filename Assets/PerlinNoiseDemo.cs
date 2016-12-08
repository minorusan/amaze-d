using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class PerlinNoiseDemo : MonoBehaviour
{
	public InputField power;
	public InputField scale;
	private float _power = 3f;
	private float _scale = 3f;
	private Vector2 v2SampleStart = new Vector2 (0f, 0f);

	public void Back ()
	{
		SceneManager.LoadSceneAsync ("MainMenu");
	}


	public void MakeSomeNoise ()
	{
		MeshFilter mf = GetComponent<MeshFilter> ();
		Vector3[] vertices = mf.mesh.vertices;
		_power = (float)Convert.ToDouble (power.text.Length > 0 ? power.text : _power.ToString ());
		_scale = (float)Convert.ToDouble (scale.text.Length > 0 ? scale.text : _scale.ToString ());
		v2SampleStart = new Vector2 (UnityEngine.Random.Range (0.0f, 100.0f), UnityEngine.Random.Range (0.0f, 100.0f));
		for (int i = 0; i < vertices.Length; i++)
		{    
			float xCoord = v2SampleStart.x + vertices [i].x * _scale;
			float yCoord = v2SampleStart.y + vertices [i].z * _scale;
			vertices [i].y = (Mathf.PerlinNoise (xCoord, yCoord) - 0.5f) * _power; 
		}
		mf.mesh.vertices = vertices;
		mf.mesh.RecalculateBounds ();
		mf.mesh.RecalculateNormals ();
	}
}
