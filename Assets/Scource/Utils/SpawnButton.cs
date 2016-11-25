using UnityEngine;
using System.Collections;
using Core.Bioms;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpawnButton : MonoBehaviour
{
    private Button _selfButton;
    public BiomBase Owner;
    public int RequiredPower;
    public GameObject PowerLackWarning;

    private void Start()
    {
        _selfButton = GetComponent<Button>();
    }

    public void Update()
    {
        if (Owner.BiomPower < RequiredPower)
        {
            _selfButton.image.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            _selfButton.image.color = Color.white;
        }
    }

    public void OnClick()
    {
        if (Owner.BiomPower < RequiredPower)
        {
            PowerLackWarning.SetActive(true);
        }
    }
}
