using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public GameObject GameOverText;
    public Image sanityBar;
    public float maxTime = 5f;

    public Animator transition;
    public float transitionTime = 3f;

    //public static Timer Instance;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    void Start()
    {
        remainingTime = maxTime;
    }

    void Update()
    {
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

        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //void UseConsumable()
    //{
    //    remainingTime += 30f;
    //}

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
}
