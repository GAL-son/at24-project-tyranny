using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    public PointsUI pointsUI;
    
    int points = 0;
    

    public void AddPoint()
    {
        UpdatePoints(1);
        UpdateUI();
    }

    public void UpdatePoints(int points)
    {
        this.points += points;
        this.points = Math.Max(0, points);
        UpdateUI();
    }

    public void ClearPoints()
    {
        points = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        pointsUI.UpdatePoints(points);
    }
}
