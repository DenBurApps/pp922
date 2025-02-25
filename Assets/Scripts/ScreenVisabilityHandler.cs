using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenVisabilityHandler : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 0.3f;
    [SerializeField] private Ease _showEase = Ease.OutQuad;
    [SerializeField] private Ease _hideEase = Ease.InQuad;

    private CanvasGroup _canvasGroup;
    private Sequence _currentAnimation;

    public event Action OnShowScreenStart;
    public event Action OnHideScreenStart;
    public event Action OnShowScreenComplete;
    public event Action OnHideScreenComplete;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        KillAnimation();
    }

    public void DisableScreen()
    {
        OnHideScreenStart?.Invoke();
        KillAnimation();
        
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        
        _currentAnimation = DOTween.Sequence();
        _currentAnimation.Append(_canvasGroup.DOFade(0f, _animationDuration).SetEase(_hideEase))
            .OnComplete(() => {
                OnHideScreenComplete?.Invoke();
            });
    }

    public void EnableScreen()
    {
        gameObject.SetActive(true);
        
        OnShowScreenStart?.Invoke();
        
        KillAnimation();
        
        _canvasGroup.alpha = 0f;
        
        _currentAnimation = DOTween.Sequence();
        _currentAnimation.Append(_canvasGroup.DOFade(1f, _animationDuration).SetEase(_showEase))
            .OnComplete(() => {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                OnShowScreenComplete?.Invoke();
            });
    }

    public void SetTransperent()
    {
        KillAnimation();
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0.01f;
        _canvasGroup.blocksRaycasts = false;
    }
    
    private void KillAnimation()
    {
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }
    }
}