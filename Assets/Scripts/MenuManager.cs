using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Button resumeBtn;
    public Button restartBtn;
    public Button menuBtn;
    public RectTransform menuPanel; // Reference to your UI menu panel

    private Vector3 hiddenPosition; // Off-screen position
    private bool isVisible;

    public void Start()
    {
        if (menuPanel != null)
        {
            // Get the off-screen position
            hiddenPosition = new Vector3(Screen.width * 2, 0, 0);
            // Move the menu off-screen initially
            menuPanel.localPosition = hiddenPosition;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            // Toggle the menu's position
            isVisible = !isVisible;
            menuPanel.localPosition = isVisible ? Vector3.zero : hiddenPosition;
            inventoryManager.LockCameraRotation(isVisible);
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
