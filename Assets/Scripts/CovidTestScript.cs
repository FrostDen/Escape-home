using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CovidTestScript : MonoBehaviour
{
    public Button testButton;
    public Slider progressBar;
    public TextMeshProUGUI resultText;
    public float testDuration = 30f;
    public float fadeDuration = 10f; // Time taken for the slider to fade out after reaching full value
    public float textAppearDuration = 10f; // Time taken for the text to appear

    private bool isTesting = false;
    private float testTimer = 0f;
    private bool fadingOut = false;
    private float fadeTimer = 0f;
    private bool textAppearing = false;
    private float textAppearTimer = 0f;
    public bool isPositive = false;

    void Start()
    {
        testButton.onClick.AddListener(StartTest);
        isPositive = false;
    }

    void Update()
    {
        if (isTesting)
        {
            testTimer += Time.deltaTime;
            progressBar.value = testTimer / testDuration;

            if (testTimer >= testDuration && !fadingOut)
            {
                fadingOut = true;
                FadeOutSlider();
                isPositive = false;
            }
        }

        if (fadingOut)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            SetSliderAlpha(alpha);

            if (fadeTimer >= fadeDuration && !textAppearing)
            {
                textAppearing = true;
                AppearText();
                isPositive = true;
}
        }

        if (textAppearing)
        {
            textAppearTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, textAppearTimer / textAppearDuration);
            SetTextAlpha(alpha);

            if (textAppearTimer >= textAppearDuration)
            {
                textAppearing = false;
            }
        }
    }

    public void StartTest()
    {
        isTesting = true;
        isPositive = false;
    }

    void FadeOutSlider()
    {
        // Start fading out the slider
        fadeTimer = 0f;
    }

    void AppearText()
    {
        resultText.gameObject.SetActive(true);
    }

    void SetSliderAlpha(float alpha)
    {
        // Set the alpha value of the slider's fill image
        var fillImage = progressBar.fillRect.GetComponent<Image>();
        Color color = fillImage.color;
        color.a = alpha;
        fillImage.color = color;
    }

    void SetTextAlpha(float alpha)
    {
        // Set the alpha value of the result text
        Color color = resultText.color;
        color.a = alpha;
        resultText.color = color;
    }
}
