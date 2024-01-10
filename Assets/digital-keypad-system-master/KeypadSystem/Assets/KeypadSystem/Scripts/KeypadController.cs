using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadController : MonoBehaviour
{
    public AN_DoorScript doorScript;
    public string password;
    public int passwordLimit = 7;
    public TMP_Text displayText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

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
            if (audioSource != null)
                audioSource.PlayOneShot(correctSound);
            displayText.text = "Access granted";
            displayText.color = Color.green;

            // Unlock the door if the password is correct
            if (doorScript != null)
            {
                doorScript.Locked = false;
                doorScript.OpenSpeed = 0f;
                doorScript.CanOpen = true; // Allow opening the door
            }
        }
        else
        {
            if (audioSource != null)
                audioSource.PlayOneShot(wrongSound);
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
