using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;


    public bool firstStar;
    public bool secondStar;
    public bool thirdStar;
    public bool badge;

    public int level;
    
    public GameObject stickersContainer;
    

    private bool _firstHintRevealed;
    private bool _secondHintRevealed;
    private bool _thirdHintRevealed;
    private bool _needToBuyHint = true;
    private bool _finishedLevel = false;

    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _shuffleCorrectStickers();
    }

    private void _shuffleCorrectStickers()
    {
        List<StickerScript> stickers = new List<StickerScript>();
        foreach (Transform stickerContainer in stickersContainer.transform)
        {
            stickers.Add(stickerContainer.gameObject.transform.GetChild(0).GetComponent<StickerScript>());
        }
        stickers = stickers.Where(item => item.group == level).ToList();
        List<StickerScript> rhythmStickers = stickers.Where(item => item.stickerType == StickerType.Rhythm).ToList();
        List<StickerScript> harmonyStickers = stickers.Where(item => item.stickerType == StickerType.Harmony).ToList();
        List<StickerScript> melodyStickers = stickers.Where(item => item.stickerType == StickerType.Melody).ToList();
        // leaves only one sticker from each type
        int chosenRhythmStickerIndex = Random.Range(0, rhythmStickers.Count);
        int chosenHarmonyStickerIndex = Random.Range(0, harmonyStickers.Count);
        int chosenMelodyStickerIndex = Random.Range(0, melodyStickers.Count);
        
        foreach (var rhythmSticker in rhythmStickers)
        {
            rhythmSticker.gameObject.transform.parent.gameObject.SetActive(false);
        }
        foreach (var harmonySticker in harmonyStickers)
        {
            harmonySticker.gameObject.transform.parent.gameObject.SetActive(false);
        }
        foreach (var melodySticker in melodyStickers)
        {
            melodySticker.gameObject.transform.parent.gameObject.SetActive(false);
        }
        rhythmStickers[chosenRhythmStickerIndex].gameObject.transform.parent.gameObject.SetActive(true);
        harmonyStickers[chosenHarmonyStickerIndex].gameObject.transform.parent.gameObject.SetActive(true);
        melodyStickers[chosenMelodyStickerIndex].gameObject.transform.parent.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    #region Star control functions
    public void UpdateStars()
    {
        int sumRhythm = int.Parse(StatsManager.Instance.statRhythm.text);
        int sumHarmony = int.Parse(StatsManager.Instance.statHarmony.text);
        int sumMelody = int.Parse(StatsManager.Instance.statMelody.text);
        int sumSpecial = 0;
        if (StatsManager.Instance.statSpecial != null)
        {
            sumSpecial = int.Parse(StatsManager.Instance.statSpecial.text);
        }
        List<int> baseStats = new List<int>();
        baseStats.Add(sumRhythm);
        baseStats.Add(sumHarmony);
        baseStats.Add(sumMelody);

        if (!firstStar)
        {
            if(_wonStar(baseStats, 50)){ 
                firstStar = true;
                // SaveManager.Instance.UpdateStars(level,1); ara
                // SaveManager.Instance.IncreaseHintPoints();
                // UIManager.Instance.ShowFirstStar(); ara
                StartCoroutine(UIManager.Instance.HideHint());
                UIManager.Instance.WriteHintText(2); 
                _needToBuyHint = true;
                // UIManager.Instance.ShowWinUI(); ara
            }
        }

        if (!secondStar)
        {
            if (_wonStar(baseStats, 75))
            {
                secondStar = true;
                // SaveManager.Instance.UpdateStars(level,2);
                // SaveManager.Instance.IncreaseHintPoints();
                // UIManager.Instance.UpdateHintPointsText();
                // UIManager.Instance.ShowSecondStar();
                StartCoroutine(UIManager.Instance.HideHint());
                UIManager.Instance.WriteHintText(3);
                _needToBuyHint = true;
            }
            
        }

        if (!thirdStar)
        {
            if (_wonStar(baseStats, 100))
            {
                thirdStar = true;
                // SaveManager.Instance.UpdateStars(level,3);
                // SaveManager.Instance.IncreaseHintPoints();
                // UIManager.Instance.UpdateHintPointsText();
                // UIManager.Instance.ShowThirdStar();
                StartCoroutine(UIManager.Instance.HideHint());
                UIManager.Instance.WriteHintText(4);
                if (UIManager.Instance.badgeImage == null)
                {
                    _finishedLevel = true;
                }
                _needToBuyHint = true;
            }
        }

        if (sumSpecial >= 100 && UIManager.Instance.BadgeExists())
        {
            badge = true;
            // SaveManager.Instance.UpdateStars(level,4);
            // SaveManager.Instance.IncreaseHintPoints();
            // UIManager.Instance.UpdateHintPointsText();
            // UIManager.Instance.ShowBadge();
            StartCoroutine(UIManager.Instance.HideHint());
            _finishedLevel = true;
        }
    }

    private bool _wonStar(List<int> baseStats, int goal)
    {
        int points = 0;
        foreach (var stat in baseStats)
        {
            if(stat >= goal) points++;
        }
        if (points == 3) return true;
        else return false;
    }

    public void IncreaseHintKeys()
    {
        int sumRhythm = int.Parse(StatsManager.Instance.statRhythm.text);
        int sumHarmony = int.Parse(StatsManager.Instance.statHarmony.text);
        int sumMelody = int.Parse(StatsManager.Instance.statMelody.text);
        int sumSpecial = 0;
        if (StatsManager.Instance.statSpecial != null)
        {
            sumSpecial = int.Parse(StatsManager.Instance.statSpecial.text);
        }
        List<int> baseStats = new List<int>();
        baseStats.Add(sumRhythm);
        baseStats.Add(sumHarmony);
        baseStats.Add(sumMelody);
        
        
        if (!_wonStar(baseStats, 50))
        {
            return;
        }
        SaveManager.Instance.UpdateStars(level,1);
        UIManager.Instance.ShowFirstStar();
        SaveManager.Instance.IncreaseHintPoints();
        UIManager.Instance.ShowWinUI();
        if (!_wonStar(baseStats, 75))
        {
            return;
        }
        SaveManager.Instance.UpdateStars(level,2);
        UIManager.Instance.ShowSecondStar();
        SaveManager.Instance.IncreaseHintPoints();
        if (!_wonStar(baseStats, 100))
        {
            return;
        }
        SaveManager.Instance.UpdateStars(level,3);
        UIManager.Instance.ShowThirdStar();
        SaveManager.Instance.IncreaseHintPoints();
        if (!(sumSpecial>=100 && UIManager.Instance.BadgeExists()))
        {
            return;
        }
        SaveManager.Instance.UpdateStars(level,4);
        SaveManager.Instance.IncreaseHintPoints();
        UIManager.Instance.ShowBadge();
    }
    #endregion

    #region Hint control functions
    public void HintButtonClicked()
    {
        if (_finishedLevel)
        {
            StartCoroutine(UIManager.Instance.ShakeHintButton());
            return;
        }
        UIManager.Instance.hintButton.interactable = true; // Reset interactivity before other logic
        if (_needToBuyHint)
        {
            if (SaveManager.Instance.CanBuyHint())
            {
                SaveManager.Instance.BuyHint();
                UIManager.Instance.UpdateHintPointsText();
                _needToBuyHint = false;
                UIManager.Instance.ShowHint();
            }
            else
            {
                StartCoroutine(UIManager.Instance.ShakeHintButton());
            }
        }
        else
        {
            UIManager.Instance.ShowHint();
        }
    }

    
    #endregion

    void Update()
    {
        
    }
}
