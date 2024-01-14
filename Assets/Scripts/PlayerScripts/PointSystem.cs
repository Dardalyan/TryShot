using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    private int points = 0;

    [SerializeField] private TextMeshProUGUI pointsText;
    
    void Start()
    {
        points = 0;
        pointsText.text = points.ToString();
    }

    public void AddToPoints()
    {
        points++;
        pointsText.text = points.ToString();
    }
}
