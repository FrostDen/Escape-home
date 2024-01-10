using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeScript : MonoBehaviour
{
    public TextMeshProUGUI clockText; // Reference to the Text UI element
    public TextMeshProUGUI clockPINText;

    public float currentTime = 0f;
    public float timeScale = 100f; // Adjust this value to change the speed of the clock

    void Start()
    {
        // Set a random starting time within a day (86400 seconds)
        currentTime = Random.Range(0f, 86400f);
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeScale;
        int hours = Mathf.FloorToInt(currentTime / 3600) % 24;
        int minutes = Mathf.FloorToInt(currentTime / 60) % 60;

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);

        if (clockText != null)
        {
            clockText.text = timeString;
            clockPINText.text = timeString;
        }
    }
}
