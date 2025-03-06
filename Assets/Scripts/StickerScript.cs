using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickerScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler , IPointerExitHandler
{
    #region Public Variables
    public int id;
    public int group;
    public Stats stats;
    public StickerType stickerType;
    
    #endregion

    #region Movement Variables
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Vector3 _originalPosition;
    private Transform _originalParent;
    private GameObject _topLevelCanvas;
    private bool _isOnBoard; // Track if the sticker is on the board
    private bool _isHovered;

    #endregion
    private void Awake()
    {
        #region Movement Setup
        _isOnBoard = false;
        _topLevelCanvas = GameObject.FindWithTag("TopLevelCanvas");
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalParent = transform.parent;
        _originalPosition = _rectTransform.anchoredPosition;
        // Find the Canvas in the parent hierarchy
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            Debug.LogError("No Canvas found in parent hierarchy for drag-and-drop!");
        }
        #endregion
    }

    private void Update()
    {
        CheckForCtrl();
    }

    private void CheckForCtrl()
    {
        if (_isHovered && Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Mute all sounds except the one associated with this sticker ID
            SoundManager.Instance.StopAllSoundsInstantly();
            SoundManager.Instance.PlayOneSoundInstantly(id);
            StartCoroutine(UIManager.Instance.FocusOnTarget(gameObject.GetComponent<RectTransform>(), 6000, 1.2f));
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            // Restore all sounds when Ctrl is released
            SoundManager.Instance.StopOneSoundInstantly(id);
            SoundManager.Instance.ContinueAllSounds();
            StartCoroutine(UIManager.Instance.FocusOffTarget(0.4f));

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
    }
    #region Movement Methods
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_topLevelCanvas.transform, true);
        if (_isOnBoard)
        {
            BoardManager.Instance.RemoveStickerFromBoard(this);
            _isOnBoard = false;
        }
        
        // _originalParent = transform.parent; // Remember the original parent
        _canvasGroup.blocksRaycasts = false; // Allow objects underneath to receive raycasts
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas != null)
        {
            // Move the sticker with the mouse pointer
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true; // Re-enable raycasting

        // Check where the sticker was dropped
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.CompareTag("Board"))
            {
                // If dropped on the Board
                transform.SetParent(raycastResult.gameObject.transform, true);
                _isOnBoard = true;
                BoardManager.Instance.AddStickerToBoard(this);
                break;
            }
        }
        
        if(!_isOnBoard){
            // If not dropped on the Board, return to the Scroll View
            transform.SetParent(_originalParent, false);
            _rectTransform.anchoredPosition = _originalPosition;
            // _isOnBoard = false;
            // Debug.Log("Sticker with id: " + _stickerScript.id + " is off the board!");
        }
    }
    #endregion
}


