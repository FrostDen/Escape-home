using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public HUD hud;
    public Button resumeBtn;
    public Button restartBtn;
    public Button menuBtn;
    public RectTransform menuPanel; // Reference to your UI menu panel
    private Vector3 hiddenPosition; // Off-screen position
    private bool isVisible;

    public bool canPressEscape = true; // Flag to allow pressing Escape

    public Texture2D defaultCursor;
    public Texture2D lookCursor;
    public Texture2D grabCursor;
    public Texture2D inspectCursor;
    public Texture2D pointCursor;

    public void Start()
    {
        if (menuPanel != null)
        {
            // Get the off-screen position
            hiddenPosition = new Vector3(Screen.width * 2, 0, 0);
            // Move the menu off-screen initially
            menuPanel.localPosition = hiddenPosition;
            Cursor.visible = true;
            Cursor.SetCursor(pointCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPressEscape)
        {
            ToggleMenu();
        }

        //// Check if menu is open and disable inventory key if applicable
        //if (isVisible && Input.GetKeyDown(KeyCode.I))
        //{
        //    inventoryManager.ToggleInventoryPanel(); // Set parameter to false to prevent actual opening
        //}
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }

    public void IntroScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        //if (inventoryManager.isInventoryOpen == false) // Ensure inventory isn't already open
        //{
            // Toggle the menu's position
            isVisible = !isVisible;
            menuPanel.localPosition = isVisible ? Vector3.zero : hiddenPosition;

            // Use Cursor.lockState and Input.GetAxis for smoother locking
            float lockValue = Mathf.Clamp01(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"));

            // Set Cursor.lockState based on lockValue
            Cursor.lockState = lockValue >= 0.5f ? CursorLockMode.Locked : CursorLockMode.None;

            // Set Cursor.visible based on isVisible
            Cursor.visible = isVisible;
            inventoryManager.LockCameraRotation(isVisible);
        //}
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

    // Call this method when the player dies to disable Escape button
    public void DisableEscapeButton()
    {
        canPressEscape = false;
    }

    // Call this method when the player respawns or restarts to enable Escape button
    public void EnableEscapeButton()
    {
        canPressEscape = true;
    }
}
