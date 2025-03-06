using System.Collections.Generic;

[System.Serializable]
public class SaveDataSimplified
{
    public int hintPoints;
    public List<LevelStar> levelStars; // List of LevelStar, a custom class to store key-value pairs.

    public SaveDataSimplified()
    {
        hintPoints = 0;
        levelStars = new List<LevelStar>();
    }
}

[System.Serializable]
public class LevelStar
{
    public int level;
    public int stars;

    public LevelStar(int level, int stars)
    {
        this.level = level;
        this.stars = stars;
    }
}