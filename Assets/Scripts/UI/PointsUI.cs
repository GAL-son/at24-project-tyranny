using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointsUI : MonoBehaviour
{
    public Text pointText;
    // Start is called before the first frame update
    public void UpdatePoints(int points)
    {
        pointText.text = points.ToString();
    }
}
