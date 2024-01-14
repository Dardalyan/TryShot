using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float hitPoints = 100f;

    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        healthText.text = Mathf.Floor(hitPoints).ToString();
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        healthText.text = Mathf.Floor(hitPoints).ToString();
        if (hitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
        }
    }
}
