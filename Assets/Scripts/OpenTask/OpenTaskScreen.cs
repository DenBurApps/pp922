using System;
using TaskListScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OpenTask
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
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

        private TaskPlane _taskPlaneToEdit;
        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action BackClicked;
        public event Action<TaskPlane> EditClicked; 
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
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
        }

        public void EnableScreen(TaskPlane taskPlane)
        {
            _screenVisabilityHandler.EnableScreen();

            if (taskPlane == null)
                throw new ArgumentNullException(nameof(taskPlane));

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
            EditClicked?.Invoke(_taskPlaneToEdit);
            _screenVisabilityHandler.DisableScreen();
        }

        private void OnBackClicked()
        {
            BackClicked?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
