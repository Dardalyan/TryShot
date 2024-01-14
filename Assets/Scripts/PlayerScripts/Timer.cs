using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer = 120f;
    
    [SerializeField] private Canvas timesOutCanvas;
    [SerializeField] private TextMeshProUGUI timerText;
    void Start()
    {
        timesOutCanvas.enabled = false;
        timer = 120f;
        timerText.text = Mathf.Floor(timer).ToString();
    }
    
    void Update()
    {
        timer -= Time.deltaTime;
        
        timerText.text = Mathf.Floor(timer).ToString();

        if (timer <= 0)
        {
            timesOutCanvas.enabled = true;
            Time.timeScale = 0;
            FindObjectOfType<WeaponSwitcher>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
