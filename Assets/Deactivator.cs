using UnityEngine;
using System.Collections;

public class Deactivator : MonoBehaviour
{

    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
