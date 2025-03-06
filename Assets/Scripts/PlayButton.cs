using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (SaveManager.Instance.SaveDataIsEmpty())
        {
            Debug.Log("it was empty");
            text.text = "New Game";
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
            SaveManager.Instance.IncreaseHintPoints();
        }
        else
        {
            Debug.Log("it was full");
            text.text = "Continue";
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
