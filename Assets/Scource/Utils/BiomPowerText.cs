using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Core.Bioms;
using Gameplay;

[RequireComponent(typeof(Text))]
public class BiomPowerText : MonoBehaviour
{
    private Text _text;
    public BiomBase biom;

    // Use this for initialization
    // Update is called once per frame

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = "Current biome power:" + biom.BiomPower.ToString();
    }
}
