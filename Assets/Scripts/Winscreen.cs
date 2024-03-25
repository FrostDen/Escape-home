using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Winscreen : MonoBehaviour
{
    public CovidTestScript covidTestScript;
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
            hud.SetNextQuest(8);
        }

        if (collider.CompareTag("CovidTest") && covidTestScript.isPositive == false)
        {
            //hintText.text = "Have you already tested yourself? No?! So what are you waiting for!";
            hintText.text = "U si sa otestoval? Nie?! Tak na èo tu èakáš!";
            Debug.Log("Facemask is present");
            hud.SetNextQuest(8);
        }

        if (inventoryItemController.Facemask == false && collider.CompareTag("CovidTest") && covidTestScript.isPositive == false)
        {
            HintPanel.SetActive(true);
            Debug.Log("No facemask!");
            hintText.text = "Kam si myslíš e ideš bez rúška a bez otestovania sa?! Vieš aká je toto zákerná choroba!";
            //hintText.text = "Where do you think you're going without a cloak and without testing yourself?! You know what an insidious disease this is!";
            hud.SetNextQuest(8);
        }

        if (collider.CompareTag("CovidTest") && covidTestScript.isPositive == true)
        {
            //hintText.text = "Make another one now and let's see and find a facemask over your mouth and nose! You can't go out without it.";
            hintText.text = "Sprav si ešte teraz jeden a uvidíme a nájdi si rúško na ústa a nos! Bez neho nemôeš ís von.";
            Debug.Log("Facemask is present");
            hud.SetNextQuest(8);

        }

        if (collider.CompareTag("CovidTest") && covidTestScript.isPositive == true && inventoryItemController.Facemask == false)
        {
            //hintText.text = "Make another one now and let's see and find a facemask over your mouth and nose! You can't go out without it.";
            hintText.text = "Sprav si ešte teraz jeden a uvidíme a nájdi si rúško na ústa a nos! Bez neho nemôeš ís von.";
            Debug.Log("Facemask is present");
            hud.SetNextQuest(8);
        }

        if (inventoryItemController.Facemask == true)
        {
            //hintText.text = "You have to have a test with negative result in order to go outside.";
            hintText.text = "Na to aby si mohol ís von, musíš ma test s negatívnym vısledkom. Rúško nestaèí!";
            Debug.Log("Facemask is present");
            hud.SetNextQuest(9);
        }

        if (inventoryItemController.Facemask == true && collider.CompareTag("CovidTest") && covidTestScript.isPositive == false)
        {
            HintPanel.SetActive(true);
            Debug.Log("No facemask!");
            hintText.text = "Rúško máš ale stále nevidím ten test negatívny...";
            //hintText.text = "You have a facemask but I still don't see the test negative...";
            hud.SetNextQuest(10);
        }

        if (collider.CompareTag("CovidTest") && covidTestScript.isPositive == true && inventoryItemController.Facemask == true)//|| inventoryItemController.CovidTest == true)
        {
            hintText.text = "Prepáè, kamarát... ale nemôem a pusti von s pozitívnym testom. Vylieè sa a príï o dva tıdne s negatívnym testom.";
            //hintText.text = "Sorry buddy... but i can't let you go outside with positive test. Get cured and come back in two weeks with a negative test.";
            Debug.Log("Facemask is present");
            //hud.WinGame();
            hud.SetNextQuest(11);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        HintPanel.SetActive(false);
        Debug.Log("Trigger exited");

    }
}
