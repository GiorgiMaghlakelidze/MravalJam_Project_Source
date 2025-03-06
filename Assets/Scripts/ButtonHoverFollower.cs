using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PriceFadeEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button; // Assign the Button component
    public CanvasGroup priceCanvasGroup; // Assign the CanvasGroup of the "Price" object

    private Color normalColor;
    private Color highlightedColor;
    private float fadeDuration;

    private Coroutine fadeCoroutine;

    void Start()
    {
        // Get the Button's ColorBlock values
        ColorBlock colorBlock = button.colors;
        normalColor = colorBlock.normalColor;
        highlightedColor = colorBlock.highlightedColor;
        fadeDuration = colorBlock.fadeDuration; // Get the button's fade duration
        SetInitialAlpha(priceCanvasGroup);
    }

    private void SetInitialAlpha(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = normalColor.a;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(highlightedColor.a); // Use the highlighted alpha
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(normalColor.a); // Use the normal alpha
    }

    private void StartFade(float targetAlpha)
    {
        // Stop any ongoing fade
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(priceCanvasGroup, targetAlpha));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // Interpolate alpha
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha is fully applied
        canvasGroup.alpha = targetAlpha;
    }
}