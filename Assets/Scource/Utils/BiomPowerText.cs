using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Core.Bioms;


public class BiomPowerText : MonoBehaviour
{

	public BiomBase biom;
	public Text text;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		text.text = "Current biom power:" + biom.BiomPower.ToString ();
	}
}
