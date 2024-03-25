using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryScript : MonoBehaviour
{
    public MobileScript mobileScript;
    public FlashlightMobileScript flashlightMobileScript;
    public AdapterScript adapterScript;
    public ChargerScript chargerScript;
    public Image batteryIndicator;
    public GameObject TurnOnBtn;
    public Image BatteryIcon;
    public Image batteryBarImage;
    public Image batteryBarImagePIN;
    public TextMeshProUGUI BatteryTextNumber;
    public TextMeshProUGUI BatteryPINTextNumber;

    public RectTransform MobileDesktop;
    public RectTransform batteryIndicationScreen;
    public Vector3 hiddenPosition; // Off-screen position
    public Vector3 originalPosition;
    private bool isVisible;

    public Sprite battery0; // Sprite for 0% capacity
    public Sprite battery5; // Sprite for 5% capacity
    public Sprite battery15; // Sprite for 15% capacity
    public Sprite battery25; // Sprite for 25% capacity
    public Sprite battery50; // Sprite for 50% capacity
    public Sprite battery75; // Sprite for 75% capacity
    public Sprite battery100; // Sprite for 100% capacity

    public float maxCapacity = 100.0f; // Maximum battery capacity
    public float currentCapacity = 0.0f; // Current battery capacity
    public float chargeRate = 5.0f; // Rate at which the battery charges per second
    public float dischargeRate = 2.0f; // Rate at which the battery discharges per second

    public HUD hud; // Reference to the HUD script
    public bool isPhoneCharging = false; // Flag to track if the phone is currently charging

    void Start()
    {
        originalPosition = batteryIndicationScreen.localPosition;
        TurnOnBtn.SetActive(false);
    }

    void Update()
    {
        // Check if both adapter and charger are connected
        if (adapterScript.isConnected && chargerScript.isConnected)
        {
            if (!isPhoneCharging)
            {
                // Set the flag to true indicating the phone is charging
                isPhoneCharging = true;

                // Complete the quest for charging the phone
                hud.mobilePhoneCharged = true;
                hud.SetNextQuest(3);
            }
        }

        TurnOnBtn.SetActive(false);
        if (adapterScript.isConnected && chargerScript.isConnected)
        {
            ChargeBattery();
        }
        else
        {
            DischargeBattery();
        }

        if (currentCapacity == 0)
        {
            flashlightMobileScript.flashlightMobile.enabled = false;
            flashlightMobileScript.isFlashlightOn = false;
            flashlightMobileScript.SetEmission(flashlightMobileScript.originalEmissionColorMobile, false);
        }
        BatteryTextNumber.text = currentCapacity.ToString("F0") + "%";
        BatteryPINTextNumber.text = currentCapacity.ToString("F0") + "%";

        UpdateBatteryImage();

        if (batteryBarImage != null)
        {
            // Update the fill amount of the image based on the current battery capacity
            batteryBarImage.fillAmount = currentCapacity / maxCapacity;
            batteryBarImagePIN.fillAmount = currentCapacity / maxCapacity;
            // Change the color of the image fill based on the current capacity ranges
            ChangeBarColor();
        }
    }

    void ChangeBarColor()
    {
        if (currentCapacity <= 15)
        {
            batteryBarImage.color = Color.red;
            batteryBarImagePIN.color = Color.red;
        }
        else if (currentCapacity <= 35)
        {
            batteryBarImage.color = new Color(1.0f, 0.5f, 0.0f); // Orange
            batteryBarImagePIN.color = new Color(1.0f, 0.5f, 0.0f); // Orange
        }
        else if (currentCapacity <= 50)
        {
            batteryBarImage.color = Color.yellow;
            batteryBarImagePIN.color = Color.yellow;
        }
        else
        {
            batteryBarImage.color = Color.green;
            batteryBarImagePIN.color = Color.green;
        }
    }

    void UpdateBatteryImage()
    {
        if (currentCapacity <= 0)
        {
            batteryIndicator.sprite = battery0;
            TurnOnBtn.SetActive(false);
            if (MobileDesktop != null)
            {
                // Get the off-screen position
                hiddenPosition = new Vector3(Screen.width * 40, 0, 0);
                // Move the menu off-screen initially
                MobileDesktop.localPosition = hiddenPosition;
            }
            batteryIndicationScreen.localPosition = originalPosition;
        }
        else if (currentCapacity <= 5)
        {
            batteryIndicator.sprite = battery5;
            TurnOnBtn.SetActive(false);
        }
        else if (currentCapacity <= 15)
        {
            batteryIndicator.sprite = battery15;
            TurnOnBtn.SetActive(true);
        }
        else if (currentCapacity <= 25)
        {
            batteryIndicator.sprite = battery25;
            TurnOnBtn.SetActive(true);
        }
        else if (currentCapacity <= 50)
        {
            batteryIndicator.sprite = battery50;
            TurnOnBtn.SetActive(true);
        }
        else if (currentCapacity <= 75)
        {
            batteryIndicator.sprite = battery75;
            TurnOnBtn.SetActive(true);
        }
        else
        {
            batteryIndicator.sprite = battery100;
            TurnOnBtn.SetActive(true);
        }
    }

    void ChargeBattery()
    {
        if (currentCapacity < maxCapacity)
        {
            // Charging the battery
            currentCapacity += chargeRate * Time.deltaTime;

            // Clamp the current capacity to the maximum capacity
            currentCapacity = Mathf.Clamp(currentCapacity, 0.0f, maxCapacity);
        }
    }

    void DischargeBattery()
    {
        if (currentCapacity > 0)
        {
            // Discharging the battery
            currentCapacity -= dischargeRate * Time.deltaTime;

            // Clamp the current capacity to a minimum of 0
            currentCapacity = Mathf.Clamp(currentCapacity, 0.0f, maxCapacity);
        }
    }
}
