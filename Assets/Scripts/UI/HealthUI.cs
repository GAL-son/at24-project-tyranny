using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Text TextHealth;
    public void UpdateHealtText(int health)
    {
        TextHealth.text = health.ToString();
    }
}
