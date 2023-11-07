using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public GameObject GameOverText;
    public Image sanityBar;
    public float maxTime = 5f;

    public Animator transition;
    public float transitionTime = 3f;

    public GameObject MessagePanel; // should be static but it can't find on Awake() method so doesn't work...

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private string currentHitItemName;
    public TextMeshProUGUI messageText;

    private float showUpDistance = 2f;
    private RaycastHit currentHit;

    //void Awake()
    //{
    //    MessagePanel = GameObject.Find("MessagePanel");
    //}

    void Start()
    {
        remainingTime = maxTime;
        messageText = MessagePanel.GetComponentInChildren<TextMeshProUGUI>();
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
        if (MessagePanel != null)
        {
            MessagePanel.SetActive(false);
        }
    }

    void GameOver()
    {
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentHit.transform != null)
        {
            currentHitItemName = currentHit.transform.name;
            OpenMessagePanel("[LMB] to pick up/[E] to grab " + currentHitItemName);
            Debug.Log(currentHitItemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseMessagePanel();
    }
}

