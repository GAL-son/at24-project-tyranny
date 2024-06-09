using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public HealthUI healthUI;
    public int baseHealth;
    

    int health;

    private void Start()
    {
        health = baseHealth;
    }

    public void Hit(int damage)
    {
        Debug.Log("HIT" + damage);
        health -= damage;

        if(health < 0)
        {
            health = 0;

            // CALL RESTART
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthUI.UpdateHealtText(health);
    }
}
