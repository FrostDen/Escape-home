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

    private float showUpDistance = 10f;
    private RaycastHit currentHit;
    private bool isObjectVisible;


    public bool isDead;

    public float maxCoughFrequency = 60f; // Adjust this value to decrease the maximum cough frequency
    public float minCoughFrequency = 1f; // Adjust this value to increase the minimum cough frequency
    public float coughFrequency;
    private float timeSinceLastCough = 1f;
    public float coughCooldown = 1f; // Adjust this value to set the minimum time between coughs


    public GameObject questPanel; // Reference to the quest panel in the UI
    public TextMeshProUGUI questText; // Reference to the text element displaying the current quest/task

    private int currentQuestIndex = 0; // Index of the current quest/task
    private string[] quests = {
        //"Find the mobile phone",
        "N·jdi mobil",
        //"Find the charger for mobile phone",
        "N·jdi nabÌjaËku od mobilu",
        //"You must charge the grandma's mobile phone",
        "MusÌö nabiù babkin mobil",
        //"Find correct pincode",
        "N·jdi spr·vny pin kÛd",
        //"Find key from the front door",
        "N·jdi kæ˙Ë od vchodov˝ch dverÌ",
        //"You must wear a facemask",
        "MusÌö maù na sebe r˙öko",
        //"Find the test",
        "N·jdi test",
        //"You need to get tested"
        "Potrebujeö sa otestovaù"
    }; // Array of tasks

    private bool mobilePhoneFound = false;
    public bool mobilePhoneCharged = false;
    private bool frontDoorKeyFound = false;
    private bool facemaskWorn = false;
    private bool testedForDisease = false;


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

        if (questPanel != null && questText != null)
        {
            questPanel.SetActive(true); // Ensure the quest panel is active
            UpdateQuest(); // Update the quest/task text
        }
    }

    void UpdateQuest()
    {
        if (currentQuestIndex < quests.Length)
        {
            //questText.text = "Task: " + quests[currentQuestIndex];
            questText.text = "⁄loha: " + quests[currentQuestIndex];
        }
        else
        {
            //questText.text = "All quests completed";
            questText.text = "Vöetky ˙lohy splnenÈ";
            //questPanel.SetActive(false); // Hide the quest panel if all quests are completed
        }
    }

    public void NextQuest()
    {
        currentQuestIndex++;
        UpdateQuest();
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

        // Check conditions to complete tasks
        if (!mobilePhoneFound && physicsObjects.Any(obj => obj.gameObject.CompareTag("Phone") && obj.isGrabbed))
        {
            mobilePhoneFound = true;
            NextQuest();
        }
        if (!mobilePhoneCharged && physicsObjects.Any(obj => obj.gameObject.CompareTag("Charger") && obj.isGrabbed))
        {
            mobilePhoneCharged = true;
            NextQuest();
        }

        if (!mobilePhoneCharged && physicsObjects.Any(obj => obj.gameObject.CompareTag("Charger") && obj.isGrabbed))
        {
            mobilePhoneCharged = true;
            NextQuest();
        }

        // Create an array to store raycast hits
        RaycastHit[] hits = new RaycastHit[10]; // Adjust the size as needed

        // Perform the raycast and store the number of hits
        int numHits = Physics.RaycastNonAlloc(playerCameraTransform.position, playerCameraTransform.forward, hits, showUpDistance, pickUpLayerMask, QueryTriggerInteraction.Ignore);

        // Loop through all hits and process them
        for (int i = 0; i < numHits; i++)
        {
            RaycastHit hit = hits[i];

            // Check if the hit object is visible and within range
            if (hit.collider != null && Vector3.Distance(hit.transform.position, playerCameraTransform.position) <= showUpDistance)
            {
                currentHit = hit;
                isObjectVisible = true;
                OnPointerEnter(new PointerEventData(EventSystem.current)); // Pass a new PointerEventData
                break; // Exit the loop after processing the first valid hit
            }
        }

        // If no valid hits were found, consider the object not visible
        if (numHits == 0 || !isObjectVisible)
        {
            isObjectVisible = false;
            OnPointerExit(new PointerEventData(EventSystem.current)); // Pass a new PointerEventData
        }

        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * showUpDistance, Color.green);
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
        if (isObjectVisible)
        {
            bool isMessageSet = false; // Flag to check if a message has been set

            foreach (PhysicsObject physicsObject in physicsObjects)
            {
                if (currentHit.transform != null && currentHit.collider != null && currentHit.collider.gameObject != null) // Check if currentHit.collider.gameObject is not null
                {
                    currentHitItemName = currentHit.collider.gameObject.name;
                    if (currentHit.transform.CompareTag("Facemask"))
                    {
                        OpenMessagePanel("stlaË [LMB] chytiù / [E] nasadiù " + currentHitItemName);
                        //OpenMessagePanel("press [LMB] to take / [E] to grab " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true
                    }

                if (currentHit.transform.CompareTag("Phone"))
                    {
                        OpenMessagePanel("stlaË [LMB] chytiù " + currentHitItemName);
                        //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true

                        if (physicsObject.isGrabbed == true)
                        {
                            OpenMessagePanel("pripoj adaptÈr do telefÛnu k nabitiu");
                            //OpenMessagePanel("connect charger to charge the phone");
                        }
                    }

                    else if (currentHit.transform.CompareTag("Charger"))
                    {
                        OpenMessagePanel("stlaË [LMB] chytiù " + currentHitItemName);
                        //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true

                        if (physicsObject.isGrabbed == true)
                        {
                            OpenMessagePanel("pripoj z·strËku do z·suvky k nabitiu telefÛna");
                            //OpenMessagePanel("connect phone to charge it");
                        }
                    }
                    else if (currentHit.transform.CompareTag("CovidTest"))
                    {
                        OpenMessagePanel("stlaË [LMB] chytiù " + currentHitItemName);
                        //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true

                        if (physicsObject.isGrabbed == true)
                        {
                            OpenMessagePanel("otestovaù vzorku soplÌka [LMB] " + currentHitItemName);
                            //OpenMessagePanel("test a sample of snotty [LMB] " + currentHitItemName);
                        }
                    }
                    else if (currentHit.transform.CompareTag("Radio"))
                    {
                        OpenMessagePanel("stlaË [LMB] chytiù / [F] zapn˙ù/vypn˙ù " + currentHitItemName);
                        ///OpenMessagePanel("press [E] to grab / [F] to turn on/off " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true

                        if (physicsObject.isGrabbed == true)
                        {
                            OpenMessagePanel("podrû [MMB] prezrieù si " + currentHitItemName);
                            //OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                        }
                    }
                    else if (currentHit.transform.CompareTag("Inspect retrievable"))
                    {
                        OpenMessagePanel("stlaË [LMB] chatiù / [E] vypiù " + currentHitItemName);
                        //OpenMessagePanel("press [LMB] to take / [E] to grab " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true

                        if (physicsObject.isGrabbed == true)
                        {
                            OpenMessagePanel("podrû [MMB] prezrieù si " + currentHitItemName);
                            //OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                        }
                    }
                    else if (currentHit.transform.CompareTag("Flashlight"))
                    {
                        OpenMessagePanel("stlaË [LMB] chytiù " + currentHitItemName);
                        //OpenMessagePanel("press [E] to grab " + currentHitItemName);
                        Debug.Log(currentHitItemName);
                        isMessageSet = true; // Set the flag to true

                        if (physicsObject.isGrabbed == true)
                        {
                            OpenMessagePanel("stlaË [F] zapn˙ù/vypn˙ù " + currentHitItemName);
                            //OpenMessagePanel("press [F] to turn on/off " + currentHitItemName);
                        }
                    }
                    if (isMessageSet) break; // Exit the loop if a message has been set
                }
            }

            // If no message has been set, close the message panel
            if (!isMessageSet)
            {
                CloseMessagePanel();
            }
        }
        else
        {
            CloseMessagePanel();
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