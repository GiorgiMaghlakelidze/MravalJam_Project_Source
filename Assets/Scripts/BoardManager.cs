using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public List<StickerScript> stickersOnBoard = new List<StickerScript>();

    public int sumRhythm;
    public int sumHarmony;
    public int sumMelody;
    public int sumSpecial;

    public bool missMatch = false;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddStickerToBoard(StickerScript sticker)
    {
        if (!stickersOnBoard.Contains(sticker))
        {
            stickersOnBoard.Add(sticker);
            UpdateStats();
            StartCoroutine(SoundManager.Instance.PlaySoundForSticker(sticker.id));
        }
    }

    public void RemoveStickerFromBoard(StickerScript sticker)
    {
        if (stickersOnBoard.Contains(sticker))
        {
            stickersOnBoard.Remove(sticker);
            UpdateStats();
            StartCoroutine(SoundManager.Instance.StopSoundForSticker(sticker.id));
        }
    }

    private void UpdateStats()
    {
        
        float multiplier = 1.0f;
        for (int i = 0; i < stickersOnBoard.Count; i++)
        {
            if (stickersOnBoard[i].group != GameManager.Instance.level)
            {
                missMatch = true;
                multiplier = 0.50f;
                break;
            };
        }
        sumRhythm = 0;
        sumHarmony = 0;
        sumMelody = 0;
        sumSpecial = 0;
        foreach (StickerScript sticker in stickersOnBoard)
        {
            sumRhythm += sticker.stats.rhythm;
            sumHarmony += sticker.stats.harmony;
            sumMelody += sticker.stats.melody;
            sumSpecial += sticker.stats.speical;
        }
        sumRhythm = (int)(sumRhythm * multiplier);
        sumHarmony = (int)(sumHarmony * multiplier);
        sumMelody = (int)(sumMelody * multiplier);
        sumSpecial = (int)(sumSpecial * multiplier);
        if(sumRhythm > 100) sumRhythm = 100;
        if(sumHarmony > 100) sumHarmony = 100;
        if(sumMelody > 100) sumMelody = 100;
        if(sumSpecial > 100) sumSpecial = 100;
        StatsManager.Instance.UpdateStats(sumRhythm, sumHarmony, sumMelody, sumSpecial);
        GameManager.Instance.UpdateStars();
    }
    
}