using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    private string _saveFilePath; // Path to save file
    private SaveData _saveData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        LoadProgress(); // Load existing data or create a new save
    }

    // Check if save data exists
    public bool SaveDataExists()
    {
        return File.Exists(_saveFilePath);
    }

    public bool SaveDataIsEmpty()
    {
        if (!SaveDataExists()) return true;
        string fileContent = File.ReadAllText(_saveFilePath);
        return string.IsNullOrEmpty(fileContent);
    }

    // Save progress
    public void SaveProgress()
    {
        // Serialize the SaveData object directly (no need for simplified class)
        string json = JsonConvert.SerializeObject(_saveData, Formatting.Indented); // Use Newtonsoft.Json
        File.WriteAllText(_saveFilePath, json); // Save to file
        
        Debug.Log("Progress Saved!");
    }

    // Load progress
    public void LoadProgress()
    {
        if (SaveDataExists())
        {
            string json = File.ReadAllText(_saveFilePath); // Read JSON from file
            
            _saveData = JsonConvert.DeserializeObject<SaveData>(json); // Deserialize into SaveData
            
            Debug.Log("Progress Loaded!");
        }
        else
        {
            _saveData = new SaveData(); // Create a new save if none exists
            Debug.Log("No save data found. New save created.");
        }
    }

    // Update stars for a level
    public void UpdateStars(int level, int stars)
    {
        if (_saveData.levelStars.ContainsKey(level))
        {
            _saveData.levelStars[level] = Mathf.Max(_saveData.levelStars[level], stars); // Save the best result
        }
        else
        {
            _saveData.levelStars.Add(level, stars);
        }

        SaveProgress(); // Save changes
    }

    // Update hint points
    public void UpdateHintPoints(int points)
    {
        _saveData.hintPoints = points;
        SaveProgress();
    }

    // Increase hint points
    public void IncreaseHintPoints()
    {
        _saveData.hintPoints++;
        SaveProgress();
    }
    
    public void BuyHint()
    {
        if (CanBuyHint())
        {
            _saveData.hintPoints -= 3;
            SaveProgress();
        }
        else
        {
            Debug.Log("couldnt buy hint");
        }
    }

    public bool CanBuyHint()
    {
        if (_saveData.hintPoints < 3) return false;
        else return true;
    }
    // Get stars for a level
    public int GetStars(int level)
    {
        return _saveData.levelStars.ContainsKey(level) ? _saveData.levelStars[level] : 0;
    }

    // Get hint points
    public int GetHintPoints()
    {
        return _saveData.hintPoints;
    }

    public int GetStarTotalCount()
    {
        int starCount = 0;
        foreach (var saveDataLevelStar in _saveData.levelStars)
        {
            if (saveDataLevelStar.Value >= 4)
            {
                starCount += 3;
                continue;
            }
            starCount += saveDataLevelStar.Value;
        }
        return starCount;
    }
}

