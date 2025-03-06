using UnityEngine;
using UnityEngine.UI;

public class SmoothWheelScroll : MonoBehaviour
{
    public ScrollRect scrollRect;   // Reference to your ScrollRect
    public float scrollSpeed = 10f; // How fast the content scrolls
    public float smoothTime = 0.1f; // How smooth the scroll is

    private float _velocity = 0f;    // Smooth scroll velocity (used for smooth movement)

    void Update()
    {
        // Get mouse wheel scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f) // Only act if the scroll wheel is moved
        {
            // Calculate the new target position for the content based on scroll input
            float targetPosition = scrollRect.content.anchoredPosition.y - scrollInput * scrollSpeed;

            // Smoothly move the content position using Mathf.SmoothDamp
            float newPos = Mathf.SmoothDamp(scrollRect.content.anchoredPosition.y, targetPosition, ref _velocity, smoothTime);

            // Clamp the new position to keep it within valid bounds
            newPos = Mathf.Clamp(newPos, 0, scrollRect.content.rect.height - scrollRect.viewport.rect.height);

            // Apply the smoothed position to the content
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, newPos);
        }
    }
}