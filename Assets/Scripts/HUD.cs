using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Winscreen winscreen;
    public List<ObjectGrabbable> objectGrabbables;
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
    }

    void Update()
    {
        #region Timer
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            sanityBar.fillAmount = remainingTime / maxTime;
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
        foreach (ObjectGrabbable grabbable in objectGrabbables)
        {
            //if (grabbable == null)
            //{
            //    Debug.LogError("A grabbable object in the list is not assigned.");
            //    continue;
            //}

            if (currentHit.transform != null)
            {
                if (currentHit.transform.CompareTag("Object"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("press [LMB] to take / [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);
                }

                if (currentHit.transform.CompareTag("Phone"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("[E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (grabbable.isGrabbed == true)
                    {
                        OpenMessagePanel("connect charger to charge it");
                    }
                }

                else if (currentHit.transform.CompareTag("Charger"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("[E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (grabbable.isGrabbed == true)
                    {
                        OpenMessagePanel("connect phone to charge it");
                    }
                }
                else if (currentHit.transform.CompareTag("Inspect"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("press [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (grabbable.isGrabbed == true)
                    {
                        OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                    }
                }
                else if (currentHit.transform.CompareTag("Radio"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("press [E] to grab / [F] to turn on/off " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (grabbable.isGrabbed == true)
                    {
                        OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                    }
                }
                else if (currentHit.transform.CompareTag("Inspect retrievable"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("press [LMB] to take / [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (grabbable.isGrabbed == true)
                    {
                        OpenMessagePanel("hold [RMB] to inspect " + currentHitItemName);
                    }
                }
                else if (currentHit.transform.CompareTag("Flashlight"))
                {
                    currentHitItemName = currentHit.transform.name;
                    OpenMessagePanel("press [E] to grab " + currentHitItemName);
                    Debug.Log(currentHitItemName);

                    if (grabbable.isGrabbed == true)
                    {
                        OpenMessagePanel("press [F] to turn on/off" + currentHitItemName);
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

