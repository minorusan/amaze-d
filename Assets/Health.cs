using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    public Image HealthBar;
    public float CurrentHealthAmount = 100;
    public GameObject DeathEffect;
    bool activatedDeathEffect = false;

    public event Action OnWillDie;
    public event Action OnDied;
    // Use this for initialization
    void OnEnable()
    {
        CurrentHealthAmount = 100;
        activatedDeathEffect = false;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (CurrentHealthAmount > 0)
        {
            HealthBar.fillAmount = CurrentHealthAmount / 100;
            HealthBar.transform.parent.LookAt(Camera.main.transform);
        }
        else
        {
            if (OnWillDie != null)
            {
                OnWillDie();
            }
            DeathEffect.gameObject.SetActive(true);
            DeathEffect.transform.position = transform.position;

            gameObject.SetActive(false);
            if (OnDied != null)
            {
                OnDied();
            }
        }
    }


}
