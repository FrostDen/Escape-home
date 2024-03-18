using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PincodeScript : MonoBehaviour
{
    public MobileScript mobileScript;
    public HUD hud; // Reference to the HUD script

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

    public Slider pinSlider; // Reference to the slider UI element
    public Color correctColor = Color.green; // Color for correct digits
    public Color wrongColor = Color.red; // Color for wrong digits

    private Coroutine resetPinCoroutine; // Coroutine reference for resetting the PIN

    private void Start()
    {
        // Generate a random four-digit PIN code
        GenerateRandomPin();
        // Initialize the pin slider color to a neutral color
        pinSlider.fillRect.GetComponent<Image>().color = Color.white;
    }

    private void GenerateRandomPin()
    {
        // Generate four random digits between 0 and 9 and concatenate them to form the PIN code
        correctPin = Random.Range(1, 9).ToString() +
                     Random.Range(1, 9).ToString() +
                     Random.Range(1, 9).ToString() +
                     Random.Range(1, 9).ToString();
    }

    public void EnterDigit(int digit)
    {
        if (enteredPin.Length < correctPin.Length)
        {
            enteredPin += digit.ToString();
            CheckPin(); // Check the pin after entering each digit
            UpdateSliderValue(); // Update the slider value after each digit
        }
    }

    private void CheckPin()
    {
        bool allDigitsCorrect = true;

        for (int i = 0; i < enteredPin.Length; i++)
        {
            if (enteredPin[i] != correctPin[i])
            {
                allDigitsCorrect = false;
                break; // Exit the loop if any digit is wrong
            }
        }

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

            // Reset the PIN after 5 seconds if the PIN was entered correctly
            if (resetPinCoroutine != null)
            {
                StopCoroutine(resetPinCoroutine);
            }
        }
        else
        {
            Debug.Log("Incorrect PIN. Try again.");
            pinSlider.fillRect.GetComponent<Image>().color = wrongColor; // Change the color of the slider to indicate incorrectness

            // Reset the PIN after 1 second if the PIN was entered incorrectly
            if (resetPinCoroutine != null)
            {
                StopCoroutine(resetPinCoroutine);
            }
            resetPinCoroutine = StartCoroutine(ClearPinAfterDelay(1.5f));
        }

        if (allDigitsCorrect)
        {
            Debug.Log("Correct PIN entered!");
            pinSlider.fillRect.GetComponent<Image>().color = correctColor; // Change the color of the slider to indicate correctness
        }
    }

    private IEnumerator ClearPinAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enteredPin = "";
        pinSlider.fillRect.GetComponent<Image>().color = Color.white; // Reset the color of the slider to neutral
        pinSlider.value = 0;
    }

    private void UpdateSliderValue()
    {
        // Calculate the fill amount increment based on the total number of digits in the correct PIN
        float fillAmountIncrement = 1f / correctPin.Length;

        // Calculate the current fill amount of the slider
        float currentFillAmount = pinSlider.value;

        // Increment the fill amount of the pinSlider
        currentFillAmount += fillAmountIncrement;

        // Clamp the fill amount to prevent it from exceeding 1
        currentFillAmount = Mathf.Clamp01(currentFillAmount);

        // Update the value of the slider
        pinSlider.value = currentFillAmount;
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
