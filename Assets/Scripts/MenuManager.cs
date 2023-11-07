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
    public GameObject menuPanel;

    private bool isVisible;

    public void Start()
    {
        menuPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isVisible = !isVisible;
            menuPanel.SetActive(isVisible);
            inventoryManager.LockCameraRotation(isVisible);
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        if (!isVisible)
        {
        menuPanel.SetActive(true);
        }
        else
        {
            menuPanel.SetActive(false);
            inventoryManager.LockCameraRotation(!isVisible);
            Cursor.visible = !isVisible;
            Cursor.lockState = !isVisible ? CursorLockMode.None : CursorLockMode.Locked;
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
