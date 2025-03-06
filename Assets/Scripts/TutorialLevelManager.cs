using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : MonoBehaviour
{

    public GameObject focusImage;

    public GameObject firstButton;

    public List<GameObject> ParentablePoints;
    
    public List<GameObject> TutorialPanels;
    // Start is called before the first frame update

    private void Awake()
    {
    }
    void Start()
    {
        int stars = SaveManager.Instance.GetStars(1);
        if (stars != 0) return;
        
        UIManager.Instance.mainCanvas.blocksRaycasts = false;
        
        StartCoroutine(WaitForAnyInput());
    }

    // Update is called once per frame
    private IEnumerator WaitForAnyInput()
    {
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // SaveManager.Instance.IncreaseHintPoints();
        // UIManager.Instance.UpdateHintPointsText();
        yield return new WaitForSeconds(1f);
        StartCoroutine(UIManager.Instance.FocusOnTarget( firstButton.GetComponent<RectTransform>(), 6000, 1.2f));
        
        yield return new WaitForSeconds(0.5f);
        TutorialPanels[0].SetActive(true);
        float size = 6000f;

        for (int i = 1; i < 5; i++)
        {
            // Wait until any key or mouse click is pressed
            yield return StartCoroutine(WaitForKeyPress());

            // Perform actions after input is received
            if (i == 1) size = 12000f;
            if (i == 3) size = 6000f;
            UIManager.Instance.TutorialFocus(size, ParentablePoints[i]);
            TutorialPanels[i - 1].SetActive(false);
            TutorialPanels[i].SetActive(true);
        }
        Debug.Log("For loop finished");
        yield return StartCoroutine(WaitForKeyPress());
        TutorialPanels[4].SetActive(false);
        // After loop finishes, disable raycasts and reset focus
        UIManager.Instance.mainCanvas.blocksRaycasts = true;
        StartCoroutine(UIManager.Instance.FocusOffTarget(0.4f));
        Debug.Log("finished finished");
    }

// Helper method to wait for user input
    private IEnumerator WaitForKeyPress()
    {
        while (!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            yield return null; // Wait for the next frame
        }

        // Wait for input release to avoid skipping iterations due to key hold
        yield return new WaitUntil(() => !Input.anyKey && !Input.GetMouseButton(0));
    }
    void Update()
    {
        // while (!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        // {
        //     StartCoroutine(UIManager.Instance.FocusOffTarget(1.2f));
        // }
    }
}
