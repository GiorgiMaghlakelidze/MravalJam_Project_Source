using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject parent;
    
    public TextMeshProUGUI starCountText;
    public TextMeshProUGUI hintCountText;
    public GameObject badgePanel;
    private Dictionary<int, LevelButton> levelButtons = new Dictionary<int, LevelButton>();
    
    // Start is called before the first frame update
    private void Awake()
    {
        RetrieveLevelButtons();
        SetUpLevels();
        ChangePointsAmount();

    }

    private void ChangePointsAmount()
    {
        int totalStarAmount = levelButtons.Count() * 3;
        int starCount = SaveManager.Instance.GetStarTotalCount();
        int hintPointCount = SaveManager.Instance.GetHintPoints();

        starCountText.text = starCount + "/" + totalStarAmount;
        hintCountText.text = hintPointCount.ToString();
    }

    private void SetUpLevels()
    {
        DisableAllLevels();
        UpdateStars();
    }

    private void DisableAllLevels()
    {
        foreach (var keyValuePair in levelButtons)
        {
            keyValuePair.Value.gameObject.GetComponent<Button>().interactable = false;
        }
    }


    void RetrieveLevelButtons()
    {
        // Ensure that the Canvas object is assigned
        if (parent == null)
        {
            Debug.LogError("Canvas not assigned!");
            return;
        }

        // Get all the child buttons of the Canvas
        LevelButton[] buttons = parent.GetComponentsInChildren<LevelButton>();
        buttons = buttons.OrderBy(button => button.transform.GetSiblingIndex()).ToArray();


        int level = 1;
        // Loop through the buttons and save their GameObjects in the dictionary
        foreach (LevelButton button in buttons)
        {
            levelButtons[level] = button; // Save in the dictionary
            level++;
        }
    }
    private void UpdateStars()
    {
        foreach (var keyValuePair in levelButtons)
        {
            
            keyValuePair.Value.gameObject.GetComponent<Button>().interactable = true;
            
            int stars = SaveManager.Instance.GetStars(keyValuePair.Key);
            if (stars == 0)
            {
                break;
            }
            
            if (stars <= 0) continue;
            keyValuePair.Value.star2.gameObject.SetActive(true);
            if (stars <= 1) continue;
            keyValuePair.Value.star1.gameObject.SetActive(true);
            if (stars <= 2) continue;
            keyValuePair.Value.star3.gameObject.SetActive(true);
            if(stars <= 3) continue;
            foreach (Transform badge in badgePanel.transform)
            {
                if (badge.gameObject.name == "Level" + keyValuePair.Key + "Badge")
                {
                    badge.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                }
            }

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
