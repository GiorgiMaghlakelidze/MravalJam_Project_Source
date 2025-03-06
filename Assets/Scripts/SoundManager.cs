using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    

    #region Variables
    [System.Serializable]
    public class StickerSound
    {
        public int id;
        public AudioClip soundClip;
        
        [NonSerialized]
        public bool FadingIn = false;
    }

    public List<StickerSound> stickerSounds = new List<StickerSound>();
    public float soundFadingTime = 4f; // Interval in seconds

    private Dictionary<int, AudioSource> _soundDictionary = new Dictionary<int, AudioSource>();
    #endregion
    
    #region Private Methods

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeSounds();
        
    }

    private void InitializeSounds()
    {
        foreach (var stickerSound in stickerSounds)
        {
            GameObject audioObject = new GameObject("audioObject"+stickerSound.id);
            audioObject.transform.SetParent(transform);
            
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = stickerSound.soundClip;
            audioSource.loop = true;
            audioSource.Play();
            audioSource.volume = 0f;
            _soundDictionary.Add(stickerSound.id, audioSource);
        }
    }

    #endregion
    
    #region Public Methods

    
    public IEnumerator PlaySoundForSticker(int stickerID)
    {
        
        AudioSource audioSource = _soundDictionary[stickerID];
        StickerSound ourStickerSound = stickerSounds[stickerID];
        stickerSounds[stickerID].FadingIn = true;

        Console.Out.WriteLine(stickerSounds[stickerID].FadingIn);
        while (audioSource.volume < 1f)
        {
            if (!stickerSounds[stickerID].FadingIn) break;
            audioSource.volume += soundFadingTime * Time.deltaTime;
            yield return null;
        }
        if (ourStickerSound.FadingIn)
            audioSource.volume = 1f;
    }

    public IEnumerator StopSoundForSticker(int stickerID)
    {
        AudioSource audioSource = _soundDictionary[stickerID];
        StickerSound ourStickerSound = stickerSounds[stickerID];
        stickerSounds[stickerID].FadingIn = false;

        Console.Out.WriteLine(stickerSounds[stickerID].FadingIn);
        while (audioSource.volume > 0f)
        {
            if (stickerSounds[stickerID].FadingIn) break;
            audioSource.volume -= soundFadingTime * Time.deltaTime;
            yield return null;
        }
        if (!ourStickerSound.FadingIn)
            audioSource.volume = 0f;
    }
    
    
    public void StopAllSounds()
    {
        foreach (var keyValuePair in _soundDictionary)
        {
            StartCoroutine(StopSoundForSticker(keyValuePair.Key));
        }
    }
    
    public void StopAllSoundsInstantly()
    {
        foreach (var keyValuePair in _soundDictionary)
        {
            keyValuePair.Value.volume = 0f;
            //fading ins wyvets DAVAMATE!
            stickerSounds[keyValuePair.Key].FadingIn = false;
        }
    }

    public void PlayOneSoundInstantly(int stickerID)
    {
        _soundDictionary[stickerID].volume = 1f;
    }
    public void StopOneSoundInstantly(int stickerID)
    {
        _soundDictionary[stickerID].volume = 0f;
    }
    public void ContinueAllSounds()
    {
        List<int> onBoardStickerIds = new List<int>();
        foreach (var stickerScript in BoardManager.Instance.stickersOnBoard)
        {
            onBoardStickerIds.Add(stickerScript.id);
        }

        foreach (var keyValuePair in _soundDictionary)
        {
            if(onBoardStickerIds.Contains(keyValuePair.Key))
            StartCoroutine(PlaySoundForSticker(keyValuePair.Key));
        }
    }
    #endregion
    
}