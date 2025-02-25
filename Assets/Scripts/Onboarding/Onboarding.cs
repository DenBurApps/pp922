using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Onboarding : MonoBehaviour
{
    [SerializeField] private List<GameObject> _steps;
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private float _fadeOutDuration = 0.3f;
    private int _currentIndex = 0;

    public event Action Shown;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Onboarding"))
        {
            Shown?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            ShowOnboarding();
        }
    }

    private void ShowOnboarding()
    {
        _currentIndex = 0;
        foreach (var item in _steps)
        {
            CanvasGroup canvasGroup = GetOrAddCanvasGroup(item);
            canvasGroup.alpha = 0;
            item.SetActive(false);
        }

        _steps[_currentIndex].SetActive(true);
        GetOrAddCanvasGroup(_steps[_currentIndex])
            .DOFade(1, _fadeInDuration)
            .SetEase(Ease.OutQuad);
    }

    public void ShowNextStep()
    {
        if (_currentIndex >= _steps.Count - 1)
        {
            PlayerPrefs.SetInt("Onboarding", 1);

            GetOrAddCanvasGroup(_steps[_currentIndex])
                .DOFade(0, _fadeOutDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => gameObject.SetActive(false));
            
            Shown?.Invoke();
            return;
        }

        GetOrAddCanvasGroup(_steps[_currentIndex])
            .DOFade(0, _fadeOutDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                _steps[_currentIndex].SetActive(false);
                _currentIndex++;

                _steps[_currentIndex].SetActive(true);
                GetOrAddCanvasGroup(_steps[_currentIndex])
                    .DOFade(1, _fadeInDuration)
                    .SetEase(Ease.OutQuad);
            });
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}