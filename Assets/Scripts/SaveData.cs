using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int hintPoints; // Total hint points
    public Dictionary<int, int> levelStars; // Key: Level number, Value: Stars unlocked

    public SaveData()
    {
        
        hintPoints = 0;
        levelStars = new Dictionary<int, int>();
    }
}