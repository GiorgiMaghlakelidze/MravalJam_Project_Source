using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    public TextMeshProUGUI statRhythm;
    public TextMeshProUGUI statHarmony;
    public TextMeshProUGUI statMelody;
    public TextMeshProUGUI statSpecial;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateStats(int sumRhythm, int sumHarmony, int sumMelody, int sumSpecial)
    {
        statRhythm.text = sumRhythm.ToString();
        statHarmony.text = sumHarmony.ToString();
        statMelody.text = sumMelody.ToString();
        if(statSpecial != null) statSpecial.text = sumSpecial.ToString();
    }
}