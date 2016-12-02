using UnityEngine;
using System.Collections;

public class SkyboxLerp : MonoBehaviour
{
    public Transform positionOne;
    public Transform positionTwo;
    public Transform positionThree;
    public Skybox skybox;
    public Material materialOne;
    public Material materialTwo;

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(positionOne.position, positionTwo.position);
        var distanceTwo = Vector3.Distance(transform.position, positionOne.position);
        var diff = distanceTwo / distance;
        if (diff > 1f)
        {
            distance = Vector3.Distance(positionTwo.position, positionThree.position);
            distanceTwo = Vector3.Distance(transform.position, positionTwo.position);
            diff = distanceTwo / distance;
            skybox.material = materialTwo;
        }
        else
        {
            skybox.material = materialOne;
        }
        skybox.material.SetFloat("_Blend", Mathf.Clamp01(diff));
    }
}
