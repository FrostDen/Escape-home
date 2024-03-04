using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using FMOD.Studio;
using System.Linq;


public class HUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerInteractions playerInteractions;
    public Winscreen winscreen;
    public List<PhysicsObject> physicsObjects;
    public MenuManager menuManager;
    public GameObject GameOverText;
    public GameObject WinText;
    public Image sanityBar;
    public float maxTime = 5f;

    public Animator transition;
    public float transitionTime = 5f;

    public GameObject MessagePanel; // should be static but it can't find on Awake() method so doesn't work...
    public TextMeshProUGUI messageText;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    [SerializeField] public Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private string currentHitItemName;

    private float showUpDistance = 2f;
    private RaycastHit currentHit;

    public bool isDead;

    public float maxCoughFrequency = 60f; // Adjust this value to decrease the maximum cough frequency
    public float minCoughFrequency = 1f; // Adjust this value to increase the minimum cough frequency
    public float coughFrequency;
    private float timeSinceLastCough = 1f;
    public float coughCooldown = 1f; // Adjust this value to set the minimum time between coughs

    //void Awake()
    //{
    //    MessagePanel = GameObject.Find("MessagePanel");
    //}

    public void Start()
    {
        remainingTime = maxTime;
        messageText = MessagePanel.GetComponentInChildren<TextMeshProUGUI>();
        LockCameraRotation(!isDead);
        originalTimeScale = Time.timeScale; // Store the original time scale

        physicsObjects = FindObjectsOfType<PhysicsObject>().ToList();
    }

    private void PlayCough()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerCough, playerCameraTransform.position);
    }

    void Update()
    {
        #region Timer

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            sanityBar.fillAmount = remainingTime / maxTime;

            // Adjust cough frequency based on remaining time
            if (remainingTime > 60) // More than 1 minute left
            {
                coughFrequency = 1f / 20f; // Cough every 20 seconds
            }
            else if (remainingTime > 30) // Less than 1 minute but more than 30 seconds left
            {
                coughFrequency = 1f / 10f; // Cough every 10 seconds
            }
            else if (remainingTime > 5) // Less than 30 seconds but more than 5 seconds left
            {
                coughFrequency = 1f / 5f; // Cough every 7 seconds
            }
            else // Less than 5 seconds left
            {
                coughFrequency = 1f; // Cough every 1 second
            }

            timeSinceLastCough += Time.deltaTime;

            // Check if enough time has passed since the last cough
            if (timeSinceLastCough >= coughCooldown)
            {
                // Reset the time since last cough
                timeSinceLastCough = 0f;

                // Check if a cough should occur based on the cough frequency
                if (Random.value < coughFrequency)
                {
                    PlayCough();
                }
            }
        }

        else if (remainingTime < 0)
        {
            remainingTime = 0;
            timerText.color = Color.red;
            GameOverText.SetActive(true);
            GameOver();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        #endregion

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, showUpDistance, pickUpLayerMask))
        {
            currentHit = hit;
            OnPointerEnter(new PointerEventData(EventSystem.current)); // Pass a new PointerEventData
        }
        else
        {
            OnPointerExit(new PointerEventData(EventSystem.current)); // Pass a new PointerEventData
        }

        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * showUpDistance, Color.red);
    }

    private float originalTimeScale;
    public float timeScaleFactor = 1.2f;
    public float timeControllerDuration = 10f; // Duration for TimeController method

    public void TimeController()
    {
        Time.timeScale = timeScaleFactor;
        StartCoroutine(ResetTimeScaleAfterDelay());
    }

    public IEnumerator ResetTimeScaleAfterDelay()
    {
        yield return new WaitForSeconds(timeControllerDuration);
        Time.timeScale = originalTimeScale; // Reset time scale to its original value
    }

    public void AddTimeToTimer(float timeToAdd)
    {
        remainingTime += timeToAdd;
        if (remainingTime > maxTime)
        {
            remainingTime = maxTime;
        }
    }

    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);
        messageText.text = text;
    }

    public void CloseMessagePanel() //static
    {
        MessagePanel.SetActive(false);
    }

    void GameOver()
    {
        // Unlock Z-axis constraint
        Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();
        playerRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;

        float forceMagnitude = 1f; // You can adjust the force magnitude as needed
        playerRigidbody.AddForce(Vector3.left * forceMagnitude, ForceMode.Impulse);

        if (playerInteractions.currentlyPickedUpObject != null)
        {
            playerInteractions.BreakConnection();
        }



        LockCameraRotation(isDead);
        StartCoroutine(RestartLevel());
        menuManager.DisableEscapeButton();
    }

    public void WinGame()
    {
        LockCameraRotation(isDead);
        WinText.SetActive(true);
        StartCoroutine(BackToMenu());
        menuManager.DisableEscapeButton();
    }

    IEnumerator RestartLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(1);
    }

    private bool isVisible;

    IEnumerator BackToMenu()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(0);
        isVisible = !isVisible;
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (PhysicsObject physicsObject in physicsObjects)
        {
            if (currentHit.transform != null && currentHit.collider != null && currentHit.collider.gameObject != null) // Check if currentHit.collider.gameObject is not null
            {
                currentHitItemName = currentHit.collider.gameObject.name;
                if (currentHit.transform.CompareTag("Object"))
                {
                    OpenMessagePanel("stla� [LMB] vzia� / [E] chyti� " + currentHitItemName);
                    //OpenMessagePanel("press [LMB] to take / [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);
                }

                if (currentHit.transform.CompareTag("Phone"))
                {
                    OpenMessagePanel("stla� [E] chyti� " + currentHitItemName);
                    //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (physicsObject.isGrabbed == true)
                    {
                        OpenMessagePanel("pripoj adapt�r do telef�nu k nabitiu");
                        //OpenMessagePanel("connect charger to charge the phone");
                    }
                }

                else if (currentHit.transform.CompareTag("Charger"))
                {
                    OpenMessagePanel("stla� [E] chyti� " + currentHitItemName);
                    //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (physicsObject.isGrabbed == true)
                    {
                        OpenMessagePanel("pripoj z�str�ku do z�suvky k nabitiu telef�na");
                        //OpenMessagePanel("connect phone to charge it");
                    }
                }
                else if (currentHit.transform.CompareTag("CovidTest"))
                {
                    OpenMessagePanel("stla� [E] chyti� " + currentHitItemName);
                    //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (physicsObject.isGrabbed == true)
                    {
                        OpenMessagePanel("otestova� vzorku sopl�ka [LMB] " + currentHitItemName);
                        //OpenMessagePanel("test a sample of snotty [LMB] " + currentHitItemName);
                    }
                }
                else if (currentHit.transform.CompareTag("Radio"))
                {
                    OpenMessagePanel("stla� [E] chyti� / [F] zapn��/vypn�� " + currentHitItemName);
                    ///OpenMessagePanel("press [E] to grab / [F] to turn on/off " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (physicsObject.isGrabbed == true)
                    {
                        OpenMessagePanel("podr� [MMB] prezrie� si " + currentHitItemName);
                        //OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                    }
                }
                else if (currentHit.transform.CompareTag("Inspect retrievable"))
                {
                    OpenMessagePanel("stla� [LMB] vzia� / [E] chyti� " + currentHitItemName);
                    //OpenMessagePanel("press [LMB] to take / [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (physicsObject.isGrabbed == true)
                    {
                        OpenMessagePanel("podr� [MMB] prezrie� si " + currentHitItemName);
                        //OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                    }
                }
                else if (currentHit.transform.CompareTag("Flashlight"))
                {
                    OpenMessagePanel("stla� [E] chyti� " + currentHitItemName);
                    //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (physicsObject.isGrabbed == true)
                    {
                        OpenMessagePanel("stla� [F] zapn��/vypn�� " + currentHitItemName);
                        //OpenMessagePanel("press [F] to turn on/off " + currentHitItemName);
                    }
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseMessagePanel();
    }

    public GameObject playerObject;

    public void LockCameraRotation(bool isDead)
    {
        if (playerObject != null)
        {
            MonoBehaviour[] scripts = playerObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().Name.Equals("FirstPersonController"))
                {
                    script.enabled = isDead;
                    break;
                }
            }
        }
    }
}