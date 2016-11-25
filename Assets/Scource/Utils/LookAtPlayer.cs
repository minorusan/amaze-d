using UnityEngine;
using System.Collections;
using Gameplay;

public class LookAtPlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Game.Instance.CurrentSession.Player.transform);
    }
}
