using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class KeypadController : MonoBehaviour
{
    public AN_DoorScript doorScript;
    public string password;
    public int passwordLimit = 4;
    public TMP_Text displayText;
    public Transform keypadTransform;

    public GameObject safeLock;

    private void Start()
    {
        doorScript.Locked = true;
        doorScript.OpenSpeed = 4f;
        displayText.text = "";
    }

    public void PasswordEntry(string number)
    {
        if (number == "Clear")
        {
            Clear();
            return;
        }
        else if (number == "Enter")
        {
            Enter();
            return;
        }

        int length = displayText.text.ToString().Length;
        if (length < passwordLimit)
        {
            displayText.text = displayText.text + number;
        }
    }

    public void Clear()
    {
        displayText.text = "";
        displayText.color = Color.white;
    }

    private void Enter()
    {
        if (displayText.text == password)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.KeypadCorrect, keypadTransform.position);
            displayText.text = "Access granted";
            displayText.color = Color.green;
            safeLock.gameObject.layer = LayerMask.NameToLayer("Object");
            doorScript.enabled = false;

            // Unlock the door if the password is correct
            if (doorScript != null)
            {
                doorScript.Locked = false;
                doorScript.OpenSpeed = 0f;
                doorScript.CanOpen = true; // Allow opening the door

                // Adjust hinge joint limits of safeLock GameObject
                doorScript.AdjustHingeJointLimits(safeLock, -110f, -10f);
            }

        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.KeypadWrong, keypadTransform.position);
            displayText.text = "Access denied";
            displayText.color = Color.red;
            StartCoroutine(waitAndClear());
        }
    }

    IEnumerator waitAndClear()
    {
        yield return new WaitForSeconds(0.75f);
        Clear();
    }
}
