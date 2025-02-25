using System;
using DG.Tweening;
using TaskListScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OpenTask
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    [RequireComponent(typeof(CanvasGroup))]
    public class OpenTaskScreen : MonoBehaviour
    {
        private const string CommentNotAddedText = "Comment not added";
        
        [SerializeField] private Sprite _completeSprite;
        [SerializeField] private Sprite _notCompleteSprite;
        
        [SerializeField] private TaskPlane _taskPlaneToFill;
        [SerializeField] private TMP_Text _commentText;
        [SerializeField] private Button _markButton;
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _backButton;

        [Header("Animation Settings")]
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private Ease _showEase = Ease.OutBack;
        [SerializeField] private Ease _hideEase = Ease.InBack;
        
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Sequence _currentAnimation;
        private TaskPlane _taskPlaneToEdit;
        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action BackClicked;
        public event Action<TaskPlane> EditClicked; 
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _markButton.onClick.AddListener(OnMarkButtonClicked);
            _editButton.onClick.AddListener(OnEditClicked);
            _backButton.onClick.AddListener(OnBackClicked);
        }

        private void OnDisable()
        {
            _markButton.onClick.RemoveListener(OnMarkButtonClicked);
            _editButton.onClick.RemoveListener(OnEditClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
        }

        private void Start()
        {
            _screenVisabilityHandler.DisableScreen();
            
            // Установка начальных значений для анимации
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
            }
            
            if (_rectTransform != null)
            {
                _rectTransform.localScale = Vector3.one * 0.9f;
            }
        }
        
        private void PlayHideAnimation(TweenCallback onComplete = null)
        {
            // Проверка наличия необходимых компонентов
            if (_canvasGroup == null || _rectTransform == null) 
            {
                Debug.LogError("Required components for animation not found!");
                onComplete?.Invoke();
                return;
            }
            
            _currentAnimation?.Kill();

            _currentAnimation = DOTween.Sequence();
        
            _currentAnimation.Join(_canvasGroup.DOFade(0f, _animationDuration))
                .Join(_rectTransform.DOScale(0.9f, _animationDuration))
                .SetEase(_hideEase)
                .OnComplete(() => {
                    onComplete?.Invoke();
                })
                .SetUpdate(true);
        }
        
        private void PlayShowAnimation()
        {
            // Проверка наличия необходимых компонентов
            if (_canvasGroup == null || _rectTransform == null) 
            {
                Debug.LogError("Required components for animation not found!");
                return;
            }
            
            _currentAnimation?.Kill();
        
            _canvasGroup.alpha = 0f;
            _rectTransform.localScale = Vector3.one * 0.9f;

            _currentAnimation = DOTween.Sequence();
        
            _currentAnimation.Join(_canvasGroup.DOFade(1f, _animationDuration))
                .Join(_rectTransform.DOScale(1f, _animationDuration))
                .SetEase(_showEase)
                .SetUpdate(true);
        }

        public void EnableScreen(TaskPlane taskPlane)
        {
            if (taskPlane == null)
                throw new ArgumentNullException(nameof(taskPlane));
                
            _screenVisabilityHandler.EnableScreen();
            PlayShowAnimation();

            _taskPlaneToEdit = taskPlane;
            _taskPlaneToFill.EnablePlane(_taskPlaneToEdit.TaskData);

            _commentText.text = string.IsNullOrEmpty(_taskPlaneToEdit.TaskData.Comment)
                ? CommentNotAddedText
                : _taskPlaneToEdit.TaskData.Comment;
            
            _markButton.image.sprite = _taskPlaneToEdit.TaskData.IsComplete ? _completeSprite : _notCompleteSprite;
        }

        private void OnMarkButtonClicked()
        {
            if (_taskPlaneToEdit.TaskData.IsComplete)
            {
                _markButton.image.sprite = _notCompleteSprite;
                _taskPlaneToEdit.SetCompleteStatus(false);
            }
            else
            {
                _markButton.image.sprite = _completeSprite;
                _taskPlaneToEdit.SetCompleteStatus(true);
            }
        }

        private void OnEditClicked()
        {
            PlayHideAnimation(() => {
                EditClicked?.Invoke(_taskPlaneToEdit);
                _screenVisabilityHandler.DisableScreen();
            });
        }

        private void OnBackClicked()
        {
            PlayHideAnimation(() => {
                BackClicked?.Invoke();
                _screenVisabilityHandler.DisableScreen();
            });
        }
    }
}