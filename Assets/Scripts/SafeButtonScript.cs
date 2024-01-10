using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

namespace NavKeypad
{
public class SafeButtonScript : MonoBehaviour
{
    public TMP_Text displayText; // Text component to display entered code
    [SerializeField] private string value; // Value this button represents (e.g., "1", "2", etc.)

    float distance;
    float angleView;
    Vector3 direction;

    public void OnButtonClick()
    {
        // Append the value of this button to the displayed code
        displayText.text += value;
    }

    public void OnButtonClickClear()
    {
        // Append the value of this button to the displayed code
        displayText.text = null;
    }

    public void OnButtonClickConfirm()
    {
        // Append the value of this button to the displayed code
        displayText.text = null;
    }

    bool NearView() // it is true if you near interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        if (angleView < 45f && distance < 2f) return true;
        else return false;
    }
}
}