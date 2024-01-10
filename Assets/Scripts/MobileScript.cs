using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileScript : MonoBehaviour
{
    public BatteryScript batteryScript;

    public RectTransform MobileDesktop;
    public GameObject virtualScreen; // Reference to the UI canvas representing the virtual mobile screen

    private bool isTouching; // Flag to track if the screen is being touched
    public RectTransform screenRect; // Reference to the virtual screen's RectTransform
    private GraphicRaycaster raycaster; // Reference to the GraphicRaycaster for handling UI clicks

    public RectTransform batteryIndicationScreen;
    public GameObject TurnOnBtn;

    public RectTransform PinKeypad;
    public Vector3 hiddenPosition; // Off-screen position
    public Vector3 originalPosition;

    public GameObject SMSPanel;

    private void Start()
    {
        // Get the RectTransform and GraphicRaycaster components
        screenRect = virtualScreen.GetComponent<RectTransform>();
        raycaster = virtualScreen.GetComponent<GraphicRaycaster>();
        
        if (MobileDesktop != null)
        {
            // Get the off-screen position
            hiddenPosition = new Vector3(Screen.width * 40, 0, 0);
            // Move the menu off-screen initially
            MobileDesktop.localPosition = hiddenPosition;
        }

        SMSPanel.SetActive(false);
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            // Check if the mouse click is within the bounds of the virtual screen
            if (RectTransformUtility.RectangleContainsScreenPoint(screenRect, mousePos))
            {
                // Simulate touch input on the virtual screen
                isTouching = true;
                HandleTouchInput(mousePos);
            }
        }

        // Check for mouse release
        if (Input.GetMouseButtonUp(0) && isTouching)
        {
            isTouching = false;
        }
    }

    private void HandleTouchInput(Vector2 inputPosition)
    {
        // Create a pointer event data
        PointerEventData pointerEventData = new PointerEventData(null);
        pointerEventData.position = inputPosition;

        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast from the pointer event data to detect UI elements under the touch/click position
        raycaster.Raycast(pointerEventData, results);

        // Process the click on UI elements (e.g., buttons, etc.)
        foreach (RaycastResult result in results)
        {
            // Handle click on UI elements here if needed
            Debug.Log("Clicked on: " + result.gameObject.name);
        }
    }

    public void TurnOn()
    {
        if (batteryIndicationScreen != null)
        {
            // Get the off-screen position
            hiddenPosition = new Vector3(Screen.width * 40, 0, 0);
            // Move the menu off-screen initially
            batteryIndicationScreen.localPosition = hiddenPosition;
        }
        PinKeypad.localPosition = originalPosition;
        MobileDesktop.localPosition = originalPosition;
    }

    public void TogglePopupSMS()
    {
        if (SMSPanel != null)
        {
            // Toggle the active state of the pop-up panel
            SMSPanel.SetActive(!SMSPanel.activeSelf);
        }
    }

}
