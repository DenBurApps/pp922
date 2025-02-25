using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private Button _addTaskButton;
    [SerializeField] private Button _taskListButton;

    [Header("Animation Settings")] [SerializeField]
    private float _fadeDuration = 0.3f;

    [SerializeField] private Ease _fadeInEase = Ease.OutQuad;
    [SerializeField] private Ease _fadeOutEase = Ease.InQuad;

    private string _version = "Application version:\n";
    private CanvasGroup _settingsCanvasGroup;
    private CanvasGroup _privacyCanvasGroup;
    private CanvasGroup _termsCanvasGroup;
    private CanvasGroup _contactCanvasGroup;
    private CanvasGroup _versionCanvasGroup;
    private Sequence _currentAnimation;

    public event Action AddTaskClicked;
    public event Action TaskListClicked;

    private void Awake()
    {
        ValidateSerializedFields();

        _settingsCanvasGroup = GetOrAddCanvasGroup(_settingsCanvas);
        _privacyCanvasGroup = GetOrAddCanvasGroup(_privacyCanvas);
        _termsCanvasGroup = GetOrAddCanvasGroup(_termsCanvas);
        _contactCanvasGroup = GetOrAddCanvasGroup(_contactCanvas);
        _versionCanvasGroup = GetOrAddCanvasGroup(_versionCanvas);

        HideCanvasImmediate(_settingsCanvas, _settingsCanvasGroup);
        HideCanvasImmediate(_privacyCanvas, _privacyCanvasGroup);
        HideCanvasImmediate(_termsCanvas, _termsCanvasGroup);
        HideCanvasImmediate(_contactCanvas, _contactCanvasGroup);
        HideCanvasImmediate(_versionCanvas, _versionCanvasGroup);

        SetVersion();
    }

    private void ValidateSerializedFields()
    {
        if (_settingsCanvas == null) Debug.LogError("Settings Canvas not assigned in inspector!");
        if (_privacyCanvas == null) Debug.LogError("Privacy Canvas not assigned in inspector!");
        if (_termsCanvas == null) Debug.LogError("Terms Canvas not assigned in inspector!");
        if (_contactCanvas == null) Debug.LogError("Contact Canvas not assigned in inspector!");
        if (_versionCanvas == null) Debug.LogError("Version Canvas not assigned in inspector!");
        if (_versionText == null) Debug.LogError("Version Text not assigned in inspector!");
        if (_addTaskButton == null) Debug.LogError("Add Task Button not assigned in inspector!");
        if (_taskListButton == null) Debug.LogError("Task List Button not assigned in inspector!");
    }

    private void OnEnable()
    {
        if (_taskListButton != null)
            _taskListButton.onClick.AddListener(OnTaskListClicked);

        if (_addTaskButton != null)
            _addTaskButton.onClick.AddListener(OnAddTaskClicked);
    }

    private void OnDisable()
    {
        if (_taskListButton != null)
            _taskListButton.onClick.RemoveListener(OnTaskListClicked);

        if (_addTaskButton != null)
            _addTaskButton.onClick.RemoveListener(OnAddTaskClicked);
    }

    private void OnDestroy()
    {
        if (_currentAnimation != null)
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }

        DOTween.Kill(this.gameObject);
    }

    private void OnAddTaskClicked()
    {
        HideSettingsWithAnimation(() => AddTaskClicked?.Invoke());
    }

    private void OnTaskListClicked()
    {
        HideSettingsWithAnimation(() => TaskListClicked?.Invoke());
    }

    public void ShowPrivacyPolicy()
    {
        ShowCanvasWithAnimation(_privacyCanvas, _privacyCanvasGroup);
    }

    public void ShowTerms()
    {
        ShowCanvasWithAnimation(_termsCanvas, _termsCanvasGroup);
    }

    public void ShowContact()
    {
        ShowCanvasWithAnimation(_contactCanvas, _contactCanvasGroup);
    }

    public void ShowVersion()
    {
        ShowCanvasWithAnimation(_versionCanvas, _versionCanvasGroup);
    }

    public void BackToSettings()
    {
        if (_privacyCanvas != null && _privacyCanvas.activeSelf)
            HideCanvasWithAnimation(_privacyCanvas, _privacyCanvasGroup, ShowSettings);
        else if (_termsCanvas != null && _termsCanvas.activeSelf)
            HideCanvasWithAnimation(_termsCanvas, _termsCanvasGroup, ShowSettings);
        else if (_contactCanvas != null && _contactCanvas.activeSelf)
            HideCanvasWithAnimation(_contactCanvas, _contactCanvasGroup, ShowSettings);
        else if (_versionCanvas != null && _versionCanvas.activeSelf)
            HideCanvasWithAnimation(_versionCanvas, _versionCanvasGroup, ShowSettings);
    }

    private void HideCanvasWithAnimation(GameObject canvas, CanvasGroup canvasGroup, Action onComplete = null)
    {
        if (canvas == null || canvasGroup == null) return;

        if (_currentAnimation != null)
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }

        _currentAnimation = DOTween.Sequence();

        var rectTransform = canvas.GetComponent<RectTransform>();

        _currentAnimation.Join(canvasGroup.DOFade(0f, _fadeDuration))
            .Join(rectTransform.DOScale(0.95f, _fadeDuration))
            .SetEase(_fadeOutEase)
            .OnComplete(() =>
            {
                canvas.SetActive(false);
                onComplete?.Invoke();
            })
            .SetUpdate(true);
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        if (obj == null) return null;

        var canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = obj.AddComponent<CanvasGroup>();
        return canvasGroup;
    }

    private void HideCanvasImmediate(GameObject canvas, CanvasGroup canvasGroup)
    {
        if (canvas == null || canvasGroup == null) return;

        canvas.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    private void SetVersion()
    {
        if (_versionText != null)
            _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        ShowCanvasWithAnimation(_settingsCanvas, _settingsCanvasGroup);
    }

    private void ShowCanvasWithAnimation(GameObject canvas, CanvasGroup canvasGroup)
    {
        if (canvas == null || canvasGroup == null) return;

        if (_currentAnimation != null)
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }

        canvas.SetActive(true);
        canvasGroup.alpha = 0f;
        var rectTransform = canvas.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * 0.95f;

        _currentAnimation = DOTween.Sequence();

        _currentAnimation.Join(canvasGroup.DOFade(1f, _fadeDuration))
            .Join(rectTransform.DOScale(1f, _fadeDuration))
            .SetEase(_fadeInEase)
            .SetUpdate(true);
    }

    private void HideSettingsWithAnimation(Action onComplete = null)
    {
        HideCanvasWithAnimation(_settingsCanvas, _settingsCanvasGroup, onComplete);
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}