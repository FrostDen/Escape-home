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
            hintText.text = "Kam si myslíš, e ideš? Najprv si nasaï rúško na tvár.";
            //hintText.text = "Where do you think you're going? You need to wear a facemask first.";
        }

        if (inventoryItemController.Facemask == true)
        {
            hintText.text = "You have to have a test with negative result in order to go outside.";
            hintText.text = "Na to aby si mohol ís von, musíš ma test s negatívnym vısledkom.";
            Debug.Log("Facemask is present");
            //hud.WinGame();
        }

        //if (inventoryItemController.Test == true)
        //{
        //    hintText.text = "Prepáè, kamarát... ale nemôem a pusti von s pozitívnym testom. Vylieè sa a príï o dva tıdne s negatívnym testom.";
        //    //hintText.text = "Sorry buddy... but i can't let you go outside with positive test. Get cured and come back in two weeks with a negative test.";
        //    Debug.Log("Facemask is present");
        //    //hud.WinGame();
        //}
    }

    private void OnTriggerExit(Collider collider)
    {
        HintPanel.SetActive(false);
        Debug.Log("Trigger exited");

    }
}
