using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Winscreen : MonoBehaviour
{
    public HUD hud;
    public InventoryItemController inventoryItemController;
    public GameObject HintPanel;
    public TextMeshProUGUI hintText;

    public void Start()
    {
        hintText = HintPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger entered");

        if (inventoryItemController.Facemask == false)
        {
            HintPanel.SetActive(true);
            Debug.Log("No facemask!");
            hintText.text = "Where do you think you're going? You need to wear a facemask first.";
        }

        if (inventoryItemController.Facemask == true)
        {
            Debug.Log("Facemask is present");
            hud.WinGame();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        HintPanel.SetActive(false);
        Debug.Log("Trigger exited");

    }
}
