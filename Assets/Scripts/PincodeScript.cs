using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PincodeScript : MonoBehaviour
{
    public MobileScript mobileScript;



    public RectTransform MobileDesktop;
    public RectTransform PinKeypad;
    public Vector3 hiddenPosition; // Off-screen position
    public Vector3 originalPosition;
    private bool isVisible;

    float distance;
    float angleView;
    Vector3 direction;

    [SerializeField] public string correctPin;
    private string enteredPin = "";   

    public void EnterDigit(int digit)
    {
        if (enteredPin.Length < correctPin.Length)
        {
            enteredPin += digit.ToString();
        }

        if (enteredPin.Length == correctPin.Length)
        {
            CheckPin();
        }
    }

    private void CheckPin()
    {
        if (enteredPin == correctPin)
        {
            Debug.Log("Correct PIN entered!");

            if (PinKeypad != null)
            {
                // Get the off-screen position
                hiddenPosition = new Vector3(Screen.width * 40, 0, 0);
                // Move the menu off-screen initially
                PinKeypad.localPosition = hiddenPosition;
            }
            StartCoroutine(ClearPinAfterDelay());
        }
        else
        {
            Debug.Log("Incorrect PIN. Try again.");
            StartCoroutine(ClearPinAfterDelay());
        }
    }

    private IEnumerator ClearPinAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        enteredPin = "";
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
