using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("Hint attributes")] 
    public Button hintButton;
    public TextMeshProUGUI hintText;
    public CanvasGroup hintPanel;
    public float hintFadeSpeed;
    public float displayDuration;
    [Space]
    [Header("Hints texts")] 
    public String hintForFirstStar;
    public String hintForSecondStar;
    public String hintForThirdStar;
    public String hintForBadge;
    [Space]
    [Header("Level goal attributes")] 
    public GameObject endLevelButton;
    [FormerlySerializedAs("endLevelText")] public TextMeshProUGUI victoryText;
    public GameObject firstStarImage;
    public GameObject secondStarImage;
    public GameObject thirdStarImage;
    public CanvasGroup firstStarImageMini;
    public CanvasGroup secondStarImageMini;
    public CanvasGroup thirdStarImageMini;
    #nullable enable
        public GameObject? badgeImage;
        public CanvasGroup? badgeImageMini;
    #nullable disable
    [Space] [Header("Points")] 
    public TextMeshProUGUI hintPointsText;
    private Coroutine _fadeOutCoroutine;
    
    [Space]
    [Header("Pause menu attributes")] 
    public CanvasGroup menuCanvasGroup;
    public GameObject pauseScreen;
    public GameObject victoryScreen;
    private bool _isPaused = false;
    
    [Space]
    [Header("Others")]
    public RectTransform focusObject;

    public CanvasGroup mainCanvas;
    
    private Vector2 _initialFocusSize = new Vector2(110000, 110000);
    private bool _isFocusing;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LevelGoalUISetup();
        HintUISetup();
        _initialFocusSize = focusObject.sizeDelta;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !victoryScreen.activeSelf)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        pauseScreen.SetActive(_isPaused);
        menuCanvasGroup.blocksRaycasts = !_isPaused;
        if (_isPaused)
        {
            SoundManager.Instance.StopAllSounds();
        }
        else
        {
            SoundManager.Instance.ContinueAllSounds();
        }
    }


    private void HintUISetup()
    {
        hintPanel.gameObject.SetActive(false);
        hintPanel.alpha = 0;
        WriteHintText(1);
        UpdateHintPointsText();
    }

    private void LevelGoalUISetup()
    {
        // endLevelButton.SetActive(false);
        firstStarImage.SetActive(false);
        secondStarImage.SetActive(false);
        thirdStarImage.SetActive(false);
        firstStarImageMini.gameObject.SetActive(false);
        secondStarImageMini.gameObject.SetActive(false);
        thirdStarImageMini.gameObject.SetActive(false);
        firstStarImageMini.alpha = 0;
        secondStarImageMini.alpha = 0;
        thirdStarImageMini.alpha = 0;
        if (badgeImage != null) badgeImage.SetActive(false);
        if (badgeImageMini != null)
        {
            badgeImageMini.gameObject.SetActive(false);
            badgeImageMini.alpha = 0;
        }
    }

    public void ShowFirstStar()
    {
        // endLevelButton.SetActive(true);
        firstStarImage.SetActive(true);
        // StartCoroutine(FadeInPanel(firstStarImageMini));
    }
    public void ShowSecondStar()
    {
        secondStarImage.SetActive(true);
        // StartCoroutine(FadeInPanel(secondStarImageMini));
    }
    public void ShowThirdStar()
    {
        thirdStarImage.SetActive(true);
        // StartCoroutine(FadeInPanel(thirdStarImageMini));
    }
    public void ShowBadge()
    {
        if (badgeImage != null) badgeImage.SetActive(true);
        // StartCoroutine(FadeInPanel(badgeImageMini));
    }

    public bool BadgeExists()
    {
        if (badgeImage != null) return true;
        else return false;
    }
    public void ShowHint()
    {
        // Stop any active fade-out coroutine
        if (_fadeOutCoroutine != null)
        {
            StopCoroutine(_fadeOutCoroutine);
        }

        // Start fading in the panel
        StartCoroutine(FadeInPanel(hintPanel));

        // Start the fade-out coroutine with a delay
        _fadeOutCoroutine = StartCoroutine(FadeOutAfterDelay());
    }


    private IEnumerator FadeInPanel(CanvasGroup obj)
    {
        obj.gameObject.SetActive(true);
        while (obj.alpha < 1f)
        {
            obj.alpha += Time.deltaTime * hintFadeSpeed;
            yield return null;
        }
        obj.alpha = 1f;
    }

    private IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration); // Wait for X seconds

        // Start fading out the panel
        while (hintPanel.alpha > 0f)
        {
            hintPanel.alpha -= Time.deltaTime * hintFadeSpeed;
            yield return null;
        }

        hintPanel.alpha = 0f;
        hintPanel.gameObject.SetActive(false);
        DeselectHintButton();
    }
    public IEnumerator ShakeHintButton()
    {
        hintButton.interactable = false;
        float shakeDuration = 0.2f;
        float shakeAmount = 4f;
        RectTransform buttonTransform = hintButton.GetComponent<RectTransform>();
        Vector3 originalPosition = buttonTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Calculate shake effect using a sine wave for smooth vibration
            float offsetX = Mathf.Sin(elapsed * 50f) * shakeAmount;
            buttonTransform.anchoredPosition = originalPosition + new Vector3(offsetX, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Return the button to its original position
        buttonTransform.anchoredPosition = originalPosition;
        hintButton.interactable = true;
        DeselectHintButton();
    }

    public IEnumerator HideHint()
    {
        hintText.gameObject.SetActive(false);
        if(_fadeOutCoroutine != null) StopCoroutine(_fadeOutCoroutine);
        while (hintPanel.alpha > 0f)
        {
            hintPanel.alpha -= Time.deltaTime * hintFadeSpeed;
            yield return null;
        }
        
        hintPanel.alpha = 0f;
        hintPanel.gameObject.SetActive(false);
        hintText.gameObject.SetActive(true);
        DeselectHintButton();
    }
    public void WriteHintText(int star)
    {
        if (star == 1)
        {
            hintText.text = hintForFirstStar;
            return;
        }

        if (star == 2)
        {
            hintText.text = hintForSecondStar;
            return;
        }

        if (star == 3)
        {
            hintText.text = hintForThirdStar;
        }
        
        if (star == 4)
        {
            hintText.text = hintForBadge;
        }
    }

    public void UpdateHintPointsText()
    {
        hintPointsText.text = SaveManager.Instance.GetHintPoints().ToString();
    }

    public void DeselectHintButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ShowWinUI()
    {
        victoryText.text = "YOU WIN!";
        victoryText.color = ColorUtility.TryParseHtmlString("#FFDB00", out Color color) ? color : victoryText.color;
    }

    #region Focus circle methods

    // public IEnumerator MoveObjectToTarget(GameObject movingObject, GameObject targetObject, float duration)
    // {
    //     
    // }
    public IEnumerator FocusOnTarget( RectTransform targetObject, float targetSize, float duration)
    {
        _isFocusing = true;
        // Set focusObject as a child of targetObject
        focusObject.gameObject.SetActive(true);
        focusObject.SetParent(targetObject);

        focusObject.localPosition = Vector3.zero;
        focusObject.sizeDelta = _initialFocusSize;
        // Store initial size of focusObject
        Vector2 initialSize = focusObject.sizeDelta;
        focusObject.localPosition = Vector3.zero; // Set position to (0, 0, 0) relative to the new parent

        float timeElapsed = 0f;

        // Smoothly change the size (width and height) over time
        while (timeElapsed < duration)
        {
            if (_isFocusing == false) yield break;
            // Apply Ease Out effect to smoothly transition size
            float t = EaseOut(timeElapsed / duration); // Apply easing to time

            // Adjust both width and height (they are identical)
            focusObject.sizeDelta = new Vector2(Mathf.Lerp(initialSize.x, targetSize, t), Mathf.Lerp(initialSize.y, targetSize, t));

            // Update the time elapsed
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final size is exactly the target size
        focusObject.sizeDelta = new Vector2(targetSize, targetSize);
    }
    private float EaseOut(float t)
    {
        return Mathf.Pow(t - 1f, 5f) + 1f; // Stronger slowdown toward the end
    }
    public IEnumerator FocusOffTarget(float duration)
    {
        _isFocusing = false;
        // Store the current (small) size before growing
        Vector2 initialSize = focusObject.sizeDelta;
        focusObject.localPosition = Vector3.zero; // Reset position to center

        float timeElapsed = 0f;

        // Smoothly grow the size (width and height) over time
        while (timeElapsed < duration)
        {
            if(_isFocusing == true) yield break; 
            float t = EaseIn(timeElapsed / duration); // Apply easing function

            // Grow from initialSize (small) to _initialFocusSize (large)
            focusObject.sizeDelta = new Vector2(Mathf.Lerp(initialSize.x, _initialFocusSize.x, t),
                Mathf.Lerp(initialSize.y, _initialFocusSize.y, t));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final size is exactly the target size
        focusObject.sizeDelta = new Vector2(_initialFocusSize.x, _initialFocusSize.y);
        focusObject.SetParent(mainCanvas.transform);
        focusObject.localPosition = Vector3.zero;
        focusObject.gameObject.SetActive(false);
    }
    private float EaseIn(float t)
    {
        return Mathf.Pow(t, 5f); // Stronger acceleration toward the end
    }

    public void TutorialFocus(float size, GameObject targetObject)
    {
        _isFocusing = false;
        focusObject.gameObject.SetActive(true);
        focusObject.sizeDelta = new Vector2(size, size);
        focusObject.SetParent(targetObject.transform);
        focusObject.localPosition = Vector3.zero;
    }
    #endregion
}
