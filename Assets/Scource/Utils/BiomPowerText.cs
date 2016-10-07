using UnityEngine;
using System.Collections;
using Core.Bioms;

public class BiomPowerText : MonoBehaviour
{

    public BiomBase biom;
    public TextMesh text;
    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
        text.text = "Current power:" + biom.BiomPower.ToString();
    }
}
